using UnityEngine;

public class ChainLighting : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.popperChainChance = 1.0f;
    }
}
