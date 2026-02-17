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
    [SerializeField] private GameObject minionPrefab;
    [SerializeField] private Transform spawnPoints;

    private float lastAttackTime;
    private float lastSpawnTime;
    private Coroutine attackCoroutine;

    // ========== PROPIEDADES ==========

    public ElParcialBoss BossBehaviour => bossBehaviour;
    public bool IsAlive => bossBehaviour != null && bossBehaviour.IsAlive;

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
        AttackType attackType = GetRandomAttackForPhase();
        bossBehaviour.PerformAttack(attackType);
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
        if (!IsAlive || spawnPoints == null || minionPrefab == null)
            return;

        // Seleccionar tipo de minion según fase
        MinionType minionType = bossBehaviour.CurrentPhase switch
        {
            1 => MinionType.Basic,
            2 => MinionType.Fast,
            3 => MinionType.Tank,
            _ => MinionType.Basic
        };

        bossBehaviour.SpawnMinion(minionType);

        // Instanciar prefab en un spawn point aleatorio
        int spawnIndex = Random.Range(0, spawnPoints.childCount);
        Vector3 spawnPos = spawnPoints.GetChild(spawnIndex).position;
        Instantiate(minionPrefab, spawnPos, Quaternion.identity);
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
