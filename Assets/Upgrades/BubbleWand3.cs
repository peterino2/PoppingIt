using UnityEngine;

public class BubbleWand3 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate += 30.0f;
    }
}
