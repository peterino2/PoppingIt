using UnityEngine;

public class BoomboxAnim : MonoBehaviour
{
    RectTransform rtrans;
    [SerializeField] bool flip = false;
    [SerializeField] float intensity = 1.0f;
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

        if(flip)
            skew *= -1;

        transform.localScale = Vector3.one * (skew * intensity * 0.1f + 1.0f);
        
        // rtrans.rotation = Quaternion.AngleAxis(skew * intensity, Vector3.forward);
        //rtrans.localScale = Vector3.one * (skew * 0.3f + 1.0f);
    }
}
