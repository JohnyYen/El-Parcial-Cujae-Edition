using UnityEngine;

/// <summary>
/// Implementación del minion básico.
/// Características:
/// - Vida: 30-50 HP
/// - Velocidad: Lenta (0.5x-0.7x)
/// - Comportamiento: Follow básico, ataque cuerpo a cuerpo simple
/// - Movimiento: Lineal directo hacia el jugador
/// </summary>
[CreateAssetMenu(fileName = "BasicMinion", menuName = "Minions/Basic Minion")]
public class BasicMinion : MinionSO
{
    public override MinionType Type => MinionType.Basic;

    private void OnEnable()
    {
        // Configuración específica del minion básico
        maxHealth = 50f;
        moveSpeed = 2f; // Velocidad lenta (0.6x de velocidad base)
        attackDamage = 10f;
        attackRange = 1.5f;
        attackCooldown = 1.5f;
        enfoqueReward = 10f;
        detectionRange = 8f;
    }

    /// <summary>
    /// Movimiento simple y directo hacia el jugador.
    /// Sin evasión ni predicción.
    /// </summary>
    public override void MoveTowardsPlayer()
    {
        if (!isAlive) return;
        
        // El movimiento real se maneja en el MonoBehaviour
        // Este método solo define la estrategia de movimiento
        // Para el Basic: movimiento lineal directo (implementado en Minion.cs)
    }

    public override void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        InvokeOnMinionAttack();
        
        // Ataque simple sin efectos especiales
        // El daño se aplica desde el MonoBehaviour
    }
}
