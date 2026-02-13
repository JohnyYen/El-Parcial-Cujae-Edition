using UnityEngine;

[CreateAssetMenu(fileName = "New Coffee Buff", menuName = "Buffs/Coffee Buff")]
public class CoffeBuffSO : ScriptableObject
{
    public float duration = 5f;
    public float speedMultiplier = 1.5f;
    public float fireRateMultiplier = 1.25f;
}