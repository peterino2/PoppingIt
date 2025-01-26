using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public Texture2D burnCursorTexture;

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

    [SerializeField] 
    TMP_Text comboText;

    int combo = 0;

    public bool combosUnlocked = false;
    public double comboMultiplier = 1.0f;
    public int comboMultStepSize = 10;

    double currentComboMultiplier = 1.0f;

    public float maxComboTimer = 0.2f;
    public float curComboTimer = 0.0f;

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
        combosUnlocked = false;
        comboMultiplier = 1.0f;
        currentComboMultiplier = 1.0f;
        _time = 0f;
        ScoreManager = ScoreManager.get();
        ScoreManager.ResetScore();
        bubbles = new List<GameObject>(1000);

        for (int i = 0; i < 100; i++)
        {
            bubbles.Add(Instantiate(bubble1));
            bubbles[i].GetComponent<Renderer>().enabled = false;
            bubbles[i].SetActive(false);
        }

        for(int i = 1; i < 10; i++)
        {
            Invoke("CreateBubbles", i * 3);
        }
        
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);

        milestones.Sort((ScoreTrigger lhs, ScoreTrigger rhs) => { return (int)(lhs.Score - rhs.Score); });
    }

    void CreateBubbles()
    {
        for (int i = 0; i < 100; i++)
        {
            bubbles.Add(Instantiate(bubble1));
            bubbles[bubbles.Count - 1].GetComponent<Renderer>().enabled = false;
            bubbles[bubbles.Count - 1].SetActive(false);
        }
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
        if (popperChainTime < 0)
        {
            popperChainTime += popperChainInterval;
            popperChain.update();
        }

        updateRandomPopper();
        updateMouseBurn();
        {
            chainLightingProjectile.transform.position = Vector3.Lerp(chainLightingProjectile.transform.position, popperChain.position, 0.3f);
        }

        foreach (GameObject b in BubblesToRemove)
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

        curComboTimer -= Time.deltaTime;
        if(curComboTimer <= 0)
        {
            curComboTimer = 0;
            combo = 0;
            comboText.text = "";
            currentComboMultiplier = 1.0f;
        }
    }

    public float mouseBurnRadius = 0.05f;
    public float mouseBurnDamage = 1;
    public float mouseBurnInterval = 0.2f;
    public int mouseBurnTargetCount = 1;
    public bool burnReady = true;
    float mouseBurnTime = 0.0f;

    public SpriteRenderer skyRenderer;

    Collider2D[] results = new Collider2D[50];
    void updateMouseBurn()
    {
        if(!burnReady)
            return;

        if (Input.GetMouseButton(0))
        {
            Cursor.SetCursor(burnCursorTexture, Vector2.zero, CursorMode.ForceSoftware);
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
                    for(int i = 0; i < numColliders && i < mouseBurnTargetCount; i += 1)
                    {
                        DamageBubble(results[i].gameObject, true);
                    }
                }
            }
        }
        else
        {
            Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.ForceSoftware);
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
            for (int j = 0; j < 100; j++)
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
                bubbles[i].GetComponent<CircleCollider2D>().enabled = true;
                spawned = true;
                bubbles[i].GetComponent<Bubble>().ReEnable(spawner.GetSpawnerLocation());
                Debug.Log("SPawned");
            }
            i++;
        }
    }

    public int hpPerBubble = 1;
    public double scorePerBubble = 1;

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
            if (triggerSecondaryEffects)
            {
                if (Random.Range(0.0f, 1.0f) < popperChainChance && popperChain.isFinished())
                {
                    popperChain.startChain(bubble.transform.position, popperChainMaxCount - 1, popperChainRadius, popperChainChance);
                }
            }

            bub.takeDamage(currentDamage);
            if (bub.curHP <= 0)
            {
                IncrementCombo();
                ScoreManager.IncrementScore(scorePerBubble * currentComboMultiplier);
                AudioSystem.get().playPop();
                //RemoveBubble(bubble);
                bubble.GetComponent<CircleCollider2D>().enabled = false;
                //StartCoroutine(AnimWait());

                //BubblesToRemove.Add(bubble);

            }
        }
        else
        {
            return;
        }
    }

    void IncrementCombo()
    {
        if(!combosUnlocked) 
        {
            return; 
        }

        combo++;
        currentComboMultiplier = comboMultiplier * (combo / comboMultStepSize);
        if(currentComboMultiplier < 1.0)
        {
            currentComboMultiplier = 1.0;
        }

        comboText.text = "Chain: " + combo.ToString();
        comboText.fontSize = 18 + combo / comboMultStepSize;
        curComboTimer = maxComboTimer;
    }

    public void addToRemoveList(GameObject bubble) { 
        BubblesToRemove.Add(bubble);
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


    /// ------------------
    
    float passiveGenerator;
    
    // pops
    public float randomPopperRate = 0.0f;

    float randomPopperTime = 0.0f;
    
    void updateRandomPopper()
    {
        if(randomPopperRate > 0)
        {
            randomPopperTime -= Time.deltaTime;
            if(randomPopperTime <= 0)
            {
                bool spawned = false;
                int i = 0;
                while (!spawned && i < bubbles.Count)
                {
                    if (bubbles[i].gameObject.GetComponent<Renderer>().enabled) {
                        DamageBubble(bubbles[i].gameObject, false);
                        break;
                    }
                    i += 1;
                }

                randomPopperTime += 1.0f / randomPopperRate;
            }
        }
    }
}
