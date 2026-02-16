using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de proyectiles.
/// Los proyectiles se disparan en patrón radial hacia el jugador.
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

        // Posición de spawn (normalmente desde la posición del boss)
        Vector3 spawnPos = Vector3.zero;
        float angle = (360f / projectileCount) * index;
        
        // Crear dirección basada en ángulo
        float radians = angle * Mathf.Deg2Rad;
        Vector3 direction = new Vector3(Mathf.Cos(radians), Mathf.Sin(radians), 0).normalized;

        // Instanciar proyectil
        GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        
        // Configurar velocidad si tiene Rigidbody
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }

        Debug.Log($"Proyectil spawn #{index + 1} - Ángulo: {angle}°");
    }
}
