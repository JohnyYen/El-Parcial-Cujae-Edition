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


        }
    }

    void OnEnable()
    {
        player_behaviour.OnJumpUsed += Jump;
        player_behaviour.OnDashUsed += Dash;
    }

    void OnDisable()
    {
        player_behaviour.OnJumpUsed -= Jump;
        player_behaviour.OnDashUsed -= Dash;
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
}
