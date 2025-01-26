using UnityEngine;

public class DartsOfWar : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.mouseBurnRadius = 1.0f;
    }
}
