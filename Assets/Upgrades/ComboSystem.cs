using UnityEngine;

public class ComboSystem : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.combosUnlocked = true;
    }
}
