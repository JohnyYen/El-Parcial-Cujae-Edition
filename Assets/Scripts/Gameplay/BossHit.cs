using UnityEngine;

public class BossHit : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
        {
            return;
        }

        gameObject.GetComponent<Boss>().TakeDamage(1);
        Destroy(collision.gameObject);
    }
}
