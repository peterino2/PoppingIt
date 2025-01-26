using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UpgradeMenuState
{
    Closed,
    Opening,
    Open,
    Closing
}


public class UpgradeMenuManager : MonoBehaviour
{
    [SerializeField]
    RectTransform Rect;

    UpgradeMenuState MenuState = UpgradeMenuState.Closed;

    public List<UpgradeBase> lockedUpgrades;

    List<UpgradeUI> UpgradeOptions;

    List<UpgradeUI> InactiveUIPool;

    public GameObject upgradesList;
    public GameObject prefab;

    Vector2 prefabSize;
    int maxColumns;
    int maxRows;

    Vector2 shadowRealm = new Vector2(-8000, -8000);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lockedUpgrades = new List<UpgradeBase>(upgradesList.GetComponents<UpgradeBase>());
        lockedUpgrades.Sort((UpgradeBase lhs, UpgradeBase rhs) => { return (int)(lhs.unlockThreshold - rhs.unlockThreshold); });

        UpgradeOptions = new List<UpgradeUI>();
        InactiveUIPool = new List<UpgradeUI>();
        for (int i = 0; i < 100; i++)
        {
            InactiveUIPool.Add(Instantiate(prefab, transform).GetComponent<UpgradeUI>());
            InactiveUIPool[i].menuManager = this;
            InactiveUIPool[i].gameObject.SetActive(false);
        }

        prefabSize = prefab.GetComponent<RectTransform>().rect.size;
        maxColumns = (int)(Rect.rect.width / prefabSize.x);
        maxRows = (int)(Rect.rect.height / prefabSize.y);
    }

    void UnlockUpgrades()
    {
        double curScore = ScoreManager.get().GetScore();

        List<UpgradeBase> newUpgrades = new List<UpgradeBase>();

        int upgradesAdded = 0;
        for(int i = 0; i < lockedUpgrades.Count; i++)
        {
            if (lockedUpgrades[i].unlockThreshold <= curScore)
            {
                newUpgrades.Add(lockedUpgrades[i]);
                upgradesAdded++;
            }
            else
            {
                break;
            }
        }

        if (upgradesAdded != 0)
        {
            lockedUpgrades.RemoveRange(0, upgradesAdded);
            
            foreach (UpgradeBase upgrade in newUpgrades)
            {
                AppendUpgrade(upgrade);
            }
        }
    }

    public void PurchaseUpgrade(UpgradeUI upgrade)
    {
        if(!ScoreManager.get().SpendScore(upgrade.upgrade.cost))
        {
            return;
        }

        if (upgrade.upgrade != null)
        {
            upgrade.upgrade.onUpgrade();
        }

        int index = UpgradeOptions.IndexOf(upgrade);
        UpgradeOptions[index].gameObject.SetActive(false);
        InactiveUIPool.Add(UpgradeOptions[index]);
        UpgradeOptions.RemoveAt(index);

        for(int i = index; i < UpgradeOptions.Count; i++)
        {
            UpgradeOptions[i].SetPosition(PositionFromIndex(i));
        }
    }

    public void AppendUpgrade(UpgradeBase upgrade)
    {
        if(InactiveUIPool.Count == 0)
        {
            for(int i = 0; i < 20; i++)
            {
                InactiveUIPool.Add(Instantiate(prefab, transform).GetComponent<UpgradeUI>());
                InactiveUIPool[i].menuManager = this;
                InactiveUIPool[i].gameObject.SetActive(false);
            }
        }

        InactiveUIPool[0].SetData(upgrade, PositionFromIndex(UpgradeOptions.Count));
        InactiveUIPool[0].gameObject.SetActive(true);
        UpgradeOptions.Add(InactiveUIPool[0]);
        InactiveUIPool.RemoveAt(0);
    }

    public Vector2 PositionFromIndex(int index)
    {
        if(index < 0)
        {
            return shadowRealm;
        }

        int row = index / maxColumns;

        if(row > maxRows)
        {
            return shadowRealm;
        }

        int col = index % maxColumns;

        return new Vector2(col * prefabSize.x, -row * prefabSize.y);
    }

    void AnimateMenu()
    {
        switch (MenuState)
        {
            case UpgradeMenuState.Closing:
                Rect.anchoredPosition = Vector2.Lerp(Rect.anchoredPosition, Vector2.zero, Time.deltaTime * 5.0f);
                if (Rect.anchoredPosition.x >= 0)
                {
                    Rect.anchoredPosition = Vector2.zero;
                    MenuState = UpgradeMenuState.Closed;
                }
                break;
            case UpgradeMenuState.Opening:
                Rect.anchoredPosition = Vector2.Lerp(Rect.anchoredPosition, new Vector2(-Rect.rect.max.x, 0), Time.deltaTime * 5.0f);
                if (Rect.anchoredPosition.x <= -Rect.rect.max.x)
                {
                    Rect.position = new Vector2(-Rect.rect.max.x, 0);
                    MenuState = UpgradeMenuState.Closed;
                }
                break;
            default:
                return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        AnimateMenu();
        UnlockUpgrades();
    }

    public void ToggleUpgradeMenu()
    {
        switch(MenuState)
        {
            case UpgradeMenuState.Closed:
                MenuState = UpgradeMenuState.Opening;
                break;
            case UpgradeMenuState.Closing:
                MenuState = UpgradeMenuState.Opening;
                break;
            case UpgradeMenuState.Opening:
                MenuState = UpgradeMenuState.Closing;
                break;
            case UpgradeMenuState.Open:
                MenuState = UpgradeMenuState.Closing;
                break;
        }
    }
}
