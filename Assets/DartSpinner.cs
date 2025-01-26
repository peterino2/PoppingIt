using UnityEngine;

public class DartSpinner : MonoBehaviour
{
    public float rotationSpeed = 30.0f;
    public SpriteRenderer spriteRenderer;

    private Quaternion targetRotation;
    float totalTime = 0.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        totalTime += Time.deltaTime;

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0.0f;
        transform.position = mouseWorldPosition;
        transform.rotation = Quaternion.Euler(0, 0, totalTime * rotationSpeed);

        spriteRenderer.enabled = GameManager.Instance.mouseBurnRadius > 0.1f && Input.GetMouseButton(0);
    }
}
