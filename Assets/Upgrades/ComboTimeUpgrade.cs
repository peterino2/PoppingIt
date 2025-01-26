using UnityEngine;

public class ComboTimeUpgrade : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.maxComboTimer += 0.2f;
    }
}