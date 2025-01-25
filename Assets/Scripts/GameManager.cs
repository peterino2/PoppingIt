using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    List<Bubble> bubbles;

    public static GameManager Instance
    {
        get { 
            if(_instance == null)
                _instance = new GameManager();
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bubbles = new List<Bubble>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddBubble(int tier, float xPos, float yPos) 
    { 
        
    }

    public void RemoveBubble(GameObject bubble) 
    {
        bubble.GetComponent<Renderer>().enabled = false;
    }
}
