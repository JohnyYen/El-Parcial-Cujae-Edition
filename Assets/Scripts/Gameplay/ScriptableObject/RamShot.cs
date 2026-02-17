using System;
using UnityEngine;

/// <summary>
/// Implementación de ataque suave tipo "Embestida".
/// El jugador carga hacia adelante de forma rápida y potente.
/// </summary>
[CreateAssetMenu(fileName = "RamShot", menuName = "Player Attacks/Soft Shot/Ram")]
public class RamShot : SoftShotSO
{
    [Header("Ram Shot Settings")]
    [SerializeField] private float ramSpeed = 15f;
    [SerializeField] private float ramDuration = 0.5f;
    
    public override event Action OnSoftShotExecuted;

    public override void SoftShot()
    {
        Debug.Log($"Ram Shot ejecutado - Velocidad: {ramSpeed}, Duración: {ramDuration}s");
        OnSoftShotExecuted?.Invoke();
    }

    // Propiedades públicas para acceder a los parámetros
    public float RamSpeed => ramSpeed;
    public float RamDuration => ramDuration;
}
