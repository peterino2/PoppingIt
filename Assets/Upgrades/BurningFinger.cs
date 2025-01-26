using UnityEngine;

public class BurningFinger : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.burnReady = true;
    }
}
