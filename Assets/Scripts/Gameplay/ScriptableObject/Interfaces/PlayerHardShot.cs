using System;
using UnityEngine;

public abstract class HardShotSO : ScriptableObject
{
    public abstract void HardShot();
    public abstract event Action OnHardShotExecuted;
}