using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewBossAttack", menuName = "Boss Attack")]
public abstract class BossAttackSO : ScriptableObject, IBossAttack
{
    // ========== IDENTIFICACIÓN ==========

    public abstract string AttackName { get; }
    public abstract AttackType Type { get; }
    public abstract float Damage { get; }

    // ========== EJECUCIÓN ==========

    /// <summary>
    /// Ejecuta el ataque sin posición específica (compatibilidad hacia atrás).
    /// </summary>
    public abstract void Execute();

    /// <summary>
    /// Ejecuta el ataque desde una posición específica del boss.
    /// Sobrescribir este método en ataques que necesiten la posición (ej: Melee).
    /// </summary>
    /// <param name="bossPosition">Posición actual del boss en el mundo</param>
    public virtual void Execute(Vector2 bossPosition)
    {
        // Por defecto, llama al Execute sin parámetros
        Execute();
    }

    public abstract bool IsInProgress { get; }

    // ========== COOLDOWN ==========

    public abstract float Cooldown { get; }
    public abstract float LastAttackTime { get; }
    public abstract bool CanExecute { get; }

    // ========== CONFIGURACIÓN POR FASE ==========

    public abstract bool IsValidForPhase(BossPhase phase);

    // ========== EVENTOS ==========

    public abstract event Action OnAttackStarted;
    public abstract event Action OnAttackEnded;
    public abstract event Action<float> OnAttackHitPlayer;
}