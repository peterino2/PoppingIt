using UnityEngine;

public class SuperBubble2 : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.scorePerBubble = 50;
        GameManager.Instance.hpPerBubble = 3;
    }
}
