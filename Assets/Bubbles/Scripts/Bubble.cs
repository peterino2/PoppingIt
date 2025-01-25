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
        transform.position += velocity;

        if(transform.position.y > 5.0f)
        {
            GameManager.Instance.RemoveBubble(gameObject);
            return;
        }

        if(velocity.x != 0)
        {
            velocity.x -= deceleration;
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
        curHP = 2;
        velocity.x = Random.Range(-0.1f, 0.1f);
        initialDirection = velocity.x > 0 ? 1 : -1;

        velocity.y = Random.Range(0.01f, 0.02f);

        deceleration = Random.Range(0.0005f, 0.0015f) * initialDirection;
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
