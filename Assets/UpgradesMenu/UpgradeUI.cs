using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeUI : MonoBehaviour
{
    public Image icon;
    public RectTransform rect;

    public TMP_Text header;
    public TMP_Text description;

    public UpgradeMenuManager menuManager;

    public UpgradeBase upgrade = null;

    public void SetData(UpgradeBase newUpgrade, Vector2 newPosition)
    {
        rect.anchoredPosition = newPosition;
        upgrade = newUpgrade;
        icon.sprite = upgrade.sprite;

        header.text = upgrade.upgradeName + ": " + upgrade.cost.ToString();
        description.text = upgrade.description;
    }

    public void SetPosition(Vector2 position)
    {
        rect.anchoredPosition = position;
    }

    public void OnClick()
    {
        menuManager.PurchaseUpgrade(this);
    }
}
