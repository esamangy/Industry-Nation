using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    [SerializeField] private AudioClipsRefrencesSO audioClipsRefrencesSO;
    private void Start() {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManager_OnOrderFailed;
    }

    private void DeliveryManager_OnOrderFailed(object sender, EventArgs e) {
        PlaySound(audioClipsRefrencesSO.orderFail, Camera.main.transform.position);
    }

    private void DeliveryManager_OnOrderSuccess(object sender, EventArgs e) {
        PlaySound(audioClipsRefrencesSO.orderSuccess, Camera.main.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f){
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f){
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }
}
