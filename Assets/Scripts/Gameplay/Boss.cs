using UnityEngine;
using System.Collections;

/// <summary>
/// Controlador MonoBehaviour del Boss "El Parcial".
/// Gestiona la instancia del comportamiento del boss y coordina ataques y eventos.
/// Análogo a Player.cs que controla PlayerSO.
/// </summary>
public class Boss : MonoBehaviour
{
    [Header("Boss Behavior")]
    [SerializeField] private ElParcialBoss bossBehaviour;

    [Header("Victory Screen")]
    [SerializeField] private VictoryScreen victoryScreen;

    [Header("Attack Timing")]
    [SerializeField] private float attackInterval = 3f;

    [Header("Attack Configuration")]
    [SerializeField] private float meleeDetectionRange = 4f;

    [Header("Minion Spawning")]
    [SerializeField] private float minionSpawnInterval = 5f;
    [SerializeField] private GameObject basicMinionPrefab;
    [SerializeField] private GameObject mediumMinionPrefab;
    [SerializeField] private Transform spawnPoints;

    private float lastAttackTime;
    private float lastSpawnTime;
    private Coroutine attackCoroutine;

    // ========== PROPIEDADES ==========

    public ElParcialBoss BossBehaviour => bossBehaviour;
    public bool IsAlive => bossBehaviour != null && bossBehaviour.IsAlive;

    private Animator animator;

    // ========== INICIALIZACIÓN ==========

    void Start()
    {
        if (bossBehaviour == null)
        {
            Debug.LogError("BossBehaviour no está asignado en el Inspector!");
            return;
        }

        SetupEventSubscriptions();
        lastAttackTime = Time.time;
        lastSpawnTime = Time.time;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!IsAlive)
            return;

        // Lógica de ataques automáticos basada en cooldown
        if (Time.time - lastAttackTime >= attackInterval)
        {
            ExecutePhaseAttack();
            lastAttackTime = Time.time;
        }

        // Lógica de spawning de minions
        if (Time.time - lastSpawnTime >= minionSpawnInterval)
        {
            SpawnMinion();
            lastSpawnTime = Time.time;
        }

