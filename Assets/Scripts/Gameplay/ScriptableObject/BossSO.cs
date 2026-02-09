using UnityEngine;


public abstract class BossSO : ScriptableObject, IBoss
{
    // Implementación de las propiedades y métodos de IBoss
    public abstract int CurrentPhase { get; }
    public abstract float MaxHealth { get; }
    public abstract float CurrentHealth { get; }
    public abstract bool IsAlive { get; }

    public abstract void TakeDamage(float amount);
    public abstract void ChangePhase(int phase);
    public abstract void PerformAttack(AttackType type);
    public abstract void SpawnMinion(MinionType type);
}