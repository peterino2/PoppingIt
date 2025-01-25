using UnityEngine;

public class Bubble : MonoBehaviour
{
    public double curHP;
    public double score;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curHP = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReEnable() { 
        
    
    }


    public void takeDamage(double dmg)
    {
        curHP -= dmg;
    }

    void updateSpriteOnHit(double hits) 
    { 
        
    }
}
