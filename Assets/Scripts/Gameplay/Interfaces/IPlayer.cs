using System;

/// <summary>
/// Interfaz base para el jugador del juego.
/// Define el comportamiento genérico que el jugador debe implementar:
/// - Movimiento con teclado (WASD/Flechas)
/// - Sistema de dash y salto con cooldowns
/// - Sistema de estrés (muere al 100%)
/// - Sistema de enfoque para super ataques
/// </summary>
public interface IPlayerProperties
{
    // ========== PROPIEDADES ==========

    /// <summary>
    /// Indica si el jugador está vivo.
    /// </summary>
    bool IsAlive { get; }

    /// <summary>
    /// Nivel de estrés actual del jugador (0-100%).
    /// Muere cuando llega a 100%.
    /// </summary>
    float Stress { get; }

    /// <summary>
    /// Nivel de enfoque actual del jugador.
    /// Se usa para super ataques.
    /// </summary>
    float Enfoque { get; }

    /// <summary>
    /// Indica si el dash está disponible.
    /// Se desactiva al usar dash y se reactiva después del cooldown.
    /// </summary>
    bool CanDash { get; }

    /// <summary>
    /// Indica si el salto está disponible.
    /// Se desactiva al usar y se reactiva al tocar suelo.
    /// </summary>
    bool CanJump { get; }

    /// <summary>
    /// Estado actual del jugador.
    /// </summary>
    PlayerState CurrentState { get; }
}

/// <summary>
/// Representa los posibles estados del jugador.
/// </summary>
public enum PlayerState
{
    Idle,
    Moving,
    Jumping,
    Dashing,
    Attacking,
    Stunned,
    Dead
}
