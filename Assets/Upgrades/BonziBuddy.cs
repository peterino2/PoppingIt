using UnityEngine;

public class BonziBuddy : UpgradeBase
{
    public GameObject bonziBuddyObject;
    public override void onUpgrade()
    {
        GameManager.Instance.randomPopperRate += 2.0f;
        bonziBuddyObject.gameObject.SetActive(true);
    }
}
