using UnityEngine;

/// <summary>
/// Componente para el hitbox de la embestida del boss.
/// Se mueve en una dirección y daña al jugador al contacto.
/// </summary>
public class ChargeHitbox : MonoBehaviour
{
    private Vector2 direction;
    private float speed;
    private float duration;
    private float damage;
    private float startTime;

    private Rigidbody2D rb;

    /// <summary>
    /// Inicializa el hitbox de carga con los parámetros dados.
    /// </summary>
    public void Initialize(Vector2 chargeDirection, float chargeSpeed, float chargeDuration, float chargeDamage)
    {
        direction = chargeDirection.normalized;
        speed = chargeSpeed;
        duration = chargeDuration;
        damage = chargeDamage;
        startTime = Time.time;

        // Configurar Rigidbody
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0;
        rb.linearVelocity = direction * speed;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Verificar si la duración terminó
        if (Time.time - startTime >= duration)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            if (player != null && player.player_behaviour != null)
            {
                player.player_behaviour.AddStress(damage);
                Debug.Log($"ChargeHitbox golpea al jugador por {damage} de estrés");
            }
        }
    }
}
