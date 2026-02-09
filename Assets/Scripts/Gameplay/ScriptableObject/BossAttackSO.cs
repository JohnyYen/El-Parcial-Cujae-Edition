using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewBossAttack", menuName = "Boss Attack")]
public abstract class BossAttackSO : ScriptableObject, IBossAttack
{
    // ========== IDENTIFICACIÓN ==========

    public abstract string AttackName { get; }
    public abstract float Damage { get; }

    // ========== EJECUCIÓN ==========

    public abstract void Execute();
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