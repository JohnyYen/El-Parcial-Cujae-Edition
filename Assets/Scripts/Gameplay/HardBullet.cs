using UnityEngine;

public class HardBullet : MonoBehaviour, IBullet
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 8f;
    [SerializeField] private float damage = 32f;
    [SerializeField] public HardShotSO bullet; // Efecto del ataque fuerte

    public float Damage => damage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;
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
