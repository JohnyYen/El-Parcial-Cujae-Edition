using System;
using UnityEngine;

public abstract class SoftShotSO : ScriptableObject
{
    public abstract void SoftShot();

    public abstract event Action OnSoftShotExecuted;
}