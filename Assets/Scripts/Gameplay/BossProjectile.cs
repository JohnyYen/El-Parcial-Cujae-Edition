using UnityEngine;

/// <summary>
/// Script para proyectiles lanzados por el boss.
/// Implementa IBullet para que pueda aplicar daño al jugador.
/// </summary>
public class BossProjectile : MonoBehaviour, IBullet
{
    [Header("Projectile Settings")]
    [SerializeField] private float damage = 20f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private bool destroyOnImpact = true;

    private Rigidbody2D rb;
    private Vector2 velocity;
    private bool initialized = false;

    public float Damage => damage;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        
        // Configuración inicial del Rigidbody2D
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 0f;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Start()
    {
        // Destruir después de cierto tiempo si no ha impactado
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Mantener la velocidad constante si está inicializado
        if (initialized && rb != null)
        {
            rb.linearVelocity = velocity;
        }
    }

    /// <summary>
    /// Inicializa el proyectil con dirección y velocidad específicas.
    /// </summary>
    public void Initialize(Vector2 direction, float speed, float projectileDamage)
    {
        damage = projectileDamage;
        velocity = direction.normalized * speed;
        
        if (rb != null)
        {
            rb.linearVelocity = velocity;
        }
        
        initialized = true;
        
        Debug.Log($"BossProjectile inicializado - Dirección: {direction.normalized}, Magnitud velocidad: {velocity.magnitude}, Velocidad: {velocity}");
    }

    /// <summary>
    /// Establece el daño del proyectil dinámicamente desde el script de ataque.
    /// </summary>
    public void SetDamage(float newDamage)
    {
        damage = newDamage;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && player.player_behaviour != null)
            {
                player.player_behaviour.AddStress(damage);
                Debug.Log($"Proyectil del boss golpea al jugador por {damage} de estrés");
            }
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        else if (collision.CompareTag("Enemy"))
        {
            // Ignorar colisiones con el boss u otros enemigos
        }
    }
}
