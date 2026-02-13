using UnityEngine;
using System;

/// <summary>
/// Implementación concreta del jugador FemaleCujae.
/// Variante más rápida y resistente del jugador base.
/// </summary>
// [CreateAssetMenu(fileName = "FemaleCujaePlayer", menuName = "Player/Female Cujae")]
public class FemaleCujaePlayer : PlayerSO
{
    // ========== CONFIGURACIÓN ==========

    [Header("Stats")]
    [SerializeField] private float maxStress = 100f;
    [SerializeField] private float maxEnfoque = 100f;
    [SerializeField] private float moveSpeed = 6f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float jumpForce = 7f;

    [Header("Cooldowns")]
    [SerializeField] private float dashCooldown = 1.5f;

    [Header("Enfoque Rewards")]
    [SerializeField] private float enfoquePerKill = 15f;
    [SerializeField] private float enfoquePerDodge = 8f;

    [Header("Stress Damage")]
    [SerializeField] private float stressPerHit = 12f;

    // ========== ESTADO PRIVADO ==========

    private float currentStress;
    private float currentEnfoque;
    private bool dashAvailable = true;
    private bool jumpAvailable = true;
    private bool isAlive = true;
    private PlayerState currentState = PlayerState.Idle;
    private float lastDashTime;

    // ========== EVENTOS ==========

    public override event Action OnPlayerDeath;
    public override event Action OnPlayerHit;
    public override event Action OnDashUsed;
    public override event Action OnJumpUsed;
    public override event Action<float> OnStressChanged;
    public override event Action<float> OnEnfoqueChanged;
    public override event Action OnDashRefreshed;
    public override event Action<PlayerState> OnStateChanged;

    // ========== PROPIEDADES ==========

    public override bool IsAlive => isAlive;

    public override float Stress => currentStress;

    public override float Enfoque => currentEnfoque;

    public override bool CanDash => dashAvailable;

    public override bool CanJump => jumpAvailable;

    public override PlayerState CurrentState => currentState;

    // ========== MÉTODOS ==========

    public override void Move(float direction)
    {
        if (!isAlive) return;

        if (direction != 0)
        {
            ChangeState(PlayerState.Moving);
            // Lógica de movimiento
            // transform.Translate(Vector2.right * direction * moveSpeed * Time.deltaTime);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public override void Dash()
    {
        if (!isAlive || !dashAvailable) return;

        dashAvailable = false;
        lastDashTime = Time.time;
        ChangeState(PlayerState.Dashing);

        OnDashUsed?.Invoke();

        // Lógica de dash
        // Invoke(nameof(RefreshDash), dashCooldown);
    }

    public override void Jump()
    {
        if (!isAlive || !jumpAvailable) return;

        jumpAvailable = false;
        ChangeState(PlayerState.Jumping);

        OnJumpUsed?.Invoke();

        // Lógica de jump
        RefreshJump();
        // Invoke(nameof(RefreshJump), 0.5f);
    }

    public override void AddStress(float amount)
    {
        if (!isAlive) return;

        currentStress = Mathf.Min(currentStress + amount, maxStress);
        OnStressChanged?.Invoke(currentStress);

        OnPlayerHit?.Invoke();

        if (currentStress >= maxStress)
        {
            Die();
        }
    }

    public override void AddEnfoque(float amount)
    {
        if (!isAlive) return;

        currentEnfoque = Mathf.Min(currentEnfoque + amount, maxEnfoque);
        OnEnfoqueChanged?.Invoke(currentEnfoque);
    }

    public override bool ConsumeEnfoque(float amount)
    {
        if (currentEnfoque >= amount)
        {
            currentEnfoque -= amount;
            OnEnfoqueChanged?.Invoke(currentEnfoque);
            return true;
        }
        return false;
    }

    public override void ReduceStress(float amount)
    {
        if (!isAlive) return;

        currentStress = Mathf.Max(currentStress - amount, 0);
        OnStressChanged?.Invoke(currentStress);
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void RefreshDash()
    {
        dashAvailable = true;
        OnDashRefreshed?.Invoke();
    }

    private void RefreshJump()
    {
        jumpAvailable = true;
    }

    private void ChangeState(PlayerState newState)
    {
        if (currentState != newState)
        {
            currentState = newState;
            OnStateChanged?.Invoke(currentState);
        }
    }

    private void Die()
    {
        isAlive = false;
        ChangeState(PlayerState.Dead);
        OnPlayerDeath?.Invoke();
    }

    // ========== INICIALIZACIÓN ==========

    private void OnEnable()
    {
        ResetPlayer();
    }

    public void ResetPlayer()
    {
        currentStress = 0;
        currentEnfoque = 0;
        dashAvailable = true;
        jumpAvailable = true;
        isAlive = true;
        currentState = PlayerState.Idle;
        lastDashTime = 0;
    }
}
