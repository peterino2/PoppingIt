using UnityEngine;

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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
