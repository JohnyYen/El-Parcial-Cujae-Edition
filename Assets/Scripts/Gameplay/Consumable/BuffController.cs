using System.Collections;
using UnityEngine;

public class BuffController : MonoBehaviour
{

    float speedMultiplier = 1f;
    float fireRateMultiplier = 1f;

    Coroutine coffeeCoroutine;

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
}
