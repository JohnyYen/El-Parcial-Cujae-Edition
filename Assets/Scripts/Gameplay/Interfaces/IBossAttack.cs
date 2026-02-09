using System;

namespace ElParcialCujae.Gameplay
{
    using ElParcialCujae.Gameplay.Enum;

    /// <summary>
    /// Interfaz que define los patrones de ataque específicos que el boss puede ejecutar.
    /// Permite compositionar diferentes tipos de ataques con cooldown, daño y validación por fase.
    /// </summary>
    public interface IBossAttack
    {
        // ========== IDENTIFICACIÓN ==========

        /// <summary>
        /// Nombre identificador del ataque.
        /// Útil para debugging y UI.
        /// </summary>
        string AttackName { get; }

        /// <summary>
        /// Daño que inflict el ataque al jugador.
        /// </summary>
        float Damage { get; }

        // ========== EJECUCIÓN ==========

        /// <summary>
        /// Ejecuta el ataque del boss.
        /// Debe verificar cooldown y fase antes de ejecutar.
        /// </summary>
        void Execute();

        /// <summary>
        /// Indica si el ataque está actualmente en progreso.
        /// </summary>
        bool IsInProgress { get; }

        // ========== COOLDOWN ==========

        /// <summary>
        /// Tiempo de cooldown del ataque en segundos.
        /// </summary>
        float Cooldown { get; }

        /// <summary>
        /// Timestamp del último ataque ejecutado.
        /// </summary>
        float LastAttackTime { get; }

        /// <summary>
        /// Verifica si el cooldown ha pasado.
        /// </summary>
        bool CanExecute { get; }

        // ========== CONFIGURACIÓN POR FASE ==========

        /// <summary>
        /// Verifica si el ataque es válido para la fase especificada.
        /// </summary>
        /// <param name="phase">Fase del boss a verificar</param>
        /// <returns>True si el ataque puede ejecutarse en esta fase</returns>
        bool IsValidForPhase(BossPhase phase);

        // ========== EVENTOS ==========

        /// <summary>
        /// Se dispara cuando el ataque comienza a ejecutarse.
        /// </summary>
        event Action OnAttackStarted;

        /// <summary>
        /// Se dispara cuando el ataque termina.
        /// </summary>
        event Action OnAttackEnded;

        /// <summary>
        /// Se dispara cuando el ataque impacta al jugador.
        /// </summary>
        event Action<float> OnAttackHitPlayer;
    }
}
