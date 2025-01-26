using UnityEngine;

public class BubbleWand2: MonoBehaviour
{
    public static void onUpgrade()
    {
        GameManager.Instance.spawnRate += 1.0f;
    }
}
