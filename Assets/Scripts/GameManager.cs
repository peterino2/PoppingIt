using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    List<GameObject> bubbles;
    public GameObject bubble1;

    [SerializeField] float _interval = 3.0f;
    float _time;

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
        _time = 0f;
        bubbles = new List<GameObject>();
        var dist = (transform.position - Camera.main.transform.position).z;
        var leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).x;
        var rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
        var topBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, dist)).y;
        var botBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;

        for (float i = leftBorder; i <  rightBorder; i+=2)
        {
            for (float j = topBorder; j < botBorder; j+=2)
            {
                float posX = Mathf.Clamp(i, leftBorder, rightBorder);
                float posY = Mathf.Clamp(j, topBorder, botBorder);

                GameObject go = Instantiate(bubble1, new Vector3(posX, posY, 0), Quaternion.identity);
                go.transform.localScale = Vector3.one;
                bubbles.Add(go);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        while (_time >= _interval) {
            SpawnBubble();
            _time -= _interval;
        }
    }

    public void AddBubble(int tier, float xPos, float yPos) 
    { 
        
    }

    private void SpawnBubble() 
    {
        bool spawned = false;
        int i = 0;
        while (!spawned && i < bubbles.Count)
        {
            if (!bubbles[i].gameObject.GetComponent<Renderer>().enabled) {
                bubbles[i].gameObject.GetComponent<Renderer>().enabled = true;
                spawned = true;
                bubbles[i].GetComponent<Bubble>().ReEnable();
                Debug.Log("SPawned");
            }
            i++;
        }
    }

    public void DamageBubble(GameObject bubble)
    {
        Bubble bub = bubble.GetComponent<Bubble>();

        if (bub != null)
        {
            bub.takeDamage(1);
            if (bub.curHP <= 0)
                RemoveBubble(bubble);
        }
        else
        {
            return;
        }
    }

    public void RemoveBubble(GameObject bubble) 
    {
        bubble.GetComponent<Renderer>().enabled = false;
    }
}
