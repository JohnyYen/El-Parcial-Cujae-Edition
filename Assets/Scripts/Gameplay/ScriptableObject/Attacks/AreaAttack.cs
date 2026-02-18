using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de área.
/// Crea una zona de daño expansiva desde la posición del boss.
/// </summary>
[CreateAssetMenu(fileName = "AreaAttack", menuName = "Boss Attacks/Area")]
public class AreaAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Explosión de Área";
    [SerializeField] private float damage = 25f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase1, BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Area Settings")]
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private float areaRadius = 5f;
    [SerializeField] private float areaDuration = 2f;
    [SerializeField] private float initialRadiusScale = 0.2f; // 20% del tamaño final
    [SerializeField] private float delayBeforeExpansion = 0.3f; // Tiempo que permanece pequeña antes de expandirse
    [SerializeField] private float expansionDuration = 0.8f; // Tiempo de expansión
    [Header("Spawn Area Settings")]
    [SerializeField] private float minDistanceLeft = 2f; // Distancia mínima a la izquierda del jefe
    [SerializeField] private float maxDistanceLeft = 10f; // Distancia máxima a la izquierda del jefe
    [SerializeField] private float verticalVariation = 5f; // Variación vertical desde la posición del jefe

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;
    private Vector2 lastBossPosition;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
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

        Debug.Log($"Boss ejecuta: {attackName} desde {bossPosition}");

        // Crear área de daño
        CreateDamageArea();

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

        // Generar posición aleatoria a la izquierda del boss
        Vector2 spawnPos = GenerateRandomPositionLeft();

        // Instanciar área de daño
        GameObject areaObj = Instantiate(areaPrefab, spawnPos, Quaternion.identity);

        // Configurar tamaño si es un círculo
        CircleCollider2D circleCollider = areaObj.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = areaRadius;
        }

        // Pasar daño y configuración de expansión a la zona desde este script de ataque
        DamageZone damageZone = areaObj.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.SetDamage(damage);
            damageZone.SetExpansion(areaRadius * initialRadiusScale, areaRadius, expansionDuration, delayBeforeExpansion);
        }
        else
        {
            Debug.LogWarning("DamageZone component not found on area prefab!");
        }

        // Destruir después de la duración
        Destroy(areaObj, areaDuration);

        Debug.Log($"Área de daño creada en {spawnPos} - Radius: {areaRadius}, Duration: {areaDuration}s, Daño: {damage}");
    }

    /// <summary>
    /// Genera una posición aleatoria a la izquierda del jefe.
    /// </summary>
    private Vector2 GenerateRandomPositionLeft()
    {
        // Distancia aleatoria a la izquierda
        float randomDistanceLeft = UnityEngine.Random.Range(minDistanceLeft, maxDistanceLeft);
        
        // Variación vertical aleatoria
        float randomVerticalOffset = UnityEngine.Random.Range(-verticalVariation, verticalVariation);
        
        // Calcular posición final
        Vector2 randomPosition = new Vector2(
            lastBossPosition.x - randomDistanceLeft,  // A la izquierda del jefe
            lastBossPosition.y + randomVerticalOffset  // Con variación vertical
        );
        
        return randomPosition;
    }
}
