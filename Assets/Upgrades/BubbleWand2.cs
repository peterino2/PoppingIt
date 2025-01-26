using UnityEngine;

public class BubbleWand2 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate += 2.1f;
    }
}
