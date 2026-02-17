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

    [Header("Attack Timing")]
    [SerializeField] private float attackInterval = 3f;

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

        // Seleccionar tipo de ataque según la fase
        Debug.Log($"Boss ejecuta ataque en fase {bossBehaviour.CurrentPhase}");
        AttackType attackType = GetRandomAttackForPhase();
        bossBehaviour.PerformAttack(attackType);
        animator.SetTrigger("Attack");
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
        // Aquí iría lógica de victoria, drop de items, etc.
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
