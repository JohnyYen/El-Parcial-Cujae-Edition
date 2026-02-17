using UnityEngine;

/// <summary>
/// Implementación del minion rápido.
/// Características:
/// - Vida: Muy baja (20-30 HP)
/// - Velocidad: Muy rápida (1.2x-1.5x)
/// - Comportamiento: Agresivo con movimientos rápidos
/// - Movimiento: Directo pero con mayor velocidad
/// </summary>
[CreateAssetMenu(fileName = "FastMinion", menuName = "Minions/Fast Minion")]
public class FastMinion : MinionSO
{
    public override MinionType Type => MinionType.Fast;

    private void OnEnable()
    {
        // Configuración específica del minion rápido
        maxHealth = 25f; // Vida muy baja
        moveSpeed = 5f; // Velocidad muy rápida (1.5x de velocidad base)
        attackDamage = 15f;
        attackRange = 1.2f;
        attackCooldown = 0.8f; // Ataca más frecuentemente
        enfoqueReward = 15f; // Más enfoque por ser más difícil de golpear
        detectionRange = 10f; // Mayor rango de detección
    }

    /// <summary>
    /// Movimiento rápido y agresivo hacia el jugador.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Para el Fast: movimiento directo pero muy rápido
    }

    public override void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        InvokeOnMinionAttack();
        
        // Ataque rápido con menor cooldown
    }
}
