using UnityEngine;

public class DartsOfWar : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.mouseBurnRadius = 1.0f;
        GameManager.Instance.mouseBurnInterval = 0.05f;
        GameManager.Instance.mouseBurnTargetCount = 2;
    }
}
