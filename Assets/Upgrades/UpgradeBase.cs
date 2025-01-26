using UnityEngine;

public class UpgradeBase : MonoBehaviour
{
    public Sprite sprite = null;
    public string upgradeName = "";
    public string description = "";
    public double cost = 0;
    public double unlockThreshold = 0;
    public virtual void onUpgrade() { }
}
