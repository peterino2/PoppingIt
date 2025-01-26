using UnityEngine;

public class YouWin: UpgradeBase
{
    public Color ggColor;
    public GameObject Satan;

    public override void onUpgrade()
    {
        GameManager.Instance.spawnRate = 666.0f;
        GameManager.Instance.skyRenderer.color = ggColor;
        Satan.SetActive(true);
        
    }
}
