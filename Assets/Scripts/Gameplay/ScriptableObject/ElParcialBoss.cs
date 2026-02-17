using UnityEngine;
using System;
using System.Linq;

/// <summary>
/// Implementación concreta del Boss "El Parcial".
/// Hereda de BossSO y define los valores específicos del jefe.
/// </summary>
[CreateAssetMenu(fileName = "ElParcialBoss", menuName = "Boss/El Parcial")]
public class ElParcialBoss : BossSO
{
    // ========== CONFIGURACIÓN ==========

    [Header("Health")]
    [SerializeField] private float maxHealth = 1000f;

    [Header("Phase Thresholds")]
    [SerializeField] private float phase2Threshold = 800f;  // 80%
    [SerializeField] private float phase3Threshold = 400f;  // 40%

    [Header("Phase 1 Attacks")]
    [SerializeField] private BossAttackSO[] phase1Attacks;

    [Header("Phase 2 Attacks")]
    [SerializeField] private BossAttackSO[] phase2Attacks;

    [Header("Phase 3 Attacks")]
    [SerializeField] private BossAttackSO[] phase3Attacks;

    // ========== ESTADO PRIVADO ==========

    private float currentHealth;
    private int currentPhase = 1;

    // ========== EVENTOS ==========

    public override event Action<float> OnHealthChanged;
    public override event Action<int> OnPhaseChanged;
    public override event Action OnBossDeath;
    public override event Action<AttackType> OnAttack;
    public override event Action<MinionType> OnMinionSpawned;

    // ========== PROPIEDADES ==========

    public override int CurrentPhase => currentPhase;
    public override float MaxHealth => maxHealth;
    public override float CurrentHealth => currentHealth;
    public override bool IsAlive => currentHealth > 0;

    // ========== INICIALIZACIÓN ==========

    private void OnEnable()
    {
        ResetBoss();
    }

    public void ResetBoss()
    {
        currentHealth = maxHealth;
        currentPhase = 1;
    }

    // ========== MÉTODOS ==========

    public override void TakeDamage(float amount)
    {
        if (!IsAlive) return;

        currentHealth = Mathf.Max(currentHealth - amount, 0);
        OnHealthChanged?.Invoke(currentHealth);

        // Verificar cambio de fase
        CheckPhaseTransition();

        if (!IsAlive)
        {
            OnBossDeath?.Invoke();
        }
    }

    public override void ChangePhase(int phase)
    {
        if (currentPhase != phase && phase >= 1 && phase <= 3)
        {
            currentPhase = phase;
            OnPhaseChanged?.Invoke(phase);
        }
    }

    public override void PerformAttack(AttackType type)
    {
        var attacks = GetPhasedAttacks();
        var validAttacks = attacks.Where(a => a != null && a.IsValidForPhase((BossPhase)currentPhase)).ToArray();

        if (validAttacks.Length == 0)
        {
            Debug.LogWarning($"No valid attacks found for phase {currentPhase}");
            return;
        }

        var selectedAttack = validAttacks[UnityEngine.Random.Range(0, validAttacks.Length)];

        if (selectedAttack == null)
        {
            Debug.LogWarning($"Selected attack is null");
            return;
        }

        Debug.Log($"Selected attack: {selectedAttack.AttackName} | CanExecute: {selectedAttack.CanExecute} | LastAttackTime: {selectedAttack.LastAttackTime} | Cooldown: {selectedAttack.Cooldown}");
        
        if (selectedAttack.CanExecute)
        {
            Debug.Log($"Boss executes {selectedAttack.AttackName} of type {type} in phase {currentPhase}");
            selectedAttack.Execute();
            OnAttack?.Invoke(type);
        }
        else
        {
            Debug.LogWarning($"Attack {selectedAttack.AttackName} not ready. Cooldown remaining: {(selectedAttack.LastAttackTime + selectedAttack.Cooldown) - Time.time:F2}s");
        }
    }

    public override void SpawnMinion(MinionType type)
    {
        OnMinionSpawned?.Invoke(type);
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void CheckPhaseTransition()
    {
        if (currentHealth <= phase3Threshold && currentPhase != 3)
        {
            ChangePhase(3);
        }
        else if (currentHealth <= phase2Threshold && currentPhase != 2)
        {
            ChangePhase(2);
        }
    }

    private BossAttackSO[] GetPhasedAttacks()
    {
        return currentPhase switch
        {
            1 => phase1Attacks ?? new BossAttackSO[0],
            2 => phase2Attacks ?? new BossAttackSO[0],
            3 => phase3Attacks ?? new BossAttackSO[0],
            _ => phase1Attacks ?? new BossAttackSO[0]
        };
    }
}
