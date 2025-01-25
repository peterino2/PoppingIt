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
    AudioSource bgmSource;

    [SerializeField] GameObject audioPlayerPrefab;
    [SerializeField] bool playBgm = true;

    [SerializeField] AudioMixer masterMix;
    AudioMixerGroup mixerGroup;

    List<GameObject> popSourceObjects = new List<GameObject>();

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
            bgmSource.Play();
        }
    }

    public static AudioSystem get()
    {
        return gStaticInstance;
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

    // Update is called once per frame
    void Update()
    {
        updatePops();
    }
}
