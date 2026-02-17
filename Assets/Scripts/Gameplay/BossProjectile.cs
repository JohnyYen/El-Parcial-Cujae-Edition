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

    public float Damage => damage;

    void Start()
    {
        // Destruir después de cierto tiempo si no ha impactado
        Destroy(gameObject, lifetime);
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
        // Si golpea al jugador
        if (collision.CompareTag("Player"))
        {
            Debug.Log($"Proyectil del boss golpea al jugador por {damage} daño");
            
            if (destroyOnImpact)
            {
                Destroy(gameObject);
            }
        }
        // Si golpea al boss (ricochete?, ignorar)
        else if (collision.CompareTag("Enemy"))
        {
            // Ignorar colisiones con el boss
        }
    }
}
