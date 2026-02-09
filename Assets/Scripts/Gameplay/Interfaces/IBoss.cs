using System;
using ElParcialCujae.Gameplay.Enum;
namespace ElParcialCujae.Gameplay
{
    /// <summary>
    /// Interfaz base para cualquier boss del juego.
    /// Define el comportamiento genérico que todo boss debe implementar:
    /// - Gestión de fases (3 fases fijas)
    /// - Sistema de salud con eventos
    /// - Tipos de ataques (Projectile, Melee, Area)
    /// - Spawn de minions
    /// </summary>
    public interface IBoss
    {
        // ========== PROPIEDADES ==========

        /// <summary>
        /// Fase actual del boss (1, 2 o 3)
        /// </summary>
        int CurrentPhase { get; }

        /// <summary>
        /// HP máximo del boss
        /// </summary>
        float MaxHealth { get; }

        /// <summary>
        /// HP actual del boss
        /// </summary>
        float CurrentHealth { get; }

        /// <summary>
        /// Indica si el boss está vivo
        /// </summary>
        bool IsAlive { get; }

        // ========== MÉTODOS ==========

        /// <summary>
        /// Aplica daño al boss.
        /// Debe reducir CurrentHealth y verificar cambio de fase.
        /// </summary>
        /// <param name="amount">Cantidad de daño a aplicar</param>
        void TakeDamage(float amount);

        /// <summary>
        /// Cambia manualmente la fase del boss.
        /// Se llama automáticamente cuando HP baja del umbral.
        /// </summary>
        /// <param name="phase">Nueva fase (1, 2 o 3)</param>
        void ChangePhase(int phase);

        /// <summary>
        /// Ejecuta un tipo de ataque específico.
        /// </summary>
        /// <param name="type">Tipo de ataque a ejecutar</param>
        void PerformAttack(AttackType type);

        /// <summary>
        /// Spawn un minion en el campo de batalla.
        /// </summary>
        /// <param name="type">Tipo de minion a spawnear</param>
        void SpawnMinion(MinionType type);

        // ========== EVENTOS ==========

        /// <summary>
        /// Se dispara cuando el HP cambia.
        /// </summary>
        event Action<float> OnHealthChanged;

        /// <summary>
        /// Se dispara cuando el boss cambia de fase.
        /// </summary>
        event Action<int> OnPhaseChanged;

        /// <summary>
        /// Se dispara cuando el boss muere.
        /// </summary>
        event Action OnBossDeath;

        /// <summary>
        /// Se dispara cuando el boss ejecuta un ataque.
        /// </summary>
        event Action<AttackType> OnAttack;

        /// <summary>
        /// Se dispara cuando un minion es spawneado.
        /// </summary>
        event Action<MinionType> OnMinionSpawned;
    }
}