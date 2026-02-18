using UnityEngine;

/// <summary>
/// Componente que detecta colisiones con proyectiles del jugador y aplica daño al minion.
/// Se espera que esté en el GameObject del minion con un collider configurado como trigger.
/// </summary>
public class MinionHit : MonoBehaviour
{
    [Header("Enfoque Reward")]
    [SerializeField] private float enfoqueReward = 5f;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        IBullet bullet = collision.GetComponent<IBullet>();
        if (bullet == null)
        {
            Debug.LogWarning($"Objeto con tag 'Bullet' no implementa IBullet: {collision.gameObject.name}");
            return;
        }

        MinionBehaviour minion = GetComponent<MinionBehaviour>();
        if (minion != null)
        {
            minion.TakeDamage(bullet.Damage);
            Debug.Log($"Minion {gameObject.name} recibe {bullet.Damage} de daño");
        }
        else
        {
            Debug.LogError("No se encontró componente MinionBehaviour en el GameObject");
        }

        Player player = FindFirstObjectByType<Player>();
        if (player != null && player.player_behaviour != null)
        {
            player.player_behaviour.AddEnfoque(enfoqueReward);
            Debug.Log($"Jugador recibe {enfoqueReward} de enfoque por acertar al minion");
        }

        Destroy(collision.gameObject);
    }
}
