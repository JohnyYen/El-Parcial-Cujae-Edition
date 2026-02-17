using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de área.
/// Crea una zona de daño expansiva que afecta toda el área de juego.
/// </summary>
[CreateAssetMenu(fileName = "AreaAttack", menuName = "Boss Attacks/Area")]
public class AreaAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Explosión de Área";
    [SerializeField] private float damage = 25f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Area Settings")]
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private float areaRadius = 5f;
    [SerializeField] private float areaDuration = 2f;

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
    public override float Damage => damage;
    public override float Cooldown => cooldown;
    public override float LastAttackTime => lastAttackTime;
    public override bool CanExecute => Time.time - lastAttackTime >= cooldown;
    public override bool IsInProgress => isInProgress;

    // ========== EVENTOS ==========

    public override event Action OnAttackStarted;
    public override event Action OnAttackEnded;
    public override event Action<float> OnAttackHitPlayer;

    // ========== MÉTODOS ==========

    public override void Execute()
    {
        if (!CanExecute)
            return;

        lastAttackTime = Time.time;
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName}");
        
        // Crear área de daño
        CreateDamageArea();

        // Notificar que golpea al jugador
        OnAttackHitPlayer?.Invoke(damage);

        isInProgress = false;
        OnAttackEnded?.Invoke();
    }

    public override bool IsValidForPhase(BossPhase phase)
    {
        foreach (var validPhase in validPhases)
        {
            if (phase == validPhase)
                return true;
        }
        return false;
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void CreateDamageArea()
    {
        if (areaPrefab == null)
        {
            Debug.LogError("Area prefab no está asignado en AreaAttack!");
            return;
        }

        // Posición de spawn (normalmente desde la posición del boss)
        Vector3 spawnPos = Vector3.zero;

        // Instanciar área de daño
        GameObject areaObj = Instantiate(areaPrefab, spawnPos, Quaternion.identity);
        
        // Configurar tamaño si es un círculo
        CircleCollider2D circleCollider = areaObj.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = areaRadius;
        }

        // Pasar daño a la zona desde este script de ataque
        DamageZone damageZone = areaObj.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.SetDamage(damage);
        }
        else
        {
            Debug.LogWarning("DamageZone component not found on area prefab!");
        }

        // Destruir después de la duración
        Destroy(areaObj, areaDuration);

        Debug.Log($"Área de daño creada - Radius: {areaRadius}, Duration: {areaDuration}s, Daño: {damage}");
    }
}
