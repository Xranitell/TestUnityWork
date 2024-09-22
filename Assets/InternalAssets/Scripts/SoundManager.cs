using System;
using System.Collections;
using System.Collections.Generic;
using AxGrid.Base;
using AxGrid.Model;
using UnityEngine;

public class SoundManager : MonoBehaviourExtBind
{
    [SerializeField] private AudioSource singleSource;
    [SerializeField] private AudioSource loopSource;
    public static SoundManager Instance { get; set; }
    
    [SerializeField] List<Sound> sounds = new List<Sound>();

    [OnAwake]
    private void Init()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private static AudioClip GetClipByName(string name)
    {
        return Instance.sounds.Find(s => s.name == name).clip;
    }
    
    [Bind("PlaySingleSound")]
    public void PlaySound(string soundName)
    {
        var clip = GetClipByName(soundName);
        
        if (clip == null)
        {
            Debug.LogError($"Sound '{soundName}' not found!");
            return;
        }
        Instance.singleSource.PlayOneShot(clip);
    }
    
    [Bind("PlaySoundLoop")]
    public void PlaySoundLoop(string soundName)
    {
        var clip = GetClipByName(soundName);

        loopSource.clip = clip;
        loopSource.loop = true;
        loopSource.Play();
    }
    
    [Bind("StopSoundLoop")]
    public void StopSoundLoop()
    {
        loopSource.Stop();
        loopSource.clip = null;
        loopSource.loop = false;
    }
}

[Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;
}