        animator.SetInteger("Phase", bossBehaviour.CurrentPhase);
    }

    // ========== MÉTODOS PÚBLICOS ==========

    public void TakeDamage(float amount)
    {
        if (bossBehaviour != null && IsAlive)
        {
            bossBehaviour.TakeDamage(amount);
        }
    }

    // ========== MÉTODOS PRIVADOS ==========

    private void SetupEventSubscriptions()
    {
        if (bossBehaviour == null) return;

        bossBehaviour.OnHealthChanged += OnBossHealthChanged;
        bossBehaviour.OnPhaseChanged += OnBossPhaseChanged;
        bossBehaviour.OnBossDeath += OnBossDeath;
        bossBehaviour.OnAttack += OnBossAttack;
        bossBehaviour.OnMinionSpawned += OnMinionSpawned;
    }

    private void ExecutePhaseAttack()
    {
        if (!IsAlive) return;

        // Buscar al jugador
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj == null)
        {
            Debug.LogWarning("No se encontró al jugador!");
            return;
        }

        // Calcular distancia al jugador
        float distanceToPlayer = Vector2.Distance(transform.position, playerObj.transform.position);
        AttackType attackType;

        // Si el jugador está cerca, intentar ataque Melee
        if (distanceToPlayer <= meleeDetectionRange)
        {
            attackType = AttackType.Melee;
            Debug.Log($"Jugador cerca ({distanceToPlayer:F2}m) - Boss intenta Melee en fase {bossBehaviour.CurrentPhase}");
        }
        else
        {
            // Si está lejos, seleccionar ataque aleatorio (excluyendo Melee)
            attackType = GetRandomRangedAttackForPhase();
            Debug.Log($"Jugador lejos ({distanceToPlayer:F2}m) - Boss ejecuta ataque a distancia en fase {bossBehaviour.CurrentPhase}");
        }

        animator.SetTrigger("Attack");
        bossBehaviour.PerformAttack(attackType, transform.position);
    }

    private AttackType GetRandomAttackForPhase()
    {
        return bossBehaviour.CurrentPhase switch
        {
            1 => (AttackType)Random.Range(0, 2),  // Melee o Projectile
            2 => (AttackType)Random.Range(0, 3),  // Cualquiera
            3 => (AttackType)Random.Range(0, 3),  // Todos con igual probabilidad
            _ => AttackType.Projectile
        };
    }

    /// <summary>
    /// Obtiene un ataque aleatorio a distancia (excluyendo Melee) según la fase actual.
    /// </summary>
    private AttackType GetRandomRangedAttackForPhase()
    {
        return bossBehaviour.CurrentPhase switch
        {
            1 => AttackType.Projectile,  // Solo Projectile disponible
            2 => Random.value < 0.5f ? AttackType.Projectile : AttackType.Area,  // Projectile o Area
            3 => Random.value < 0.5f ? AttackType.Projectile : AttackType.Area,  // Projectile o Area
            _ => AttackType.Projectile
        };
    }

    private void SpawnMinion()
    {
        if (!IsAlive || spawnPoints == null)
            return;

        // Seleccionar tipo y prefab de minion según fase
        MinionType minionType;
        GameObject prefabToSpawn;

        switch (bossBehaviour.CurrentPhase)
        {
            case 1:
                minionType = MinionType.Basic;
                prefabToSpawn = basicMinionPrefab;
                break;
                
            case 2:
                // Fase 2: 50% Basic, 50% Medium
                if (Random.value < 0.5f)
                {
                    minionType = MinionType.Basic;
                    prefabToSpawn = basicMinionPrefab;
                }
                else
                {
                    minionType = MinionType.Medium;
                    prefabToSpawn = mediumMinionPrefab;
                }
                break;
                
            case 3:
                // Fase 3: Solo Medium (más difíciles)
                minionType = MinionType.Medium;
                prefabToSpawn = mediumMinionPrefab;
                break;
                
            default:
                minionType = MinionType.Basic;
                prefabToSpawn = basicMinionPrefab;
                break;
        }

        // Verificar que el prefab esté asignado
        if (prefabToSpawn == null)
        {
            Debug.LogWarning($"Boss: Prefab para {minionType} no está asignado!");
            return;
        }

        // Notificar al BossSO
        bossBehaviour.SpawnMinion(minionType);

        // Instanciar prefab en un spawn point aleatorio
        int spawnIndex = Random.Range(0, spawnPoints.childCount);
        Vector3 spawnPos = spawnPoints.GetChild(spawnIndex).position;
        Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }

    // ========== CALLBACKS DE EVENTOS ==========

    private void OnBossHealthChanged(float newHealth)
    {
        float healthPercent = (newHealth / bossBehaviour.MaxHealth) * 100f;
        Debug.Log($"Boss HP: {newHealth:F1}/{bossBehaviour.MaxHealth:F1} ({healthPercent:F0}%)");
    }

    private void OnBossPhaseChanged(int newPhase)
    {
        Debug.Log($"¡Boss entra en Fase {newPhase}!");
        // Aquí iría cambio de animaciones, efectos visuales, sonidos, etc.
    }

    private void OnBossDeath()
    {
        Debug.Log("¡El Parcial ha sido derrotado!");

        // Mostrar pantalla de victoria
        if (victoryScreen != null)
        {
            victoryScreen.Show();
        }
        else
        {
            Debug.LogWarning("VictoryScreen no está asignado en el Inspector!");
        }
    }

    private void OnBossAttack(AttackType attackType)
    {
        Debug.Log($"Boss ejecuta ataque tipo: {attackType}");
    }

    private void OnMinionSpawned(MinionType minionType)
    {
        Debug.Log($"Boss invoca minion: {minionType}");
    }

    private void OnDisable()
    {
        if (bossBehaviour != null)
        {
            bossBehaviour.OnHealthChanged -= OnBossHealthChanged;
            bossBehaviour.OnPhaseChanged -= OnBossPhaseChanged;
            bossBehaviour.OnBossDeath -= OnBossDeath;
            bossBehaviour.OnAttack -= OnBossAttack;
            bossBehaviour.OnMinionSpawned -= OnMinionSpawned;
        }
    }
}
