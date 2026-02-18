using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Player Behavior")]
    [SerializeField] public PlayerSO player_behaviour;

    [Header("Buffs")]
    [SerializeField] private BuffController buffController;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [Header("Bullet Prefabs")]
    [SerializeField] private GameObject softBulletPrefab;
    [SerializeField] private GameObject hardBulletPrefab;

    private bool isGrounded = true;
    private Rigidbody2D rb;
    private Animator animator;

    // ========== PROPIEDADES ==========

    public BuffController BuffController => buffController;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        if (player_behaviour != null && buffController != null)
        {
            player_behaviour.SetBuffController(buffController);
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer.value);

        // Debug.Log($" Invencible: {player_behaviour.IsInvincible} | Speed Multiplier: {player_behaviour.DashSpeed} | Fire Rate Multiplier: {player_behaviour.SoftAttackCooldown}");
    }

    void FixedUpdate()
    {
        if (player_behaviour != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");

            if (horizontalInput > 0)
                transform.localScale = new Vector3(1, 1, 1); // Mirar a la derecha
            else if (horizontalInput < 0)
                transform.localScale = new Vector3(-1, 1, 1); // Mirar a la izquierda


            player_behaviour.Move(transform, horizontalInput);
            animator.SetFloat("Speed", Mathf.Abs(horizontalInput));

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

        // Suscribirse a eventos del BuffController
        if (buffController != null)
        {
            buffController.OnSpeedMultiplierChanged += OnSpeedMultiplierChanged;
            buffController.OnFireRateMultiplierChanged += OnFireRateMultiplierChanged;
            buffController.OnInvincibilityStatusChanged += OnInvincibilityStatusChanged;
        }
    }

    void OnDisable()
    {
        player_behaviour.OnJumpUsed -= Jump;
        player_behaviour.OnDashUsed -= Dash;
        player_behaviour.OnSoftAttackUsed -= SoftAttack;
        player_behaviour.OnHardAttackUsed -= HardAttack;

        // Desuscribirse de eventos del BuffController
        if (buffController != null)
        {
            buffController.OnSpeedMultiplierChanged -= OnSpeedMultiplierChanged;
            buffController.OnFireRateMultiplierChanged -= OnFireRateMultiplierChanged;
            buffController.OnInvincibilityStatusChanged -= OnInvincibilityStatusChanged;
        }
    }

    public void Jump()
    {
        if (player_behaviour != null && isGrounded)
        {
            animator.SetTrigger("Jump");
            rb.AddForce(Vector2.up * player_behaviour.JumpForce, ForceMode2D.Impulse);
        }
    }

    public void Dash()
    {
        if (player_behaviour != null)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float speedMultiplier = buffController != null ? buffController.SpeedMultiplier : 1f;

            if (horizontalInput > 0) // Dashing right
                rb.AddForce(Vector2.right * player_behaviour.DashSpeed * speedMultiplier, ForceMode2D.Impulse);
            else if (horizontalInput < 0) // Dashing left
                rb.AddForce(Vector2.left * player_behaviour.DashSpeed * speedMultiplier, ForceMode2D.Impulse);

            animator.SetTrigger("Dash");
        }
    }

    public void SoftAttack()
    {
        // Esqueleto del ataque suave
        Debug.Log("SoftAttack ejecutado!");

        Instantiate(softBulletPrefab, transform.position + transform.right * 0.5f, Quaternion.identity);
        animator.SetTrigger("SoftAttack");

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
        animator.SetTrigger("Special");

        // Aquí se agregará:
        // - Reproducir animación de ataque fuerte
        // - Crear hitbox más grande para detectar enemigos
        // - Aplicar daño a enemigos en la zona
        // - Aplicar knockback significativo
        // - Crear efecto visual de impacto
        // - Aplicar stun a enemigos (congelación breve)
    }

    // ========== MÉTODOS CALLBACK PARA BUFFS ==========

    private void OnSpeedMultiplierChanged(float newMultiplier)
    {
        // Este evento se dispara cuando el multiplicador de velocidad cambia
        // Los cambios se aplican automáticamente en:
        // - Move() de PlayerSO (ya integrado)
        // - Dash() de Player.cs (se aplica abajo)
        Debug.Log($"SpeedMultiplier changed to: {newMultiplier}");
    }

    private void OnFireRateMultiplierChanged(float newMultiplier)
    {
        // Este evento se dispara cuando el multiplicador de fuego cambia
        // Los cambios se aplican automáticamente en:
        // - SoftAttack() de PlayerSO (ya integrado)
        // - HardAttack() de PlayerSO (ya integrado)
        Debug.Log($"FireRateMultiplier changed to: {newMultiplier}");
    }

    private void OnInvincibilityStatusChanged(bool isInvincible)
    {
        // Actualizar el estado de invencibilidad en PlayerSO
        if (player_behaviour != null)
        {
            player_behaviour.SetInvincibility(isInvincible);
            Debug.Log($"Invincibility status changed to: {isInvincible}");
        }
    }
}
