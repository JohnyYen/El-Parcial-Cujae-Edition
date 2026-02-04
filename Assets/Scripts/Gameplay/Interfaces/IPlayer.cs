using System;

namespace ElParcialCujae.Gameplay
{
    /// <summary>
    /// Interfaz base para el jugador del juego.
    /// Define el comportamiento genérico que el jugador debe implementar:
    /// - Movimiento con teclado (WASD/Flechas)
    /// - Sistema de dash y salto con cooldowns
    /// - Sistema de estrés (muere al 100%)
    /// - Sistema de enfoque para super ataques
    /// </summary>
    public interface IPlayer
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

        // ========== MÉTODOS ==========

        /// <summary>
        /// Mueve al jugador en la dirección especificada.
        /// </summary>
        /// <param name="direction">Dirección de movimiento (-1, 0, 1 en eje X)</param>
        void Move(float direction);

        /// <summary>
        /// Ejecuta un dash en la dirección actual del movimiento.
        /// Solo disponible si CanDash es true.
        /// </summary>
        void Dash();

        /// <summary>
        /// Ejecuta un salto.
        /// Solo disponible si CanJump es true.
        /// </summary>
        void Jump();

        /// <summary>
        /// Agrega estrés al jugador.
        /// Se llama cuando recibe ataques.
        /// </summary>
        /// <param name="amount">Cantidad de estrés a agregar</param>
        void AddStress(float amount);

        /// <summary>
        /// Agrega enfoque al jugador.
        /// Se gana al esquivar ataques o derrotar enemigos.
        /// </summary>
        /// <param name="amount">Cantidad de enfoque a agregar</param>
        void AddEnfoque(float amount);

        /// <summary>
        /// Consume enfoque del jugador.
        /// Se usa para super ataques.
        /// </summary>
        /// <param name="amount">Cantidad de enfoque a consumir</param>
        /// <returns>True si se pudo consumir, false si no hay suficiente</param>
        bool ConsumeEnfoque(float amount);

        /// <summary>
        /// Reduce el estrés del jugador (curación).
        /// </summary>
        /// <param name="amount">Cantidad de estrés a reducir</param>
        void ReduceStress(float amount);

        // ========== EVENTOS ==========

        /// <summary>
        /// Se dispara cuando el jugador muere (estrés = 100%).
        /// </summary>
        event Action OnPlayerDeath;

        /// <summary>
        /// Se dispara cuando el jugador recibe un golpe.
        /// </summary>
        event Action OnPlayerHit;

        /// <summary>
        /// Se dispara cuando el jugador usa dash.
        /// </summary>
        event Action OnDashUsed;

        /// <summary>
        /// Se dispara cuando el jugador usa salto.
        /// </summary>
        event Action OnJumpUsed;

        /// <summary>
        /// Se dispara cuando el estrés cambia.
        /// </summary>
        event Action<float> OnStressChanged;

        /// <summary>
        /// Se dispara cuando el enfoque cambia.
        /// </summary>
        event Action<float> OnEnfoqueChanged;

        /// <summary>
        /// Se dispara cuando el dash se regenera (cooldown completado).
        /// </summary>
        event Action OnDashRefreshed;

        /// <summary>
        /// Se dispara cuando el estado del jugador cambia.
        /// </summary>
        event Action<PlayerState> OnStateChanged;
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
}
