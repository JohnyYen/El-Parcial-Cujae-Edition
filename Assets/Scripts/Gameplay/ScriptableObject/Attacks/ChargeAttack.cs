using UnityEngine;
using System;

/// <summary>
/// Implementación de ataque de embestida (Charge).
/// El boss carga hacia la posición del jugador con velocidad aumentada.
/// Crea un hitbox de daño durante el movimiento.
/// </summary>
[CreateAssetMenu(fileName = "ChargeAttack", menuName = "Boss Attacks/Charge")]
public class ChargeAttack : BossAttackSO
{
    // ========== CONFIGURACIÓN ==========

    [SerializeField] private string attackName = "Embestida";
    [SerializeField] private float damage = 25f;
    [SerializeField] private float cooldown = 3f;
    [SerializeField] private BossPhase[] validPhases = { BossPhase.Fase2, BossPhase.Fase3 };

    [Header("Charge Settings")]
    [SerializeField] private GameObject chargeHitboxPrefab; // Prefab con collider de daño
    [SerializeField] private float chargeSpeed = 15f;
    [SerializeField] private float chargeDuration = 0.6f;
    [SerializeField] private float chargeDistance = 10f;

    // ========== ESTADO PRIVADO ==========

    private float lastAttackTime = -999f;
    private bool isInProgress;
    private Vector2 lastBossPosition;

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

    /// <summary>
    /// Se dispara cuando el boss debe moverse hacia una dirección.
    /// Parámetro: dirección normalizada de la carga.
    /// </summary>
    public event Action<Vector2> OnChargeDirection;

    // ========== MÉTODOS ==========

    void OnEnable()
    {
        lastAttackTime = -999f;
    }

    public override void Execute()
    {
        Execute(Vector2.zero);
    }

    public override void Execute(Vector2 bossPosition)
    {
        if (!CanExecute)
            return;

        lastAttackTime = Time.time;
        lastBossPosition = bossPosition;
        isInProgress = true;
        OnAttackStarted?.Invoke();

        Debug.Log($"Boss ejecuta: {attackName} desde {bossPosition}");

        // Obtener posición del jugador
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning($"No se encontró jugador para ChargeAttack!");
            isInProgress = false;
            OnAttackEnded?.Invoke();
            return;
        }

        // Calcular dirección hacia el jugador
        Vector2 direction = ((Vector2)player.transform.position - lastBossPosition).normalized;

        // Notificar dirección de carga (para que el Boss pueda animarse)
        OnChargeDirection?.Invoke(direction);

        // Crear hitbox de carga
        CreateChargeHitbox(lastBossPosition, direction);

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

    private void CreateChargeHitbox(Vector2 startPos, Vector2 direction)
    {
        // Si hay prefab de hitbox, usarlo
        if (chargeHitboxPrefab != null)
        {
            GameObject hitbox = Instantiate(chargeHitboxPrefab, startPos, Quaternion.identity);

            // Configurar movimiento del hitbox
            ChargeHitbox hitboxScript = hitbox.GetComponent<ChargeHitbox>();
            if (hitboxScript != null)
            {
                hitboxScript.Initialize(direction, chargeSpeed, chargeDuration, damage);
            }
            else
            {
                // Si no tiene script, configurar manualmente
                Rigidbody2D rb = hitbox.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * chargeSpeed;
                }
                Destroy(hitbox, chargeDuration);
            }
        }
        else
        {
            // Sin prefab: crear hitbox simple
            Debug.Log($"Charge hacia dirección {direction} - Speed: {chargeSpeed}, Duration: {chargeDuration}s");

            // Crear GameObject temporal con collider
            GameObject hitbox = new GameObject("ChargeHitbox");
            hitbox.transform.position = startPos;

            // Añadir collider
            CircleCollider2D collider = hitbox.AddComponent<CircleCollider2D>();
            collider.isTrigger = true;
            collider.radius = 1f;

            // Añadir Rigidbody para movimiento
            Rigidbody2D rb = hitbox.AddComponent<Rigidbody2D>();
            rb.gravityScale = 0;
            rb.linearVelocity = direction * chargeSpeed;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;

            // Añadir script de daño
            ChargeHitbox hitboxScript = hitbox.AddComponent<ChargeHitbox>();
            hitboxScript.Initialize(direction, chargeSpeed, chargeDuration, damage);

            // Destruir después de la duración
            Destroy(hitbox, chargeDuration);
        }

        Debug.Log($"ChargeHitbox creado en {startPos} - Dir: {direction}, Speed: {chargeSpeed}, Damage: {damage}");
    }
}
