using UnityEngine;

public class Bubble : MonoBehaviour
{
    public double curHP;
    public double score;

    protected Vector3 velocity = Vector3.zero;

    [SerializeField]
    protected float deceleration;

    float initialDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime * GameManager.Instance.bubbleVelocityScale;

        if(transform.position.y > 5.0f)
        {
            GameManager.Instance.RemoveBubble(gameObject);
            return;
        }

        if(velocity.x != 0)
        {
            velocity.x -= deceleration * Time.deltaTime;
            if (initialDirection > 0)
            {
                if (velocity.x <= 0)
                {
                    velocity.x = 0;
                }
            }
            else
            {
                if (velocity.x >= 0)
                {
                    velocity.x = 0;
                }
            }
        }
    }

    void Setup()
    {
        curHP = 1;
        velocity.x = Random.Range(-10f, 10f);
        initialDirection = velocity.x > 0 ? 1 : -1;

        velocity.y = Random.Range(5.0f, 10.0f);

        deceleration = Random.Range(0.05f, 0.15f) * initialDirection;
    }

    public void ReEnable(Vector3 position) 
    {
        transform.position = position;
        Setup();
    }


    public void takeDamage(double dmg)
    {
        curHP -= dmg;
    }

    void updateSpriteOnHit(double hits) 
    { 
        
    }
}
