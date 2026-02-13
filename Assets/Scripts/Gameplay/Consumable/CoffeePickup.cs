using UnityEngine;

public class CoffeePickup : MonoBehaviour
{
    [SerializeField] private CoffeBuffSO coffeeBuff;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
            return;

           BuffController buffController = collision.GetComponent<BuffController>();
           if (buffController != null)
           {
               buffController.ApplyCoffeeBuff(coffeeBuff);
               Destroy(gameObject);
           }
    }
}
