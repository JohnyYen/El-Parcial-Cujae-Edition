using UnityEngine;

public class NotebookPickup : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Debug.Log("Player collided with NotebookPickup!");
        Player player = collision.GetComponent<Player>();
        if (player != null && player.BuffController != null)
        {
            player.BuffController.ApplyNotebookBuff();
            Destroy(gameObject);
        }
    }
}
