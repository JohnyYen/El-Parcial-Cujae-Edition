using UnityEngine;

public class Player : MonoBehaviour, IPlayerProperties
{
    [SerializeField] public PlayerSO player_behaviour;
    // ========== PROPIEDADES ==========
    [SerializeField] private bool isAlive = true;
    [SerializeField] private float stress = 0f;
    [SerializeField] private float enfoque = 0f;
    [SerializeField] private bool canDash = true;
    [SerializeField] private bool canJump = true;
    [SerializeField] private PlayerState currentState = PlayerState.Idle;

    public bool IsAlive => isAlive;
    public float Stress => stress;
    public float Enfoque => enfoque;
    public bool CanDash => canDash;
    public bool CanJump => canJump;
    public PlayerState CurrentState => currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
