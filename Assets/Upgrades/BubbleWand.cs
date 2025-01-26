using UnityEngine;

public class BubbleWand: MonoBehaviour
{
    public void onUpgrade()
    {
        GameManager.Instance.spawnRate += 0.3f;
    }
}
