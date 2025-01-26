using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Image icon;
    public RectTransform rect;

    public UpgradeMenuManager menuManager;

    public UpgradeBase upgrade = null;

    public void SetData(UpgradeBase newUpgrade, Vector2 newPosition)
    {
        rect.anchoredPosition = newPosition;
        upgrade = newUpgrade;
        icon.sprite = upgrade.sprite;
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }

    public void OnClick()
    {
        menuManager.RemoveUpgrade(this);
    }
}
