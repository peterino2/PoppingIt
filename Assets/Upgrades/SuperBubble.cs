using UnityEngine;

public class SuperBubble : UpgradeBase
{
    public override void onUpgrade()
    {
        GameManager.Instance.scorePerBubble = 10;
        GameManager.Instance.hpPerBubble = 2;
    }
}
