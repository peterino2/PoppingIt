using UnityEngine;

public class ChainLighting2 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.popperChainRadius = 2.0f;
    }
}
