using System;
using UnityEngine;

public class NormalSoftShot : SoftShotSO
{
    public override event Action OnSoftShotExecuted;

    public override void SoftShot()
    {
        OnSoftShotExecuted?.Invoke();
    }
}