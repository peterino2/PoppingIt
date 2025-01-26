using UnityEngine;

public class ComboMultiplierUpgrade : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.comboMultiplier += 1.0f;
    }
}
