using UnityEngine;

public class HardBullet : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifetime = 8f;
    [SerializeField] public HardShotSO bullet; // Efecto del ataque fuerte
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
        Destroy(gameObject);
    }
}
