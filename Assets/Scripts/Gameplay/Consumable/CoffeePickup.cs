using UnityEngine;

public class CoffeePickup : MonoBehaviour
{
    [SerializeField] private CoffeBuffSO coffeeBuff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

        Player player = collision.GetComponent<Player>();
        // if (player != null && player == null || player.BuffController == null)
        //     Debug.Log("Player collided with CoffeePickup!");
        if (player != null && player.BuffController != null)
        {
            // Debug.Log("Player collided with CoffeePickup!");
            player.BuffController.ApplyCoffeeBuff(coffeeBuff);
            Destroy(gameObject);
        }
    }
}
