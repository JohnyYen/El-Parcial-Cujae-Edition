using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{   
    [Header("Player Behavior")]
    [SerializeField] public PlayerSO player_behaviour;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject softBulletPrefab;
    [SerializeField] private GameObject hardBulletPrefab;

    private bool isGrounded = true;
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer.value);
    }

    void FixedUpdate()
    {
        if (player_behaviour != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            player_behaviour.Move(transform, horizontalInput);

            if (Input.GetAxis("Dash") > 0.0f)
            {   
                Debug.Log("Dash input detected!");
                player_behaviour.Dash(transform);
            }

            if (Input.GetAxis("Jump") > 0.1f)
            {
                player_behaviour.Jump(transform);
            }

            if (Input.GetAxis("SoftAttack") > 0.0f)
            {
                Debug.Log("SoftAttack input detected!");
                player_behaviour.SoftAttack();
            }

            if (Input.GetAxis("HardAttack") > 0.0f)
            {
                Debug.Log("HardAttack input detected!");
                player_behaviour.HardAttack();
            }
        }
    }

    void OnEnable()
    {
        player_behaviour.OnJumpUsed += Jump;
        player_behaviour.OnDashUsed += Dash;
        player_behaviour.OnSoftAttackUsed += SoftAttack;
        player_behaviour.OnHardAttackUsed += HardAttack;
    }

    void OnDisable()
    {
        player_behaviour.OnJumpUsed -= Jump;
        player_behaviour.OnDashUsed -= Dash;
        player_behaviour.OnSoftAttackUsed -= SoftAttack;
        player_behaviour.OnHardAttackUsed -= HardAttack;
    }

    public void Jump()
    {   
        if (player_behaviour != null && isGrounded)
        {
            rb.AddForce(Vector2.up * player_behaviour.JumpForce, ForceMode2D.Impulse);
        }
    }

    public void Dash()
    {
        if (player_behaviour != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0) // Dashing right
                rb.AddForce(Vector2.right * player_behaviour.DashSpeed, ForceMode2D.Impulse);
            else if (horizontalInput < 0) // Dashing left
                rb.AddForce(Vector2.left * player_behaviour.DashSpeed, ForceMode2D.Impulse);
        }
    }

    public void SoftAttack()
    {
        // Esqueleto del ataque suave
        Debug.Log("SoftAttack ejecutado!");
        
        Instantiate(softBulletPrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
        
        // Aquí se agregará:
        // - Reproducir animación de ataque suave
        // - Crear hitbox temporal para detectar enemigos
        // - Aplicar daño a enemigos en la zona
        // - Aplicar knockback menor
    }

    public void HardAttack()
    {
        // Esqueleto del ataque fuerte
        Debug.Log("HardAttack ejecutado!");
        
        Instantiate(hardBulletPrefab, transform.position + transform.right * 0.5f, Quaternion.identity);

        // Aquí se agregará:
        // - Reproducir animación de ataque fuerte
        // - Crear hitbox más grande para detectar enemigos
        // - Aplicar daño a enemigos en la zona
        // - Aplicar knockback significativo
        // - Crear efecto visual de impacto
        // - Aplicar stun a enemigos (congelación breve)
    }
}
