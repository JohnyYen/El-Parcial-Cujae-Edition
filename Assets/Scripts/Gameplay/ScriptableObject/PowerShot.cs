using System;
using UnityEngine;

/// <summary>
/// Implementación de ataque fuerte tipo "Disparo de Poder".
/// Un ataque potente que requiere carga y consume mucha energía.
/// </summary>
[CreateAssetMenu(fileName = "PowerShot", menuName = "Player Attacks/Hard Shot/Power")]
public class PowerShot : HardShotSO
{
    [Header("Power Shot Settings")]
    [SerializeField] private float chargeTime = 1f;
    [SerializeField] private float powerMultiplier = 1.5f;
    [SerializeField] private float explosionRadius = 3f;
    
    public override event Action OnHardShotExecuted;

    public override void HardShot()
    {
        Debug.Log($"Power Shot ejecutado - Tiempo de carga: {chargeTime}s, Multiplicador: {powerMultiplier}x, Radio: {explosionRadius}");
        OnHardShotExecuted?.Invoke();
    }

    // Propiedades públicas para acceder a los parámetros
    public float ChargeTime => chargeTime;
    public float PowerMultiplier => powerMultiplier;
    public float ExplosionRadius => explosionRadius;
}
