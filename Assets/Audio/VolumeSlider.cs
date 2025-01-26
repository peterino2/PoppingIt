using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{

    [SerializeField] AudioMixer mixer;
    [SerializeField] Scrollbar scrollbar;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    float ConvertToDecibel(float volume)
    {
        // Ensure volume is in the 0-1 range
        volume = Mathf.Clamp01(volume);
        
        // Convert to decibels
        float dB = volume > 0 ? 20f * Mathf.Log10(volume) : -80f;
        
        // Clamp the lower bound to -80dB
        return Mathf.Max(dB, -80f);
    }

    public void SetVolume()
    {
        float volume = scrollbar.value;
        mixer.SetFloat("MasterVolume", ConvertToDecibel(volume));
        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
