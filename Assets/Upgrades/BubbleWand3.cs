using UnityEngine;

public class BubbleWand3: MonoBehaviour
{
    public static void onUpgrade()
    {
        GameManager.Instance.spawnRate += 3.0f;
    }
}
