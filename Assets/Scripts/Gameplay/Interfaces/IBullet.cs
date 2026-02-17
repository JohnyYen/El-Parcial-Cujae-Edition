using UnityEngine;

/// <summary>
/// Interfaz que define el contrato para cualquier proyectil en el juego.
/// Permite acceder al daño desde diferentes tipos de balas de forma uniforme.
/// </summary>
public interface IBullet
{
    /// <summary>
    /// El daño que inflige este proyectil al impactar.
    /// </summary>
    float Damage { get; }
}
