using UnityEngine;
using System.Collections;

/// <summary>
/// Componente MonoBehaviour que controla el comportamiento físico de un minion.
/// Usa un MinionSO para definir stats y lógica, similar al patrón Player/PlayerSO.
/// Implementa la máquina de estados del minion.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MinionBehaviour : MonoBehaviour
{
    // ========== CONFIGURACIÓN ==========

    [Header("Minion Data")]
    [SerializeField] private MinionSO minionData;

    [Header("Player Reference")]
    [SerializeField] private Transform playerTransform;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color hitColor = Color.red;
    [SerializeField] private float hitFlashDuration = 0.1f;

    [Header("Attack Feedback")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackRadius = 0.5f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Particles (Optional)")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private GameObject hitParticles;

    // ========== COMPONENTES ==========

    private Rigidbody2D rb;
    private Animator animator;
    private Color originalColor;

    // ========== ESTADO ==========

    private MinionState currentState = MinionState.Idle;
    private Vector2 patrolStartPosition;
    private Vector2 patrolTargetPosition;
    private float patrolWaitTime = 0f;
    private bool movingTowardsPatrolTarget = true;

    // ========== ENUMS ==========

    public enum MinionState
    {
        Idle,
        Patrol,
        Chase,
        Attack,
        Hit,
        Death
    }

    // ========== PROPIEDADES ==========

    public MinionSO MinionData => minionData;
    public MinionState CurrentState => currentState;
    public bool IsAlive => minionData != null && minionData.IsAlive;

    // ========== INICIALIZACIÓN ==========

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;

        // Buscar al jugador si no está asignado
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
                playerTransform = player.transform;
        }

        // Inicializar el minion
        if (minionData != null)
        {
            minionData.Initialize();
            
            // Suscribirse a eventos
            minionData.OnMinionHit += HandleHit;
            minionData.OnMinionDeath += HandleDeath;
            minionData.OnMinionAttack += HandleAttack;
            minionData.OnMinionSpawned += HandleSpawned;
        }

        // Configurar patrol
        patrolStartPosition = transform.position;
        GenerateNewPatrolTarget();

        // Estado inicial
        ChangeState(minionData.UsePatrol ? MinionState.Patrol : MinionState.Idle);
    }

    void OnDestroy()
    {
        // Desuscribirse de eventos
        if (minionData != null)
        {
            minionData.OnMinionHit -= HandleHit;
            minionData.OnMinionDeath -= HandleDeath;
            minionData.OnMinionAttack -= HandleAttack;
            minionData.OnMinionSpawned -= HandleSpawned;
        }
    }

    // ========== UPDATE ==========

    void Update()
    {
        if (minionData == null || !minionData.IsAlive) return;

        UpdateStateMachine();
        UpdateAnimations();
    }

    void FixedUpdate()
    {
        if (minionData == null || !minionData.IsAlive) return;

        ExecuteMovement();
    }

    // ========== MÁQUINA DE ESTADOS ==========

    private void UpdateStateMachine()
    {
        switch (currentState)
        {
            case MinionState.Idle:
                HandleIdleState();
                break;

            case MinionState.Patrol:
                HandlePatrolState();
                break;

            case MinionState.Chase:
                HandleChaseState();
                break;

            case MinionState.Attack:
                HandleAttackState();
                break;

            case MinionState.Hit:
                // Estado temporal, vuelve automáticamente a chase
                break;

            case MinionState.Death:
                // Estado final
                break;
        }
    }

    private void HandleIdleState()
    {
        // Detectar jugador
        if (IsPlayerInDetectionRange())
        {
            ChangeState(MinionState.Chase);
        }
    }

    private void HandlePatrolState()
    {
        // Detectar jugador (prioridad)
        if (IsPlayerInDetectionRange())
        {
            ChangeState(MinionState.Chase);
            return;
        }

        // Ejecutar patrol
        float distanceToTarget = Vector2.Distance(transform.position, patrolTargetPosition);
        
        if (distanceToTarget < 0.5f)
        {
            // Llegó al punto de patrol, espera y cambia dirección
            patrolWaitTime += Time.deltaTime;
            
            if (patrolWaitTime > 1f)
            {
                GenerateNewPatrolTarget();
                patrolWaitTime = 0f;
            }
        }
    }

    private void HandleChaseState()
    {
        // Si pierde al jugador de vista
        if (!IsPlayerInDetectionRange())
        {
            ChangeState(minionData.UsePatrol ? MinionState.Patrol : MinionState.Idle);
            return;
        }

        // Si está en rango de ataque
        if (IsPlayerInAttackRange())
        {
            ChangeState(MinionState.Attack);
            return;
        }

        // Continuar persecución (movimiento en FixedUpdate)
    }

    private void HandleAttackState()
    {
        // Si el jugador sale del rango
        if (!IsPlayerInAttackRange())
        {
            ChangeState(MinionState.Chase);
            return;
        }

        // Intentar atacar (respeta cooldown internamente)
        minionData.Attack();
    }

    private void ChangeState(MinionState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        OnStateChanged(newState);
    }

    private void OnStateChanged(MinionState newState)
    {
        // Lógica al cambiar de estado
        switch (newState)
        {
            case MinionState.Chase:
                // Visual: orientar hacia el jugador
                break;

            case MinionState.Attack:
                // Visual: preparar ataque
                break;

            case MinionState.Death:
                // Detener movimiento
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    // ========== MOVIMIENTO ==========

    private void ExecuteMovement()
    {
        switch (currentState)
        {
            case MinionState.Patrol:
                MoveTowardsTarget(patrolTargetPosition);
                break;

            case MinionState.Chase:
                MoveTowardsPlayer();
                break;

            case MinionState.Attack:
                // No se mueve mientras ataca
                rb.linearVelocity = Vector2.zero;
                break;

            default:
                rb.linearVelocity = Vector2.zero;
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        if (playerTransform == null) return;

        // Movimiento estándar hacia el jugador
        Vector2 direction = (playerTransform.position - transform.position).normalized;
        rb.linearVelocity = direction * minionData.MoveSpeed;
        
        // Flip sprite
        FlipSprite(direction.x);
    }



    private void MoveTowardsTarget(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * minionData.MoveSpeed;
        
        // Flip sprite
        FlipSprite(direction.x);
    }

    private void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return;
        
        if (directionX < 0)
            spriteRenderer.flipX = true;
        else if (directionX > 0)
            spriteRenderer.flipX = false;
    }

    // ========== DETECCIÓN ==========

    private bool IsPlayerInDetectionRange()
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= minionData.DetectionRange;
    }

    private bool IsPlayerInAttackRange()
    {
        if (playerTransform == null) return false;
        float distance = Vector2.Distance(transform.position, playerTransform.position);
        return distance <= minionData.AttackRange;
    }

    // ========== PATROL ==========

    private void GenerateNewPatrolTarget()
    {
        Vector2 randomOffset = Random.insideUnitCircle * minionData.PatrolRadius;
        patrolTargetPosition = patrolStartPosition + randomOffset;
    }

    // ========== EVENTOS DEL MINION ==========

    private void HandleSpawned()
    {
        // Feedback visual del spawn
        Debug.Log($"{minionData.Type} minion spawned!");
    }

    private void HandleHit(float damage)
    {
        // Cambiar temporalmente a estado Hit
        if (currentState != MinionState.Death)
        {
            StartCoroutine(HitFeedback());
            
            // Instantiate hit particles
            if (hitParticles != null)
            {
                Instantiate(hitParticles, transform.position, Quaternion.identity);
            }
        }
    }

    private void HandleDeath()
    {
        ChangeState(MinionState.Death);
        
        // Dar enfoque al jugador
        Player player = playerTransform?.GetComponent<Player>();
        if (player != null && player.player_behaviour != null)
        {
            player.player_behaviour.AddEnfoque(minionData.EnfoqueReward);
        }

        // Death particles
        if (deathParticles != null)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }

        // Destruir después de la animación
        Destroy(gameObject, 1f);
    }

    private void HandleAttack()
    {
        // Aplicar daño al jugador si está en rango
        if (attackPoint == null)
            attackPoint = transform;

        Debug.Log($"{minionData.Type} minion is attacking!");
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        
        foreach (Collider2D hit in hits)
        {
            Debug.Log($"{minionData.Type} minion hit something in attack range: {hit.name}");
            Player player = hit.GetComponent<Player>();
            if (player != null && player.player_behaviour != null)
            {
                player.player_behaviour.AddStress(minionData.AttackDamage);
                Debug.Log($"{minionData.Type} minion attacked player for {minionData.AttackDamage} damage!");
            }
        }
    }

    // ========== FEEDBACK VISUAL ==========

    private IEnumerator HitFeedback()
    {
        MinionState previousState = currentState;
        ChangeState(MinionState.Hit);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = hitColor;
            yield return new WaitForSeconds(hitFlashDuration);
            spriteRenderer.color = originalColor;
        }

        // Volver al estado de persecución si sigue vivo
        if (minionData.IsAlive)
        {
            ChangeState(MinionState.Chase);
        }
    }

    private void UpdateAnimations()
    {
        if (animator == null) return;

        // Actualizar parámetros del animator según el estado
        animator.SetBool("IsMoving", rb.linearVelocity.magnitude > 0.1f);
        animator.SetBool("IsAttacking", currentState == MinionState.Attack);
        animator.SetBool("IsDead", currentState == MinionState.Death);
    }

    // ========== DAÑO PÚBLICO ==========

    /// <summary>
    /// Método público para que balas/ataques externos apliquen daño.
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (minionData != null)
        {
            minionData.TakeDamage(amount);
        }
    }

    // ========== GIZMOS ==========

    private void OnDrawGizmosSelected()
    {
        if (minionData == null) return;

        // Detection range
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, minionData.DetectionRange);

        // Attack range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, minionData.AttackRange);

        // Attack point
        if (attackPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
        }

        // Patrol radius
        if (minionData.UsePatrol)
        {
            Gizmos.color = Color.cyan;
            Vector2 startPos = Application.isPlaying ? patrolStartPosition : (Vector2)transform.position;
            Gizmos.DrawWireSphere(startPos, minionData.PatrolRadius);
        }
    }
}
