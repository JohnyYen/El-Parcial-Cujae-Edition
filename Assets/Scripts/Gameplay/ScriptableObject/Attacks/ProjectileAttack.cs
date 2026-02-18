using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de proyectiles.
/// Los proyectiles se disparan en un cono de 45° hacia la izquierda (hacia el jugador).
/// </summary>
[CreateAssetMenu(fileName = "ProjectileAttack", menuName = "Boss Attacks/Projectile")]
public class ProjectileAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Lluvia de Proyectiles";
    [SerializeField] private float damage = 20f;
    [SerializeField] private float cooldown = 2f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase1, BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCount = 3;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float spawnDelay = 0f; // Delay entre proyectiles (0 = todos a la vez)

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;
    private Vector2 lastBossPosition;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
    public override AttackType Type => AttackType.Projectile;
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

        // Lógica de disparo de proyectiles
        for (int i = 0; i < projectileCount; i++)
        {
            SpawnProjectile(i);
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

    private void SpawnProjectile(int index)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab no está asignado en ProjectileAttack!");
            return;
        }

        // Posición de spawn desde la posición del boss
        Vector2 spawnPos = lastBossPosition;

        // Calcular ángulo en un cono de 45° apuntando hacia la izquierda (hacia el jugador)
        // Ángulo base: 180° (izquierda)
        // Spread: ±22.5° (cono de 45° total)
        float baseAngle = 180f; // Dirección hacia la izquierda
        float spreadAngle;
        
        if (projectileCount == 1)
        {
            spreadAngle = 0f; // Disparo recto hacia la izquierda
        }
        else
        {
            // Distribuir uniformemente entre -22.5° y +22.5°
            spreadAngle = -22.5f + (45f / (projectileCount - 1)) * index;
        }
        
        float finalAngle = baseAngle + spreadAngle;
        
        // Crear dirección basada en ángulo final y normalizar explícitamente
        float radians = finalAngle * Mathf.Deg2Rad;
        Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

        // Instanciar proyectil
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        // Inicializar el proyectil usando su método Initialize
        BossProjectile bossProj = projectile.GetComponent<BossProjectile>();
        if (bossProj != null)
        {
            // Usar el método Initialize que configura todo internamente
            bossProj.Initialize(direction, projectileSpeed, damage);
            Debug.Log($"Proyectil #{index + 1} - Ángulo: {finalAngle}°, Dirección: {direction}, Speed: {projectileSpeed}");
        }
        else
        {
            Debug.LogError($"¡BossProjectile component NO encontrado en el prefab! El proyectil no funcionará correctamente.");
        }
    }
}
