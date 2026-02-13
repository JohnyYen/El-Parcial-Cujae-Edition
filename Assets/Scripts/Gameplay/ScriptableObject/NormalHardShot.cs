using System;
using UnityEngine;

public class NormalHardShot : HardShotSO
{
    public override event Action OnHardShotExecuted;

    public override void HardShot()
    {
        OnHardShotExecuted?.Invoke();
    }
}