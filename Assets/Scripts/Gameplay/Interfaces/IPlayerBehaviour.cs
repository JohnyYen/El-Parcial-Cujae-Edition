using UnityEngine;


public interface IPlayerBehaviour
{
    // ========== MÉTODOS ==========

    void Move(float direction);
    void Dash();
    void Jump();
    void AddStress(float amount);
    void AddEnfoque(float amount);
    bool ConsumeEnfoque(float amount);
    void ReduceStress(float amount);

    // ========== EVENTOS ==========

    event System.Action OnPlayerDeath;
    event System.Action OnPlayerHit;
    event System.Action OnDashUsed;
    event System.Action OnJumpUsed;
    event System.Action<float> OnStressChanged;
    event System.Action<float> OnEnfoqueChanged;
    event System.Action OnDashRefreshed;
    event System.Action<PlayerState> OnStateChanged;
}