using UnityEngine;

/// <summary>
/// Componente para enemigos (minions) que dañan al jugador al contacto físico.
/// Se puede adjuntar a cualquier enemigo que deba hacer daño por colisión.
/// </summary>
public class EnemyHit : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private float contactDamage = 10f;
    [SerializeField] private float damageCooldown = 1f;
    [SerializeField] private bool destroyOnHit = false;

    private float lastDamageTime = -999f;

    void OnCollisionEnter2D(Collision2D collision)
    {
        TryApplyDamage(collision.collider);
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        TryApplyDamage(collision.collider);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        TryApplyDamage(collision);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        TryApplyDamage(collision);
    }

    private void TryApplyDamage(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        if (Time.time - lastDamageTime < damageCooldown)
            return;

        Player player = collision.GetComponent<Player>();
        if (player != null && player.player_behaviour != null)
        {
            player.player_behaviour.AddStress(contactDamage);
            lastDamageTime = Time.time;
            Debug.Log($"{gameObject.name} golpea al jugador por {contactDamage} de estrés");

            if (destroyOnHit)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetDamage(float newDamage)
    {
        contactDamage = newDamage;
    }
}
