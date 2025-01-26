using System.Collections.Generic;
using UnityEngine;
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
    public Texture2D clickedCursorTexture;

    public List<ScoreTrigger> milestones;

    public int currentMilestone = 0;

    public double currentDamage = 1.0f;

    public enum PopperType
    {
        Single,
        Multiple
    };

    public float bubbleVelocityScale = 0.1f;
    public float spawnRate = 0.3f;

    public PopperType currentPopper;

    [SerializeField] float _interval = 3.0f;
    float _time;

    [SerializeField] GameObject popperChainPrefab;

    GameObject chainLightingProjectile;
    [SerializeField] float chainLightingSpeed = 20.0f;


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
        popperChain = new PopperChain();
        chainLightingProjectile = Instantiate(popperChainPrefab);
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
        
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

        milestones.Sort((ScoreTrigger lhs, ScoreTrigger rhs) => { return (int)(lhs.Score - rhs.Score); });
    }

    void updateInterval()
    {
        _interval = 1.0f / spawnRate;
    }

    // Update is called once per frame
    void Update()
    {

        if(clickCursorDelay > 0)
        {
            clickCursorDelay -= Time.deltaTime;
            if(clickCursorDelay <= 0)
            {
                Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
            }
        }
        popperChainTime -= Time.deltaTime;
        if(popperChainTime < 0)
        {
            popperChainTime += popperChainInterval;
            popperChain.update();
        }

        updateMouseBurn();
        {
            chainLightingProjectile.transform.position = Vector3.Lerp(chainLightingProjectile.transform.position, popperChain.position, 0.3f);
        }

        foreach(GameObject b in BubblesToRemove)
        {
            RemoveBubble(b);
        }
        BubblesToRemove.Clear();

        _time += Time.deltaTime;
        while (_time >= _interval) {
            SpawnBubble();
            _time -= _interval;
        }
        updateInterval();

    }

    public float mouseBurnRadius = 0.5f;
    public float mouseBurnDamage = 1;
    public float mouseBurnInterval = 0.2f;
    float mouseBurnTime = 0.0f;

    Collider2D[] results = new Collider2D[50];
    void updateMouseBurn()
    {
        if (Input.GetMouseButton(0))
        {
            // Code to execute while left mouse button is held down
            if(mouseBurnDamage > 0)
            {
                mouseBurnTime -= Time.deltaTime;
                if(mouseBurnTime <= 0)
                {
                    mouseBurnTime = mouseBurnInterval;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    mouseWorldPosition.z = 0f; // Set z to 0 for 2D games
                    int numColliders = Physics2D.OverlapCircleNonAlloc(mouseWorldPosition, mouseBurnRadius, results);
                    for(int i = 0; i < numColliders; i += 1)
                    {
                        DamageBubble(results[i].gameObject, true);

                    }
                }
            }
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

    [SerializeField] public float popperChainChance = 0.0f;
    [SerializeField] public int popperChainMaxCount = 5;
    [SerializeField] public float popperChainRadius = 1.0f;
    [SerializeField] public float popperChainInterval = 0.1f;

    float popperChainTime = 0.0f;

    PopperChain popperChain;

    List<GameObject> BubblesToRemove = new List<GameObject>();


    public void DamageBubble(GameObject bubble, bool triggerSecondaryEffects)
    {
        Bubble bub = bubble.GetComponent<Bubble>();

        if (bub != null)
        {
            if(triggerSecondaryEffects)
            {
                if(Random.Range(0.0f, 1.0f) < popperChainChance && popperChain.isFinished())
                {
                    popperChain.startChain(bubble.transform.position, popperChainMaxCount - 1, popperChainRadius, popperChainChance);
                }
            }

            bub.takeDamage(currentDamage);
            if (bub.curHP <= 0)
            {
                ScoreManager.IncrementScore();
                AudioSystem.get().playPop();
                //RemoveBubble(bubble);
                BubblesToRemove.Add(bubble);

            }
        }
        else
        {
            return;
        }
    }


    float clickCursorDelay = 0.0f;
    public void OnClick()
    {
        Cursor.SetCursor(clickedCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
        clickCursorDelay = 0.14f;
    }

    public void RemoveBubble(GameObject bubble) 
    {
        bubble.GetComponent<Renderer>().enabled = false;
        bubble.SetActive(false);
    }
}
