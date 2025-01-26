using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.Events;

[System.Serializable]
public class ScoreTrigger
{
    public double Score = 0;
    public UnityEvent OnScoreTriggered;

    public void NotifyScoreTriggered()
    {
        OnScoreTriggered.Invoke();
    }
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    public ScoreManager ScoreManager;

    List<GameObject> bubbles;

    public GameObject bubble1;

    public BubbleSpawner spawner;

    public Texture2D cursorTexture;

    public List<ScoreTrigger> milestones;

    public int currentMilestone = 0;

    public double currentDamage = 1.0f;

    public enum PopperType
    {
        Single,
        Multiple
    };
    public PopperType currentPopper;

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
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        _instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _time = 0f;
        ScoreManager = ScoreManager.get();
        ScoreManager.ResetScore();
        bubbles = new List<GameObject>(1000);
        for (int i = 0; i < 1000; i++)
        {
            bubbles.Add(Instantiate(bubble1));
            bubbles[i].GetComponent<Renderer>().enabled = false;
        }
        
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        milestones.Sort((ScoreTrigger lhs, ScoreTrigger rhs) => { return (int)(lhs.Score - rhs.Score); });
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

        if(bubbles.Count == 0)
        {
            for (int j = 0; j < 1000; j++)
            {
                bubbles.Add(Instantiate(bubble1));
                bubbles[i].GetComponent<Renderer>().enabled = false;
            }
        }

        while (!spawned && i < bubbles.Count)
        {
            if (!bubbles[i].gameObject.GetComponent<Renderer>().enabled) {
                bubbles[i].gameObject.GetComponent<Renderer>().enabled = true;
                bubbles[i].gameObject.SetActive(true);
                spawned = true;
                bubbles[i].GetComponent<Bubble>().ReEnable(spawner.GetSpawnerLocation());
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
            bub.takeDamage(currentDamage);
            if (bub.curHP <= 0)
            {
                ScoreManager.IncrementScore();
                AudioSystem.get().playPop();
                RemoveBubble(bubble);
            }
        }
        else
        {
            return;
        }
    }

    public void RemoveBubble(GameObject bubble) 
    {
        bubble.GetComponent<Renderer>().enabled = false;
        bubble.SetActive(false);
    }
}
