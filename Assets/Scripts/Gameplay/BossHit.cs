using UnityEngine;

/// <summary>
/// Componente que detecta colisiones con proyectiles y aplica daño al boss.
/// Se espera que esté en el GameObject del boss con un collider configurado como trigger.
/// </summary>
public class BossHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"BossHit detecta colisión con: {collision.gameObject.name}");
        // Verificar que el objeto tiene el tag "Bullet"
        if (!collision.CompareTag("Bullet"))
            return;

        // Obtener el componente IBullet del projectil
        IBullet bulletComponent = collision.GetComponent<IBullet>();
        if (bulletComponent == null)
        {
            Debug.LogWarning($"Objeto con tag 'Bullet' no implementa IBullet: {collision.gameObject.name}");
            return;
        }

        // Obtener el componente Boss y aplicar daño
        Boss boss = gameObject.GetComponent<Boss>();
        if (boss != null)
        {
            boss.TakeDamage(bulletComponent.Damage);
            Debug.Log($"Boss recibe {bulletComponent.Damage} de daño. HP actual: {boss.BossBehaviour.CurrentHealth:F1}");
        }
        else
        {
            Debug.LogError("No se encontró componente Boss en el GameObject");
        }

        // Destruir el projectil después del impacto
        Destroy(collision.gameObject);
    }
}
