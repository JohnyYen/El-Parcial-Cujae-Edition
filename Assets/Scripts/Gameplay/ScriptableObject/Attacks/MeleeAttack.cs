using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque cuerpo a cuerpo (Melee).
/// El boss intenta atacar directamente al jugador en rango cercano.
/// </summary>
[CreateAssetMenu(fileName = "MeleeAttack", menuName = "Boss Attacks/Melee")]
public class MeleeAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Golpe Directo";
    [SerializeField] private float damage = 30f;
    [SerializeField] private float cooldown = 1.5f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase1, BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Melee Settings")]
    [SerializeField] private float meleeRange = 3f;
    [SerializeField] private float meleeDuration = 0.5f;

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
    public override float Damage => damage;
    public override float Cooldown => cooldown;
    public override float LastAttackTime => lastAttackTime;
    public override bool CanExecute => Time.time - lastAttackTime >= cooldown;
    public override bool IsInProgress => isInProgress;

    // ========== EVENTOS ==========

    public override event Action OnAttackStarted;
    public override event Action OnAttackEnded;
    public override event Action<float> OnAttackHitPlayer;

    // ========== MÉTODOS ==========

    public override void Execute()
    {
        if (!CanExecute)
            return;

        lastAttackTime = Time.time;
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName}");
        
        // Crear hitbox melee y verificar colisión
        CreateMeleeHitbox();

        // Notificar que golpea al jugador
        OnAttackHitPlayer?.Invoke(damage);

        isInProgress = false;
        OnAttackEnded?.Invoke();
    }

    public override bool IsValidForPhase(BossPhase phase)
    {
        foreach (var validPhase in validPhases)
        {
            if (phase == validPhase)
                return true;
        }
        return false;
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void CreateMeleeHitbox()
    {
        // La lógica de hitbox dependerá del sistema de colisiones del juego
        // Por ahora solo registramos el ataque
        
        Debug.Log($"Hitbox Melee activado - Range: {meleeRange}, Duration: {meleeDuration}s");

        // Aquí se podría:
        // 1. Crear un collider temporal
        // 2. Detectar colisiones con Physics2D.OverlapCircle
        // 3. Aplicar daño a entidades en rango
    }
}
