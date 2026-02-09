using UnityEngine;
using System;

[CreateAssetMenu(fileName = "NewPlayer", menuName = "Player")]
public abstract class PlayerSO : ScriptableObject, IPlayer
{
    // ========== PROPIEDADES ==========

    public abstract bool IsAlive { get; }
    public abstract float Stress { get; }
    public abstract float Enfoque { get; }
    public abstract bool CanDash { get; }
    public abstract bool CanJump { get; }
    public abstract PlayerState CurrentState { get; }

    // ========== MÉTODOS ==========

    public abstract void Move(float direction);
    public abstract void Dash();
    public abstract void Jump();
    public abstract void AddStress(float amount);
    public abstract void AddEnfoque(float amount);
    public abstract bool ConsumeEnfoque(float amount);
    public abstract void ReduceStress(float amount);

    // ========== EVENTOS ==========

    public abstract event Action OnPlayerDeath;
    public abstract event Action OnPlayerHit;
    public abstract event Action OnDashUsed;
    public abstract event Action OnJumpUsed;
    public abstract event Action<float> OnStressChanged;
    public abstract event Action<float> OnEnfoqueChanged;
    public abstract event Action OnDashRefreshed;
    public abstract event Action<PlayerState> OnStateChanged;
}
