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
