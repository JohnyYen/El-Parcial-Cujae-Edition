using UnityEngine;

/// <summary>
/// Implementación del minion medio (Medium).
/// Características:
/// - Vida: Media (60-80 HP)
/// - Velocidad: Media (3-4 unidades)
/// - Comportamiento: Equilibrado entre ataque y velocidad
/// - Movimiento: Directo con velocidad moderada
/// </summary>
[CreateAssetMenu(fileName = "MediumMinion", menuName = "Minions/Medium Minion")]
public class MediumMinion : MinionSO
{
    public override MinionType Type => MinionType.Medium;

    private void OnEnable()
    {
        // Configuración específica del minion medio
        maxHealth = 70f;
        moveSpeed = 3.5f;
        attackDamage = 20f;
        attackRange = 1.5f;
        attackCooldown = 1.2f;
        enfoqueReward = 20f;
        detectionRange = 9f;
    }

    /// <summary>
    /// Movimiento equilibrado hacia el jugador.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Para el Medium: movimiento equilibrado
    }

    public override void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        InvokeOnMinionAttack();
        
        // Ataque equilibrado
    }
}
