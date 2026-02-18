using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de lluvia de proyectiles.
/// Los proyectiles caen desde arriba de la pantalla hacia la posición del jugador.
/// Temático: "Lluvia de Hojas" - como hojas de examen cayendo.
/// </summary>
[CreateAssetMenu(fileName = "RainAttack", menuName = "Boss Attacks/Rain")]
public class RainAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Lluvia de Hojas";
    [SerializeField] private float damage = 10f;
    [SerializeField] private float cooldown = 6f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Rain Settings")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int projectileCount = 6;
    [SerializeField] private float spawnHeight = 8f; // Altura de spawn sobre el jugador
    [SerializeField] private float spawnWidth = 5f; // Ancho del área de lluvia
    [SerializeField] private float projectileSpeed = 8f;

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;

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
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName}");

        // Obtener posición del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No se encontró al jugador para RainAttack!");
            isInProgress = false;
            OnAttackEnded?.Invoke();
            return;
        }

        // Ejecutar lluvia de proyectiles
        SpawnRainProjectiles(player.transform.position);

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

    private void SpawnRainProjectiles(Vector2 playerPosition)
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile prefab no está asignado en RainAttack!");
            return;
        }

        for (int i = 0; i < projectileCount; i++)
        {
            // Posición de spawn: arriba del jugador + offset aleatorio en X
            float randomOffsetX = UnityEngine.Random.Range(-spawnWidth / 2f, spawnWidth / 2f);
            Vector2 spawnPos = new Vector2(
                playerPosition.x + randomOffsetX,
                playerPosition.y + spawnHeight
            );

            // Instanciar proyectil
            GameObject projectile = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

            // Configurar velocidad hacia abajo
            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.down * projectileSpeed;
            }

            // Pasar daño al proyectil
            BossProjectile bossProj = projectile.GetComponent<BossProjectile>();
            if (bossProj != null)
            {
                bossProj.SetDamage(damage);
            }

            Debug.Log($"Proyectil lluvia #{i + 1} - Pos: {spawnPos}, Daño: {damage}");
        }
    }
}
