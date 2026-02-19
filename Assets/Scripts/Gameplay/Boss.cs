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

    [Header("Phase Music with Crossfade")]
    [SerializeField] private AudioClip phase1Music;
    [SerializeField] private AudioClip phase2Music;
    [SerializeField] private AudioClip phase3Music;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.5f;
    [SerializeField][Range(0.5f, 5f)] private float crossfadeDuration = 2f;
    [SerializeField] private bool playMusicOnStart = true;

    private float lastAttackTime;
    private float lastSpawnTime;
    private Coroutine attackCoroutine;

    // ========== PROPIEDADES ==========

    public ElParcialBoss BossBehaviour => bossBehaviour;
    public bool IsAlive => bossBehaviour != null && bossBehaviour.IsAlive;

    private Animator animator;

    // ========== MÚSICA ==========
    private AudioSource musicSource;
    private int currentMusicPhase = 0;
    private Coroutine activeCrossfade;
    private bool isCrossfading = false;

    // ========== INICIALIZACIÓN ==========

    void Start()
    {
        if (bossBehaviour == null)
        {
            Debug.LogError("BossBehaviour no está asignado en el Inspector!");
            return;
        }

        SetupEventSubscriptions();
        SetupMusic();
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

        // Actualizar parámetro de fase en el Animator
        if (animator != null)
        {
            animator.SetInteger("Phase", bossBehaviour.CurrentPhase);
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

    // ========== MÉTODOS DE MÚSICA ==========

    private void SetupMusic()
    {
        // Crear AudioSource dedicado para música
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.volume = 0f; // Iniciar en silencio para fade in
        musicSource.loop = true;
        musicSource.playOnAwake = false;
        
        // Reproducir música inicial si está configurado
        if (playMusicOnStart && phase1Music != null)
        {
            StartCoroutine(InitialFadeIn());
        }
    }

    private IEnumerator InitialFadeIn()
    {
        musicSource.clip = phase1Music;
        musicSource.Play();
        currentMusicPhase = 1;
        
        // Fade in desde 0 hasta musicVolume
        float elapsed = 0f;
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t);
            yield return null;
        }
        
        musicSource.volume = musicVolume;
    }

    private void CrossfadeToPhase(int newPhase)
    {
        // Evitar cambios innecesarios o durante transición
        if (currentMusicPhase == newPhase || isCrossfading) return;
        
        // Detener crossfade anterior si existe
        if (activeCrossfade != null)
        {
            StopCoroutine(activeCrossfade);
        }
        
        // Iniciar nuevo crossfade
        AudioClip newClip = GetMusicForPhase(newPhase);
        if (newClip != null)
        {
            activeCrossfade = StartCoroutine(CrossfadeCoroutine(newClip, newPhase));
        }
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newClip, int targetPhase)
    {
        isCrossfading = true;
        float elapsed = 0f;
        float startVolume = musicSource.volume;
        
        // FASE 1: Fade out (reducir volumen actual) - 50% del tiempo
        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (crossfadeDuration * 0.5f);
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }
        
        musicSource.volume = 0f;
        
        // FASE 2: Cambiar clip
        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();
        currentMusicPhase = targetPhase;
        
        // FASE 3: Fade in (aumentar volumen nuevo) - 50% del tiempo
        elapsed = 0f;
        while (elapsed < crossfadeDuration * 0.5f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / (crossfadeDuration * 0.5f);
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t);
            yield return null;
        }
        
        musicSource.volume = musicVolume;
        isCrossfading = false;
        activeCrossfade = null;
        
        Debug.Log($"Crossfade completado a Fase {targetPhase}");
    }

    private AudioClip GetMusicForPhase(int phase)
    {
        return phase switch
        {
            1 => phase1Music,
            2 => phase2Music,
            3 => phase3Music,
            _ => null
        };
    }

    private IEnumerator FadeOutAndStop()
    {
        if (musicSource == null || !musicSource.isPlaying) yield break;
        
        float elapsed = 0f;
        float startVolume = musicSource.volume;
        
        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / crossfadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }
        
        musicSource.Stop();
        musicSource.volume = 0f;
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

        // Activar animación de ataque
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }
        
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
        
        // Actualizar parámetro de fase inmediatamente
        if (animator != null)
        {
            animator.SetInteger("Phase", newPhase);
        }
        
        // Cambiar música con crossfade
        CrossfadeToPhase(newPhase);
    }

    private void OnBossDeath()
    {
        Debug.Log("¡El Parcial ha sido derrotado!");
        
        // Detener música actual con fade out
        if (activeCrossfade != null)
        {
            StopCoroutine(activeCrossfade);
        }
        StartCoroutine(FadeOutAndStop());

        // Activar animación de muerte
        if (animator != null)
        {
            animator.SetTrigger("Complete");
        }

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
        // Detener coroutines de música
        if (activeCrossfade != null)
        {
            StopCoroutine(activeCrossfade);
        }
        
        // Limpiar eventos
        if (bossBehaviour != null)
        {
            bossBehaviour.OnHealthChanged -= OnBossHealthChanged;
            bossBehaviour.OnPhaseChanged -= OnBossPhaseChanged;
            bossBehaviour.OnBossDeath -= OnBossDeath;
            bossBehaviour.OnAttack -= OnBossAttack;
            bossBehaviour.OnMinionSpawned -= OnMinionSpawned;
        }
        
        // Detener música
        if (musicSource != null)
        {
            musicSource.Stop();
        }
    }
}
