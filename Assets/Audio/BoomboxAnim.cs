using UnityEngine;
using UnityEngine.LowLevelPhysics;

public class BoomboxAnim : MonoBehaviour
{
    RectTransform rtrans;
    [SerializeField] float intensity = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rtrans = GetComponent<RectTransform>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float playtime = AudioSystem.get().bgmPlayTime;
        // float skew = AudioSystem.get().skewCurve.Evaluate(playtime);
        float skew = AudioSystem.get().clipLoudness * 10;
        rtrans.rotation = Quaternion.AngleAxis(skew * intensity, Vector3.forward);
        rtrans.localScale = Vector3.one * (skew * 0.3f + 1.0f);
    }
}
