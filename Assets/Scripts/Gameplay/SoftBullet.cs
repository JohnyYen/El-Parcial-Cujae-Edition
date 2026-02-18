using UnityEngine;

public class SoftBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 8f;
    [SerializeField] private float damage = 14f;
    [SerializeField] public SoftShotSO bullet;

    public float Damage => damage;

    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    void FixedUpdate()
    {
        rb.linearVelocityX = transform.right.x * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // No destruir si impacta un enemigo (el enemigo maneja la destrucción)
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
            return;

        Destroy(gameObject);
    }
}
