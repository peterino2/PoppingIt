using UnityEngine;

public class ChainLighting3 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.popperChainMaxCount = 20;
        GameManager.Instance.popperChainInterval = 0.03f;
    }
}
