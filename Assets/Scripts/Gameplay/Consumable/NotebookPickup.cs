using UnityEngine;

public class NotebookPickup : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        BuffController buffController = collision.GetComponent<BuffController>();
        if (buffController != null)
        {
            buffController.ApplyNotebookBuff();
            Destroy(gameObject);
        }
    }
}
