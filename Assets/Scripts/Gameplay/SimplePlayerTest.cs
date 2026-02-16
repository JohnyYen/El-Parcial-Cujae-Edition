using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerTest : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        Vector2 input = Vector2.zero;

        if (kb.aKey.isPressed || kb.leftArrowKey.isPressed) input.x = -1f;
        if (kb.dKey.isPressed || kb.rightArrowKey.isPressed) input.x = 1f;
        if (kb.wKey.isPressed || kb.upArrowKey.isPressed) input.y = 1f;
        if (kb.sKey.isPressed || kb.downArrowKey.isPressed) input.y = -1f;

        input = input.normalized;
        transform.Translate(input * moveSpeed * Time.deltaTime);

        bool isMoving = input.magnitude > 0.1f;
        if (animator != null) animator.SetBool("isWalking", isMoving);

        // Flip sprite horizontally when moving left
        if (input.x < 0) transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        else if (input.x > 0) transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
}
