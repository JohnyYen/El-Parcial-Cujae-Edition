using UnityEngine;
using System;

public abstract class PlayerSO : ScriptableObject, IPlayerBehaviour, IPlayerProperties
{
    // ========== PROPIEDADES ==========

    public abstract bool IsAlive { get; }
    public abstract float Stress { get; }
    public abstract float Enfoque { get; }
    public abstract bool CanDash { get; }
    public abstract bool CanJump { get; }
    public abstract bool IsInvincible { get; }
    public abstract PlayerState CurrentState { get; }

    public abstract float JumpForce { get; }
    public abstract float DashSpeed { get; }
    public abstract float SoftAttackCooldown { get; }
    public abstract float HardAttackCooldown { get; }
    public abstract float DashCooldown { get; }

    // ========== MÉTODOS ==========

    public abstract void Move(Transform transform,float direction);
    public abstract void Dash(Transform transform);
    public abstract void Jump(Transform transform);
    public abstract void AddStress(float amount);
    public abstract void AddEnfoque(float amount);
    public abstract bool ConsumeEnfoque(float amount);
    public abstract void ReduceStress(float amount);

    public abstract void SoftAttack();
    public abstract void HardAttack();
    public abstract void SetBuffController(BuffController buffController);
    public abstract void SetInvincibility(bool value);

    // ========== EVENTOS ==========

    public abstract event Action OnPlayerDeath;
    public abstract event Action OnPlayerHit;
    public abstract event Action OnDashUsed;
    public abstract event Action OnJumpUsed;
    public abstract event Action<float> OnStressChanged;
    public abstract event Action<float> OnEnfoqueChanged;
    public abstract event Action OnDashRefreshed;
    public abstract event Action<PlayerState> OnStateChanged;

    /// <summary>
    /// Se dispara cuando se ejecuta un ataque suave.
    /// </summary>
    public abstract event Action OnSoftAttackUsed;

    /// <summary>
    /// Se dispara cuando se ejecuta un ataque fuerte.
    /// </summary>
    public abstract event Action OnHardAttackUsed;

    /// <summary>
    /// Se dispara cuando un ataque conecta al enemigo.
    /// Parámetro: daño infringido
    /// </summary>
    public abstract event Action<float> OnAttackHit;

    /// <summary>
    /// Se dispara cuando el estado de invencibilidad cambia.
    /// Parámetro: true si está invencible, false si no
    /// </summary>
    public abstract event Action<bool> OnInvincibilityStatusChanged;
}
