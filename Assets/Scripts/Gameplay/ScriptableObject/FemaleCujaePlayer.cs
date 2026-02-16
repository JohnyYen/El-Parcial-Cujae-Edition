using UnityEngine;
using System;

/// <summary>
/// Implementación concreta del jugador FemaleCujae.
/// Variante más rápida y resistente del jugador base.
/// </summary>
[CreateAssetMenu(fileName = "FemaleCujaePlayer", menuName = "Player/Female Cujae")]
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

    [Header("Soft Attack")]
    [SerializeField] private float softAttackDamage = 14f;
    [SerializeField] private float softAttackCooldown = 0.25f;

    [Header("Hard Attack")]
    [SerializeField] private float hardAttackDamage = 32f;
    [SerializeField] private float hardAttackCooldown = 1.3f;
    [SerializeField] private float hardAttackEnfoqueCost = 18f;

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
    private bool isInvincible = false;
    private PlayerState currentState = PlayerState.Idle;
    private float lastDashTime;
    private float lastSoftAttackTime;
    private float lastHardAttackTime;
    private BuffController buffController;

    // ========== EVENTOS ==========

    public override event Action OnPlayerDeath;
    public override event Action OnPlayerHit;
    public override event Action OnDashUsed;
    public override event Action OnJumpUsed;
    public override event Action<float> OnStressChanged;
    public override event Action<float> OnEnfoqueChanged;
    public override event Action OnDashRefreshed;
    public override event Action<PlayerState> OnStateChanged;
    public override event Action OnSoftAttackUsed;
    public override event Action OnHardAttackUsed;
    public override event Action<float> OnAttackHit;
    public override event Action<bool> OnInvincibilityStatusChanged;

    // ========== PROPIEDADES ==========

    public override bool IsAlive => isAlive;

    public override float Stress => currentStress;

    public override float Enfoque => currentEnfoque;

    public override bool CanDash => dashAvailable;

    public override bool CanJump => jumpAvailable;

    public override bool IsInvincible => isInvincible;

    public override PlayerState CurrentState => currentState;

    public override float JumpForce => jumpForce;

    public override float DashSpeed => dashSpeed;

    // ========== MÉTODOS ==========

    public override void Move(Transform transform, float direction)
    {
        if (!isAlive) return;

        if (direction != 0)
        {
            ChangeState(PlayerState.Moving);
            float speedMultiplier = buffController != null ? buffController.SpeedMultiplier : 1f;
            transform.Translate(Vector2.right * direction * moveSpeed * speedMultiplier * Time.deltaTime);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }

    public override void Dash(Transform transform)
    {
        if (!isAlive || !dashAvailable) return;

        dashAvailable = false;
        lastDashTime = Time.time;
        ChangeState(PlayerState.Dashing);

        OnDashUsed?.Invoke();

        // Lógica de dash
        // Invoke(nameof(RefreshDash), dashCooldown);
    }

    public override void Jump(Transform transform)
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
        if (!isAlive || isInvincible) return;

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
        lastSoftAttackTime = -softAttackCooldown;
        lastHardAttackTime = -hardAttackCooldown;
    }

    public override void SoftAttack()
    {
        if (!isAlive) return;

        float fireRateMultiplier = buffController != null ? buffController.FireRateMultiplier : 1f;
        float adjustedCooldown = softAttackCooldown / fireRateMultiplier;

        // Verificar cooldown del ataque suave
        if (Time.time < lastSoftAttackTime + adjustedCooldown) return;

        // Actualizar timestamp del último ataque
        lastSoftAttackTime = Time.time;

        // Cambiar estado
        ChangeState(PlayerState.Attacking);

        // Disparar evento
        OnSoftAttackUsed?.Invoke();

        // Disparar evento de ataque conectado (para el sistema de daño)
        OnAttackHit?.Invoke(softAttackDamage);
    }

    public override void HardAttack()
    {
        if (!isAlive) return;

        // Verificar que hay suficiente enfoque
        if (!ConsumeEnfoque(hardAttackEnfoqueCost)) return;

        float fireRateMultiplier = buffController != null ? buffController.FireRateMultiplier : 1f;
        float adjustedCooldown = hardAttackCooldown / fireRateMultiplier;

        // Verificar cooldown del ataque fuerte
        if (Time.time < lastHardAttackTime + adjustedCooldown) return;

        // Actualizar timestamp del último ataque
        lastHardAttackTime = Time.time;

        // Cambiar estado
        ChangeState(PlayerState.Attacking);

        // Disparar evento
        OnHardAttackUsed?.Invoke();

        // Disparar evento de ataque conectado (para el sistema de daño)
        OnAttackHit?.Invoke(hardAttackDamage);
    }

    public override void SetBuffController(BuffController buffController)
    {
        this.buffController = buffController;
    }

    public override void SetInvincibility(bool value)
    {
        if (isInvincible != value)
        {
            isInvincible = value;
            OnInvincibilityStatusChanged?.Invoke(value);
        }
    }
}
