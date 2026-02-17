using UnityEngine;

/// <summary>
/// Implementación del minion tanque.
/// Características:
/// - Vida: Alta (120-180 HP)
/// - Velocidad: Muy lenta (0.3x-0.5x)
/// - Comportamiento: Resistente, controla espacio
/// - Movimiento: Lento pero imparable
/// </summary>
[CreateAssetMenu(fileName = "TankMinion", menuName = "Minions/Tank Minion")]
public class TankMinion : MinionSO
{
    [Header("Tank Specific")]
    [SerializeField] private float damageReduction = 0.2f; // 20% de reducción de daño

    public override MinionType Type => MinionType.Tank;

    private void OnEnable()
    {
        // Configuración específica del minion tanque
        maxHealth = 150f; // Vida alta
        moveSpeed = 1.5f; // Velocidad muy lenta (0.45x de velocidad base)
        attackDamage = 30f; // Daño alto
        attackRange = 2f; // Mayor rango de ataque
        attackCooldown = 2f; // Cooldown más largo
        enfoqueReward = 30f; // Mayor recompensa por dificultad
        detectionRange = 7f;
    }

    /// <summary>
    /// Movimiento lento pero constante hacia el jugador.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Para el Tank: movimiento lento pero constante
    }

    /// <summary>
    /// Aplica daño reducido por la resistencia del tanque.
    /// </summary>
    public override void TakeDamage(float amount)
    {
        if (!isAlive) return;

        // Aplica reducción de daño
        float reducedDamage = amount * (1f - damageReduction);
        currentHealth -= reducedDamage;
        InvokeOnMinionHit(reducedDamage);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    public override void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        InvokeOnMinionAttack();
        
        // Ataque pesado con alto daño
    }
}
