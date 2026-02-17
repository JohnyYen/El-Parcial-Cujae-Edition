using UnityEngine;
using System;

/// <summary>
/// ScriptableObject abstracto base para cualquier minion del juego.
/// Implementa la interfaz IMinion y define el comportamiento genérico que todo minion debe cumplir.
/// Las implementaciones concretas definen stats específicos según el tipo de minion.
/// </summary>
public abstract class MinionSO : ScriptableObject, IMinion
{
    // ========== CONFIGURACIÓN ==========

    [Header("Stats Base")]
    [SerializeField] protected float maxHealth = 50f;
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float attackDamage = 10f;
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected float attackCooldown = 1f;

    [Header("Rewards")]
    [SerializeField] protected float enfoqueReward = 10f;

    [Header("Detection")]
    [SerializeField] protected float detectionRange = 8f;

    [Header("Patrol (Opcional)")]
    [SerializeField] protected bool usePatrol = false;
    [SerializeField] protected float patrolRadius = 5f;

    // ========== ESTADO PRIVADO ==========

    protected float currentHealth;
    protected bool isAlive = true;
    protected float lastAttackTime;
    protected Transform target;
    protected Vector2 patrolStartPosition;
    protected Vector2 patrolTargetPosition;
    protected bool isInitialized = false;

    // ========== PROPIEDADES ==========

    public abstract MinionType Type { get; }

    public virtual float Health => currentHealth;

    public virtual float MaxHealth => maxHealth;

    public virtual bool IsAlive => isAlive;

    public virtual float EnfoqueReward => enfoqueReward;

    // ========== EVENTOS ==========

    public virtual event Action<float> OnMinionHit;
    public virtual event Action OnMinionDeath;
    public virtual event Action OnMinionAttack;
    public virtual event Action OnMinionSpawned;

    // ========== MÉTODOS PÚBLICOS ==========

    /// <summary>
    /// Inicializa el minion con valores predeterminados.
    /// Debe llamarse cuando se spawea el minion.
    /// </summary>
    public virtual void Initialize()
    {
        currentHealth = maxHealth;
        isAlive = true;
        lastAttackTime = -attackCooldown;
        isInitialized = true;
        OnMinionSpawned?.Invoke();
    }

    /// <summary>
    /// Aplica daño al minion.
    /// </summary>
    public virtual void TakeDamage(float amount)
    {
        if (!isAlive) return;

        currentHealth -= amount;
        OnMinionHit?.Invoke(amount);

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die();
        }
    }

    /// <summary>
    /// Ejecuta un ataque contra el jugador si está en rango y el cooldown ha terminado.
    /// </summary>
    public virtual void Attack()
    {
        if (!isAlive) return;
        if (Time.time - lastAttackTime < attackCooldown) return;

        lastAttackTime = Time.time;
        OnMinionAttack?.Invoke();
        
        // El daño se aplicará desde el MonoBehaviour que use este SO
    }

    /// <summary>
    /// Persigue al jugador en dirección a su posición actual.
    /// </summary>
    public abstract void MoveTowardsPlayer();

    /// <summary>
    /// Patrol entre puntos definidos.
    /// </summary>
    public virtual void Patrol()
    {
        if (!isAlive) return;
        
        // La lógica de patrol específica se implementa en las clases concretas
        // o en el MonoBehaviour que use este SO
    }

    /// <summary>
    /// Ejecuta la muerte del minion.
    /// Otorga enfoque al jugador y marca al minion como muerto.
    /// </summary>
    public virtual void Die()
    {
        if (!isAlive) return;

        isAlive = false;
        OnMinionDeath?.Invoke();
    }

    // ========== MÉTODOS AUXILIARES ==========

    /// <summary>
    /// Verifica si el jugador está dentro del rango de ataque.
    /// </summary>
    protected virtual bool IsPlayerInAttackRange(Transform minionTransform, Transform playerTransform)
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(minionTransform.position, playerTransform.position);
        return distance <= attackRange;
    }

    /// <summary>
    /// Verifica si el jugador está dentro del rango de detección.
    /// </summary>
    protected virtual bool IsPlayerInDetectionRange(Transform minionTransform, Transform playerTransform)
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(minionTransform.position, playerTransform.position);
        return distance <= detectionRange;
    }

    /// <summary>
    /// Obtiene la dirección hacia el jugador.
    /// </summary>
    protected virtual Vector2 GetDirectionToPlayer(Transform minionTransform, Transform playerTransform)
    {
        if (playerTransform == null) return Vector2.zero;
        return (playerTransform.position - minionTransform.position).normalized;
    }

    /// <summary>
    /// Resetea el estado del minion para reutilización (Object Pooling).
    /// </summary>
    public virtual void ResetState()
    {
        currentHealth = maxHealth;
        isAlive = true;
        lastAttackTime = -attackCooldown;
        target = null;
    }

    // ========== PROPIEDADES AUXILIARES PARA MONOBEHAVIOUR ==========

    public float MoveSpeed => moveSpeed;
    public float AttackDamage => attackDamage;
    public float AttackRange => attackRange;
    public float AttackCooldown => attackCooldown;
    public float DetectionRange => detectionRange;
    public bool UsePatrol => usePatrol;
    public float PatrolRadius => patrolRadius;

    // ========== MÉTODOS PROTEGIDOS PARA INVOCAR EVENTOS ==========

    /// <summary>
    /// Invoca el evento OnMinionHit desde clases hijas.
    /// </summary>
    protected void InvokeOnMinionHit(float damage)
    {
        OnMinionHit?.Invoke(damage);
    }

    /// <summary>
    /// Invoca el evento OnMinionAttack desde clases hijas.
    /// </summary>
    protected void InvokeOnMinionAttack()
    {
        OnMinionAttack?.Invoke();
    }

    /// <summary>
    /// Invoca el evento OnMinionDeath desde clases hijas.
    /// </summary>
    protected void InvokeOnMinionDeath()
    {
        OnMinionDeath?.Invoke();
    }

    /// <summary>
    /// Invoca el evento OnMinionSpawned desde clases hijas.
    /// </summary>
    protected void InvokeOnMinionSpawned()
    {
        OnMinionSpawned?.Invoke();
    }
}
