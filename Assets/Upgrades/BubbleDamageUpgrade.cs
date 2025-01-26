using UnityEngine;

public class BubbleDamageUpgrade : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.currentDamage += 1;
    }
}
