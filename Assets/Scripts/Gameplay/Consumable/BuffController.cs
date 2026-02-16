using System.Collections;
using UnityEngine;

public class BuffController : MonoBehaviour
{

    [SerializeField] public float speedMultiplier = 1f;
    [SerializeField] float fireRateMultiplier = 1f;

    [SerializeField] bool isInvencible = false;
    Coroutine coffeeCoroutine;
    Coroutine notebookCoroutine;

    public float SpeedMultiplier => speedMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;

    public void ApplyCoffeeBuff(CoffeBuffSO buff)
    {
        if (coffeeCoroutine != null)
            StopCoroutine(coffeeCoroutine);

        coffeeCoroutine = StartCoroutine(CoffeeRoutine(buff));
    }

    IEnumerator CoffeeRoutine(CoffeBuffSO buff)
    {
        speedMultiplier = buff.speedMultiplier;
        fireRateMultiplier = buff.fireRateMultiplier;

        yield return new WaitForSeconds(buff.duration);

        speedMultiplier = 1f;
        fireRateMultiplier = 1f;
    }
    public void ApplyNotebookBuff()
    {
        if (notebookCoroutine != null)
            StopCoroutine(notebookCoroutine);

        notebookCoroutine = StartCoroutine(NotebookRoutine());
    }
    IEnumerator NotebookRoutine()
    {
        // Aquí se implementaría el efecto del cuaderno, por ejemplo:
        // - Aumentar el daño de los ataques
        // - Reducir el tiempo de recarga
        // - O cualquier otro efecto deseado
        isInvencible = true; // Ejemplo: el jugador se vuelve invencible
        yield return new WaitForSeconds(5f); // Duración del efecto del cuaderno

        // Aquí se revertirían los cambios realizados por el 
        isInvencible = false; // Revertir el estado de invencibilidad
    }
}
