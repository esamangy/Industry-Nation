using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour {
    private const string MUSIC_VOLUME = "MusicVolume";
    public static MusicManager Instance { get; private set;}
    private AudioSource audioSource;
    private float volume = .5f;

    private void Awake() {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        
        volume = PlayerPrefs.GetFloat(MUSIC_VOLUME, .5f);
        audioSource.volume = volume;
    }
    public void ChangeVolume(){
        volume += .1f;
        if(volume > 1f){
            volume = 0;
        }
        audioSource.volume = volume;

        PlayerPrefs.SetFloat(MUSIC_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume(){
        return volume;
    }
}
