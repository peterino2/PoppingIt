using UnityEngine;

public class Bubble : MonoBehaviour
{
    public double curHP;
    public double score;
    public Animator animator;

    public SpriteRenderer spriteRend;
    protected Vector3 velocity = Vector3.zero;

    [SerializeField]
    protected float deceleration;


    public Color tint1;
    public Color tint2;
    public Color tint3;

    public Sprite texture1;
    public Sprite texture2;
    public Sprite texture3;

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

        if(transform.position.y > 10.0f)
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
        curHP = GameManager.Instance.hpPerBubble;
        animator.SetFloat("hp", (float)curHP);
        velocity.x = Random.Range(-5f, 5f);
        initialDirection = velocity.x > 0 ? 1 : -1;

        velocity.y = Random.Range(1.0f, 5.0f);

        deceleration = Random.Range(0.05f, 0.15f) * initialDirection;

        if(curHP == 1)
        {
            spriteRend.color = tint1;
        }
        else if(curHP == 2)
        {
            spriteRend.color = tint2;
        }
        else if(curHP > 2)
        {
            spriteRend.color = tint3;
        }
    }

    public void ReEnable(Vector3 position) 
    {
        transform.position = position;
        Setup();
    }


    public void takeDamage(double dmg)
    {
        curHP -= dmg;
        animator.SetFloat("hp", (float)curHP);
        if (curHP > 0) {
            //animator.ResetTrigger("Damaged");
            animator.SetTrigger("Damaged");
            if(curHP == 1)
            {
                spriteRend.color = tint1;
            }
            else if(curHP == 2)
            {
                spriteRend.color = tint2;
            }
            else if(curHP > 2)
            {
                spriteRend.color = tint3;
            }
        }
    }

    public void PendingRemoval() {
        // Debug.Log("Pend Removal");
        GameManager.Instance.addToRemoveList(gameObject);
    }

    void updateSpriteOnHit(double hits) 
    { 
        
    }
}
