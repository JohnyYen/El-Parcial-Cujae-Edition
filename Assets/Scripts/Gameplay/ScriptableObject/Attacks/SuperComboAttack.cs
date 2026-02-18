using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Implementación de Super Combo (Fase 3).
/// Ejecuta una secuencia de 3 ataques seguidos sin pausa.
/// Representa el último recurso del boss.
/// </summary>
[CreateAssetMenu(fileName = "SuperComboAttack", menuName = "Boss Attacks/Super Combo")]
public class SuperComboAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Super Combo";
    [SerializeField] private float damage = 60f;
    [SerializeField] private float cooldown = 10f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase3 };

    [Header("Combo Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject areaPrefab;
    [SerializeField] private int comboHits = 3;
    [SerializeField] private float timeBetweenHits = 0.5f;
    [SerializeField] private float projectileSpeed = 12f;

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

        Debug.Log($"Boss ejecuta: {attackName} - ¡Secuencia de {comboHits} ataques!");

        // Ejecutar combo secuencial (sin corutina ya que ScriptableObjects no pueden ejecutarlas)
        for (int i = 0; i < comboHits; i++)
        {
            ExecuteComboHit(i);
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

    private void ExecuteComboHit(int hitIndex)
    {
        // Alternar entre tipos de ataque según el índice
        switch (hitIndex % 3)
        {
            case 0:
                // Hit 1: Proyectiles en abanico
                SpawnProjectileFan();
                break;
            case 1:
                // Hit 2: Área de daño en el boss
                SpawnAreaAtBoss();
                break;
            case 2:
                // Hit 3: Proyectiles hacia el jugador
                SpawnProjectilesAtPlayer();
                break;
        }

        Debug.Log($"Super Combo hit #{hitIndex + 1} ejecutado");
    }

    private void SpawnProjectileFan()
    {
        if (projectilePrefab == null) return;

        int projectileCount = 5;
        float fanAngle = 120f; // Ángulo total del abanico
        float startAngle = -fanAngle / 2f;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle = startAngle + (fanAngle / (projectileCount - 1)) * i;
            float radians = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)).normalized;

            GameObject projectile = Instantiate(projectilePrefab, lastBossPosition, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }

            BossProjectile bossProj = projectile.GetComponent<BossProjectile>();
            if (bossProj != null)
            {
                bossProj.SetDamage(damage / comboHits);
            }
        }

        Debug.Log($"SuperCombo: Proyectiles en abanico desde {lastBossPosition}");
    }

    private void SpawnAreaAtBoss()
    {
        if (areaPrefab == null) return;

        GameObject area = Instantiate(areaPrefab, lastBossPosition, Quaternion.identity);

        CircleCollider2D circleCollider = area.GetComponent<CircleCollider2D>();
        if (circleCollider != null)
        {
            circleCollider.radius = 4f;
        }

        DamageZone damageZone = area.GetComponent<DamageZone>();
        if (damageZone != null)
        {
            damageZone.SetDamage(damage / comboHits);
        }

        Destroy(area, 2f);

        Debug.Log($"SuperCombo: Área de daño en {lastBossPosition}");
    }

    private void SpawnProjectilesAtPlayer()
    {
        if (projectilePrefab == null) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;

        Vector2 direction = ((Vector2)player.transform.position - lastBossPosition).normalized;

        // 3 proyectiles hacia el jugador con ligero spread
        for (int i = -1; i <= 1; i++)
        {
            float angleOffset = i * 15f * Mathf.Deg2Rad;
            Vector2 spreadDirection = new Vector2(
                direction.x * Mathf.Cos(angleOffset) - direction.y * Mathf.Sin(angleOffset),
                direction.x * Mathf.Sin(angleOffset) + direction.y * Mathf.Cos(angleOffset)
            ).normalized;

            GameObject projectile = Instantiate(projectilePrefab, lastBossPosition, Quaternion.identity);

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = spreadDirection * projectileSpeed * 1.2f;
            }

            BossProjectile bossProj = projectile.GetComponent<BossProjectile>();
            if (bossProj != null)
            {
                bossProj.SetDamage(damage / comboHits);
            }
        }

        Debug.Log($"SuperCombo: Proyectiles hacia jugador");
    }
}
