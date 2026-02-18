using UnityEngine;

/// <summary>
/// Proyectil disparado por minions de tipo Ranged.
/// Se mueve en línea recta y aplica daño al jugador.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class MinionProjectile : MonoBehaviour
{
    // ========== CONFIGURACIÓN ==========

    [Header("Projectile Settings")]
    [SerializeField] private float damage = 15f;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float lifetime = 5f;

    [Header("Visual")]
    [SerializeField] private GameObject impactEffect;

    [Header("Layers")]
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask obstacleLayer;

    // ========== COMPONENTES ==========

    private Rigidbody2D rb;
    private Vector2 direction;
    private float spawnTime;

    // ========== PROPIEDADES ==========

    public float Damage
    {
        get => damage;
        set => damage = value;
    }

    public float Speed
    {
        get => speed;
        set => speed = value;
    }

    // ========== INICIALIZACIÓN ==========

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnTime = Time.time;
    }

    void Start()
    {
        // Auto-destruir después del lifetime
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        // Mover el proyectil
        if (direction != Vector2.zero)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    // ========== CONFIGURACIÓN ==========

    /// <summary>
    /// Establece la dirección del proyectil.
    /// </summary>
    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection.normalized;
        
        // Rotar el sprite hacia la dirección
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    /// <summary>
    /// Inicializa el proyectil con todos los parámetros.
    /// </summary>
    public void Initialize(Vector2 shootDirection, float projectileDamage, float projectileSpeed)
    {
        damage = projectileDamage;
        speed = projectileSpeed;
        SetDirection(shootDirection);
    }

    // ========== COLISIONES ==========

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verificar si golpeó al jugador
        if (IsInLayerMask(other.gameObject, playerLayer))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.player_behaviour != null)
            {
                // Aplicar daño
                player.player_behaviour.AddStress(damage);
                Debug.Log($"Minion projectile hit player for {damage} damage!");
            }

            // Destruir proyectil
            OnHit(other.transform.position);
            return;
        }

        // Verificar si golpeó un obstáculo
        if (IsInLayerMask(other.gameObject, obstacleLayer))
        {
            OnHit(other.transform.position);
            return;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si golpeó al jugador
        if (IsInLayerMask(collision.gameObject, playerLayer))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && player.player_behaviour != null)
            {
                // Aplicar daño
                player.player_behaviour.AddStress(damage);
                Debug.Log($"Minion projectile hit player for {damage} damage!");
            }

            // Destruir proyectil
            OnHit(collision.contacts[0].point);
            return;
        }

        // Cualquier otra colisión destruye el proyectil
        OnHit(collision.contacts[0].point);
    }

    // ========== EFECTOS ==========

    private void OnHit(Vector2 hitPosition)
    {
        // Instanciar efecto de impacto
        if (impactEffect != null)
        {
            Instantiate(impactEffect, hitPosition, Quaternion.identity);
        }

        // Destruir proyectil
        Destroy(gameObject);
    }

    // ========== UTILIDADES ==========

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) > 0;
    }

    // ========== GIZMOS ==========

    void OnDrawGizmos()
    {
        // Dibujar dirección del proyectil
        Gizmos.color = Color.red;
        if (direction != Vector2.zero)
        {
            Gizmos.DrawRay(transform.position, direction * 2f);
        }
    }
}
