using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject Spawner;

    public float OscillationAmplitude = 2.0f;
    public float OscillationSpeed = 10.0f;

    private Vector2 StartPosition = Vector2.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(OscillationSpeed * Time.deltaTime, 0.0f ,0.0f);
        if (transform.position.x >= StartPosition.x + OscillationAmplitude || transform.position.x <= StartPosition.x - OscillationAmplitude)
        {
            OscillationSpeed *= -1.0f;
        }
        
    }

    public Vector3 GetSpawnerLocation()
    {
        return transform.position;
    }
}
