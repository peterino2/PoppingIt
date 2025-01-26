using UnityEngine;

public class BubbleWand3 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate += 15.0f;
    }
}
