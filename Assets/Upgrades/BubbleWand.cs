using UnityEngine;

public class BubbleWand : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate += 1.0f;
    }
}
