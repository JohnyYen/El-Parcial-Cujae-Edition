using System.Collections;
using UnityEngine;
using System;

public class BuffController : MonoBehaviour
{
    // ========== VARIABLES PRIVADAS ==========
    
    [SerializeField] private float speedMultiplier = 1f;
    [SerializeField] private float fireRateMultiplier = 1f;
    [SerializeField] private bool isInvencible = false;
    
    private Coroutine coffeeCoroutine;
    private Coroutine notebookCoroutine;

    // ========== PROPIEDADES PÚBLICAS ==========
    
    public float SpeedMultiplier => speedMultiplier;
    public float FireRateMultiplier => fireRateMultiplier;
    public bool IsInvincible => isInvencible;

    // ========== EVENTOS ==========
    
    /// <summary>
    /// Se dispara cuando el multiplicador de velocidad cambia.
    /// Parámetro: nuevo valor del multiplicador
    /// </summary>
    public event Action<float> OnSpeedMultiplierChanged;

    /// <summary>
    /// Se dispara cuando el multiplicador de velocidad de fuego cambia.
    /// Parámetro: nuevo valor del multiplicador
    /// </summary>
    public event Action<float> OnFireRateMultiplierChanged;

    /// <summary>
    /// Se dispara cuando el estado de invencibilidad cambia.
    /// Parámetro: true si está invencible, false si no
    /// </summary>
    public event Action<bool> OnInvincibilityStatusChanged;

    // ========== MÉTODOS PÚBLICOS ==========

    public void ApplyCoffeeBuff(CoffeBuffSO buff)
    {
        if (coffeeCoroutine != null)
            StopCoroutine(coffeeCoroutine);

        coffeeCoroutine = StartCoroutine(CoffeeRoutine(buff));
    }

    public void ApplyNotebookBuff()
    {
        if (notebookCoroutine != null)
            StopCoroutine(notebookCoroutine);

        notebookCoroutine = StartCoroutine(NotebookRoutine());
    }

    // ========== CORRUTINAS PRIVADAS ==========

    private IEnumerator CoffeeRoutine(CoffeBuffSO buff)
    {
        // Aplicar nuevos multiplicadores
        speedMultiplier = buff.speedMultiplier;
        fireRateMultiplier = buff.fireRateMultiplier;

        // Disparar eventos para notificar al Player
        OnSpeedMultiplierChanged?.Invoke(speedMultiplier);
        OnFireRateMultiplierChanged?.Invoke(fireRateMultiplier);
        
        Debug.Log($"Coffee buff applied! SpeedMultiplier: {speedMultiplier}, FireRateMultiplier: {fireRateMultiplier}");
        yield return new WaitForSeconds(buff.duration);


        Debug.Log("Coffee buff expired. Reverting to default values.");
        // Revertir multiplicadores a 1f (valores por defecto)
        speedMultiplier = 1f;
        fireRateMultiplier = 1f;

        // Disparar eventos para notificar la finalización del buff
        OnSpeedMultiplierChanged?.Invoke(speedMultiplier);
        OnFireRateMultiplierChanged?.Invoke(fireRateMultiplier);
    }

    private IEnumerator NotebookRoutine()
    {
        // Aplicar invencibilidad
        isInvencible = true;
        OnInvincibilityStatusChanged?.Invoke(true);

        yield return new WaitForSeconds(5f);

        // Revertir invencibilidad
        isInvencible = false;
        OnInvincibilityStatusChanged?.Invoke(false);
    }
}
