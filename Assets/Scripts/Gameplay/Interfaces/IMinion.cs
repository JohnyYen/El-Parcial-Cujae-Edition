using System;

/// <summary>
/// Interfaz base para cualquier minion del juego.
/// Define el comportamiento genérico que todo minion debe implementar:
/// - Sistema de salud variable según dificultad
/// - Tipos de movimiento (persecución o patrol)
/// - Sistema de ataque al jugador
/// - Recompensa de enfoque al ser derrotado
/// </summary>
public interface IMinion
{
    // ========== PROPIEDADES ==========

    /// <summary>
    /// Tipo específico del minion.
    /// Determina características como HP, velocidad y comportamiento.
    /// </summary>
    MinionType Type { get; }

    /// <summary>
    /// Salud actual del minion.
    /// </summary>
    float Health { get; }

    /// <summary>
    /// Salud máxima del minion.
    /// Varía según el tipo: Basic (bajo), Fast (muy bajo), Tank (alto), Ranged (medio).
    /// </summary>
    float MaxHealth { get; }

    /// <summary>
    /// Indica si el minion está vivo.
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// Cantidad de enfoque que otorga al jugador al ser derrotado.
    /// Depende de la dificultad del minion.
    /// </summary>
    float EnfoqueReward { get; }

    // ========== MÉTODOS ==========

    /// <summary>
    /// Aplica daño al minion.
    /// </summary>
    /// <param name="amount">Cantidad de daño a aplicar</param>
    void TakeDamage(float amount);

    /// <summary>
    /// Ejecuta un ataque contra el jugador.
    /// </summary>
    void Attack();

    /// <summary>
    /// Persigue al jugador en dirección a su posición actual.
    /// Utilizado por minions Basic, Fast, Tank.
    /// </summary>
    void MoveTowardsPlayer();

    /// <summary>
    /// Patrol entre puntos definidos.
    /// Utilizado por minions sentinela o que mantienen posiciones.
    /// </summary>
    void Patrol();

    /// <summary>
    /// Ejecuta la muerte del minion.
    /// Otorga enfoque al jugador y destruye el objeto.
    /// </summary>
    void Die();

    /// <summary>
    /// Inicializa el comportamiento específico del minion.
    /// Se llama al spawear el minion.
    /// </summary>
    void Initialize();

    // ========== EVENTOS ==========

    /// <summary>
    /// Se dispara cuando el minion recibe daño.
    /// </summary>
    event Action<float> OnMinionHit;

    /// <summary>
    /// Se dispara cuando el minion muere.
    /// Útil para otorgar enfoque al jugador.
    /// </summary>
    event Action OnMinionDeath;

    /// <summary>
    /// Se dispara cuando el minion ataca al jugador.
    /// </summary>
    event Action OnMinionAttack;

    /// <summary>
    /// Se dispara cuando el minion es spawneado.
    /// </summary>
    event Action OnMinionSpawned;
}
