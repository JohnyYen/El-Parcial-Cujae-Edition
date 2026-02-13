using UnityEngine;

public class SoftBullet : MonoBehaviour
{

    [SerializeField] private float speed = 5f;
    [SerializeField] private float lifetime = 8f;
    [SerializeField] public SoftShotSO bullet;

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
        Destroy(gameObject);
    }
}
