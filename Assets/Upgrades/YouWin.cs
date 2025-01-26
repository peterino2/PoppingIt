using UnityEngine;

public class YouWin: UpgradeBase
{
    public Color ggColor;

    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate = 666.0f;
        GameManager.Instance.skyRenderer.color = ggColor;
    }
}
