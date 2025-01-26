using UnityEngine;
using System.Collections.Generic;

public class PopperChain
{
    List<Bubble> BubbleHitList = new List<Bubble>();

    Collider2D[] results = new Collider2D[50];

    public Vector3 position = Vector3.zero;

    float jumpChance = 0.0f;
    float radius = 0.0f;
    public int jumpsLeft = 0;

    public void update()
    {
        if(jumpsLeft > 0)
        {
            if(Random.Range(0.0f, 1.0f) > jumpChance)
            {
                jumpsLeft = 0;
            }
            else 
            {
                float distance = 1000000f;
                int numColliders = Physics2D.OverlapCircleNonAlloc(position, radius, results);
                bool found = false;
                if(numColliders != 0)
                {
                    Bubble target = results[0].gameObject.GetComponent<Bubble>();
                    for(int i = 0; i < numColliders; i += 1)
                    {
                        Bubble b = results[i].gameObject.GetComponent<Bubble>();
                        if(b != null && !BubbleHitList.Contains(b))
                        {
                            float d = (results[i].transform.position - position).magnitude;
                            if(d < distance || i == 0)
                            {
                                distance = d;
                                target = b;
                                position = results[i].transform.position;
                                found = true;
                            }
                        }

                    }

                    if(found)
                    {
                        BubbleHitList.Add(target);
                        GameManager.Instance.DamageBubble(target.gameObject, false);
                        jumpsLeft -= 1;
                    }
                    else
                    {
                        jumpsLeft = 0;
                    }
                }
                else 
                {
                    jumpsLeft = 0;
                }
            }
        }
    }

    public bool isFinished()
    {
        return jumpsLeft <= 0;
    }

    public void startChain(Vector3 _position, int jumps, float _radius, float chance)
    {
        BubbleHitList.Clear();
        position = _position;
        jumpsLeft = jumps;
        jumpChance = chance;
        radius = _radius;
    }
}
