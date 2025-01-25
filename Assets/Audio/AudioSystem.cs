using UnityEngine;
using System.Collections.Generic;

public class AudioSystem : MonoBehaviour
{
    static AudioSystem gStaticInstance;
    const int ClipCount = 10;

    [SerializeField] AudioClip[] pops = new AudioClip[ClipCount];

    List<AudioSource> popSources = new List<AudioSource>();
    List<int> freeSources = new List<int>();
    List<int> playingList = new List<int>();

    AudioSource BgmSource;

    [SerializeField] GameObject audioPlayerPrefab;

    List<GameObject> popSourceObjects = new List<GameObject>();

    private void Awake()
    {
        gStaticInstance = this;
    }

    void Start()
    {
        for(int i = 0; i < 20; i += 1)
        {
            popSourceObjects.Add(Instantiate(audioPlayerPrefab));
            freeSources.Add(i);
            AudioSource source = popSourceObjects[i].GetComponent<AudioSource>();
            popSources.Add(source);
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
