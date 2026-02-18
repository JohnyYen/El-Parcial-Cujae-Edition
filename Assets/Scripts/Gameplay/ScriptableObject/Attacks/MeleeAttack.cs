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
    [SerializeField] private LayerMask playerLayer;

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;

    // ========== PROPIEDADES ==========

    public override string AttackName => attackName;
    public override AttackType Type => AttackType.Melee;
    public override float Damage => damage;
    public override float Cooldown => cooldown;
    public override float LastAttackTime => lastAttackTime;
    public override bool CanExecute => Time.time - lastAttackTime >= cooldown - 0.01f;
    public override bool IsInProgress => isInProgress;

    // ========== EVENTOS ==========

    public override event Action OnAttackStarted;
    public override event Action OnAttackEnded;
    public override event Action<float> OnAttackHitPlayer;

    // ========== MÉTODOS ==========

    public override void Execute()
    {
        // Sin posición del boss, no podemos detectar colisión
        Debug.LogWarning("MeleeAttack.Execute() llamado sin posición del boss. Usar Execute(Vector2).");
    }

    public override void Execute(Vector2 bossPosition)
    {
        if (!CanExecute)
            return;

        lastAttackTime = Time.time;
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName} desde posición {bossPosition}");

        // Detectar y dañar al jugador en rango
        CreateMeleeHitbox(bossPosition);

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

    private void CreateMeleeHitbox(Vector2 bossPosition)
    {
        // Dibujar círculo de debug para visualizar el rango
        DrawDebugCircle(bossPosition, meleeRange, Color.red, 1f);

        // Detectar colisiones con Physics2D.OverlapCircle
        Collider2D[] hits = Physics2D.OverlapCircleAll(bossPosition, meleeRange, playerLayer);

        if (hits.Length == 0)
        {
            Debug.Log($"MeleeAttack: No hay jugador en rango {meleeRange} desde {bossPosition}");
            return;
        }

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                Player player = hit.GetComponent<Player>();
                if (player != null && player.player_behaviour != null)
                {
                    player.player_behaviour.AddStress(damage);
                    OnAttackHitPlayer?.Invoke(damage);
                    Debug.Log($"MeleeAttack golpea al jugador por {damage} de estrés");
                }
            }
        }
    }

    /// <summary>
    /// Dibuja un círculo de debug en 2D.
    /// </summary>
    private void DrawDebugCircle(Vector2 center, float radius, Color color, float duration)
    {
        int segments = 36;
        float angleStep = 360f / segments;

        Vector3 prevPoint = center + new Vector2(Mathf.Cos(0), Mathf.Sin(0)) * radius;

        for (int i = 1; i <= segments; i++)
        {
            float angle = Mathf.Deg2Rad * (angleStep * i);
            Vector3 newPoint = center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
            Debug.DrawLine(prevPoint, newPoint, color, duration);
            prevPoint = newPoint;
        }
    }
}
