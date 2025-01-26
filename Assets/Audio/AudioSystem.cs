using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;

public class AudioSystem : MonoBehaviour
{
    static AudioSystem gStaticInstance;
    const int ClipCount = 10;

    [SerializeField] AudioClip[] pops = new AudioClip[ClipCount];
    [SerializeField] AudioClip bgmClip;

    List<AudioSource> popSources = new List<AudioSource>();
    List<int> freeSources = new List<int>();
    List<int> playingList = new List<int>();

    GameObject bgmObject;
    public AudioSource bgmSource;

    public float bgmPlayTime = 0.0f;

    [SerializeField] GameObject audioPlayerPrefab;
    [SerializeField] bool playBgm = true;

    [SerializeField] AudioMixer masterMix;
    AudioMixerGroup mixerGroup;

    List<GameObject> popSourceObjects = new List<GameObject>();

    [SerializeField] public AnimationCurve skewCurve;

    private void Awake()
    {
        gStaticInstance = this;
    }
    void Start()
    {
        AudioMixerGroup[] groups = masterMix.FindMatchingGroups("Master");
        mixerGroup = groups[0];

        bgmObject = Instantiate(audioPlayerPrefab);
        bgmObject.transform.SetParent(this.gameObject.transform);
        bgmSource = bgmObject.GetComponent<AudioSource>();
        bgmSource.outputAudioMixerGroup = mixerGroup;

        for(int i = 0; i < 20; i += 1)
        {
            GameObject newPopObject = Instantiate(audioPlayerPrefab);
            newPopObject.transform.SetParent(this.gameObject.transform);
            popSourceObjects.Add(newPopObject);
            freeSources.Add(i);
            AudioSource source = popSourceObjects[i].GetComponent<AudioSource>();
            popSources.Add(source);
            source.outputAudioMixerGroup = mixerGroup;
        }

        bgmSource.loop = true;
        bgmSource.clip = bgmClip;
        if(playBgm)
        {
            startBgm();
        }
    }

    public static AudioSystem get()
    {
        return gStaticInstance;
    }

    public void stopBgm()
    {
        bgmSource.Pause();
    }

    public void startBgm()
    {
        bgmSource.Play();
        bgmPlayTime = 0;
    }

    public void playPop()
    {
        Debug.Log($"playing, sources count {freeSources.Count}");
        if(freeSources.Count == 0)
        {
            return;
        }

        int index = freeSources[freeSources.Count - 1];
        freeSources.RemoveAt(freeSources.Count - 1);
        AudioClip selectedPop = pops[Random.Range(0, 2)];
        popSources[index].pitch = Random.Range(0.75f, 1.25f);
        popSources[index].PlayOneShot(selectedPop);
        playingList.Add(index);
    }

    List<int> removeList = new List<int>();

    void updatePops()
    {
        // Debug.Log(playingList.Count);

        foreach (int playing in playingList)
        {
            if(!popSources[playing].isPlaying)
            {
                removeList.Add(playing);
                freeSources.Add(playing);
                Debug.Log($"recycling: {playing} lenFree {freeSources.Count}");
            }
        }

        foreach(int remove in removeList)
        {
            playingList.Remove(remove);
        }
        removeList.Clear();
    }

    void updatePlaytime()
    {
        if(bgmSource.isPlaying)
        {
            bgmPlayTime += Time.deltaTime;
        }
    }

    public float updateStep = 0.05f;
    const int sampleDataLength = 1024;

    private float currentUpdateTime = 0f;
    public float clipLoudness;
    private float[] clipSampleData = new float[sampleDataLength];
    void updateAmplitude()
    {
        currentUpdateTime += Time.deltaTime;
        //if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
            bgmSource.GetOutputData(clipSampleData, 0);
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength;
        }
    }

    // Update is called once per frame
    void Update()
    {
        updatePops();
        updatePlaytime();
        updateAmplitude();
    }
}
