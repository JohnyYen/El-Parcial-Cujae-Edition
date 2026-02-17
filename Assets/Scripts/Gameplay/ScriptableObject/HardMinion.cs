using UnityEngine;

/// <summary>
/// Implementación del minion difícil (Hard).
/// Características:
/// - Vida: Alta (100-150 HP)
/// - Velocidad: Lenta (1.5-2 unidades)
/// - Comportamiento: Resistente con reducción de daño
/// - Movimiento: Lento pero imparable
/// </summary>
[CreateAssetMenu(fileName = "HardMinion", menuName = "Minions/Hard Minion")]
public class HardMinion : MinionSO
{
    [Header("Hard Specific")]
    [SerializeField] private float damageReduction = 0.25f; // 25% de reducción de daño

    public override MinionType Type => MinionType.Hard;

    private void OnEnable()
    {
        // Configuración específica del minion difícil
        maxHealth = 120f;
        moveSpeed = 1.8f;
        attackDamage = 30f;
        attackRange = 2f;
        attackCooldown = 1.8f;
        enfoqueReward = 35f;
        detectionRange = 8f;
    }

    /// <summary>
    /// Movimiento lento pero constante hacia el jugador.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Para el Hard: movimiento lento pero constante
    }

    /// <summary>
    /// Aplica daño reducido por la resistencia del minion hard.
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
