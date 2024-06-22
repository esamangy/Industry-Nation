using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Audio Clip Refrences", menuName = "Audio/Audio Clip References", order = 0)]
public class AudioClipsRefrencesSO : ScriptableObject {

    public AudioClip[] anvil;
    public AudioClip[] orderFail;
    public AudioClip[] orderSuccess;
    public AudioClip[] footstep;
    public AudioClip[] objectPickup;
    public AudioClip[] objectDrop;
    public AudioClip[] trash;
    public AudioClip[] warning;
    //add ones for all the sawing

}
