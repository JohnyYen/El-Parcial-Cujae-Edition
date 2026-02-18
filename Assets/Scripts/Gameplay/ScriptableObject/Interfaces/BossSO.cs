using UnityEngine;
using System;

public abstract class BossSO : ScriptableObject, IBoss
{
    // ========== PROPIEDADES ==========

    public abstract int CurrentPhase { get; }
    public abstract float MaxHealth { get; }
    public abstract float CurrentHealth { get; }
    public abstract bool IsAlive { get; }

    // ========== MÉTODOS ==========

    public abstract void TakeDamage(float amount);
    public abstract void ChangePhase(int phase);

    /// <summary>
    /// Ejecuta un ataque sin pasar la posición del boss (compatibilidad).
    /// </summary>
    public abstract void PerformAttack(AttackType type);

    /// <summary>
    /// Ejecuta un ataque pasando la posición del boss.
    /// Necesario para ataques como Melee que necesitan saber dónde está el boss.
    /// </summary>
    public abstract void PerformAttack(AttackType type, Vector2 bossPosition);

    public abstract void SpawnMinion(MinionType type);

    // ========== EVENTOS ==========

    public abstract event Action<float> OnHealthChanged;
    public abstract event Action<int> OnPhaseChanged;
    public abstract event Action OnBossDeath;
    public abstract event Action<AttackType> OnAttack;
    public abstract event Action<MinionType> OnMinionSpawned;
}