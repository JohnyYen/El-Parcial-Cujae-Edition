using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque Furia Total (Fase 3).
/// Crea múltiples zonas de daño en áreas aleatorias del escenario.
/// Representa la desesperación final del boss.
/// </summary>
[CreateAssetMenu(fileName = "FuriaTotalAttack", menuName = "Boss Attacks/Furia Total")]
public class FuriaTotalAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Furia Total";
    [SerializeField] private float damage = 40f;
    [SerializeField] private float cooldown = 8f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase3 };

    [Header("Furia Settings")]
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private int areaCount = 5;
    [SerializeField] private float areaRadius = 3f;
    [SerializeField] private float areaDuration = 3f;
    [SerializeField] private float spawnRadius = 10f; // Radio alrededor del boss para spawnear áreas
    [SerializeField] private float spawnDelay = 0.2f; // Delay entre cada área

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;
    private Vector2 lastBossPosition;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
    public override AttackType Type => AttackType.Area;
    public override float Damage => damage;
    public override float Cooldown => cooldown;
    public override float LastAttackTime => lastAttackTime;
    public override bool CanExecute => Time.time - lastAttackTime >= cooldown - 0.01f;
    public override bool IsInProgress => isInProgress;

    // ========== EVENTOS ==========

    public override event Action OnAttackStarted;
    public override event Action OnAttackEnded;
    public override event Action<float> OnAttackHitPlayer;

    // ========== MÉTODOS ==========

    void OnEnable()
    {
        lastAttackTime = -999f;
    }

    public override void Execute()
    {
        Execute(Vector2.zero);
    }

    public override void Execute(Vector2 bossPosition)
    {
        if (!CanExecute)
            return;

        lastAttackTime = Time.time;
        lastBossPosition = bossPosition;
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName} - ¡Múltiples áreas de daño!");

        // Crear múltiples áreas de daño
        for (int i = 0; i < areaCount; i++)
        {
            CreateDamageArea(i);
        }

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

    private void CreateDamageArea(int index)
    {
        if (areaPrefab == null)
        {
            Debug.LogError("Area prefab no está asignado en FuriaTotalAttack!");
            return;
        }

        // Posición aleatoria alrededor del boss
        Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * spawnRadius;
        Vector2 spawnPos = lastBossPosition + randomOffset;

        // Instanciar área de daño
        GameObject areaObj = Instantiate(areaPrefab, spawnPos, Quaternion.identity);

        // Configurar tamaño si es un círculo
        CircleCollider2D circleCollider = areaObj.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = areaRadius;
        }

        // Pasar daño a la zona
        DamageZone damageZone = areaObj.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.SetDamage(damage);
        }

        // Destruir después de la duración
        Destroy(areaObj, areaDuration);

        Debug.Log($"FuriaTotal área #{index + 1} - Pos: {spawnPos}, Radius: {areaRadius}");
    }
}
