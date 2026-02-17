using UnityEngine;

/// <summary>
/// Implementación del minion a distancia.
/// Características:
/// - Vida: Media (60-100 HP)
/// - Velocidad: Normal (0.8x-1.0x)
/// - Comportamiento: Mantiene distancia y ataca a rango
/// - Movimiento: Combinación de seguimiento y evasión
/// </summary>
[CreateAssetMenu(fileName = "RangedMinion", menuName = "Minions/Ranged Minion")]
public class RangedMinion : MinionSO
{
    [Header("Ranged Specific")]
    [SerializeField] private float preferredDistance = 5f; // Distancia preferida del jugador
    [SerializeField] private float retreatDistance = 3f; // Si está más cerca, retrocede

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileSpeed = 8f;

    public override MinionType Type => MinionType.Ranged;

    public float PreferredDistance => preferredDistance;
    public float RetreatDistance => retreatDistance;
    public GameObject ProjectilePrefab => projectilePrefab;
    public float ProjectileSpeed => projectileSpeed;

    private void OnEnable()
    {
        // Configuración específica del minion a distancia
        maxHealth = 70f; // Vida media
        moveSpeed = 3f; // Velocidad normal (0.9x de velocidad base)
        attackDamage = 15f;
        attackRange = 6f; // Rango de ataque a distancia
        attackCooldown = 2f; // Cooldown entre disparos
        enfoqueReward = 20f;
        detectionRange = 10f; // Gran rango de detección
        
        preferredDistance = 5f;
        retreatDistance = 3f;
    }

    /// <summary>
    /// Movimiento que mantiene distancia óptima del jugador.
    /// Se acerca si está lejos, retrocede si está cerca.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Para el Ranged: mantiene distancia óptima
    }

    public override void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        InvokeOnMinionAttack();
        
        // Ataque a distancia (dispara proyectil)
        // La instanciación del proyectil se hace en el MonoBehaviour
    }

    /// <summary>
    /// Verifica si debe retroceder (jugador muy cerca).
    /// </summary>
    public bool ShouldRetreat(Transform minionTransform, Transform playerTransform)
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(minionTransform.position, playerTransform.position);
        return distance < retreatDistance;
    }

    /// <summary>
    /// Verifica si está en distancia óptima.
    /// </summary>
    public bool IsAtOptimalDistance(Transform minionTransform, Transform playerTransform)
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(minionTransform.position, playerTransform.position);
        return Mathf.Abs(distance - preferredDistance) < 1f;
    }
}
