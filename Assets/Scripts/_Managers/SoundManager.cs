using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    private const string SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";
    [SerializeField] private AudioClipsRefrencesSO audioClipsRefrencesSO;
    public static SoundManager Instance{get; private set;}
    private float volume = .5f;
    private void Awake() {
        Instance = this;

        volume = PlayerPrefs.GetFloat(SOUND_EFFECTS_VOLUME, .5f);
    }
    private void Start() {
        DeliveryManager.Instance.OnOrderSuccess += DeliveryManager_OnOrderSuccess;
        DeliveryManager.Instance.OnOrderFailed += DeliveryManager_OnOrderFailed;
        Anvil.OnAnyHammer += Anvil_OnAnyHammer;
        PlayerController.Instance.OnGrabbedSomething += PlayerController_OnGrabbedSomething;
        BaseWorkbench.OnAnyObjectPlacedHere += BaseWorkbench_OnAnyObjectPlacedHere;
        TrashCan.OnAnyObjectTrashed += TrashCan_OnAnyObjectTrashed;
    }

    private void TrashCan_OnAnyObjectTrashed(object sender, EventArgs e) {
        TrashCan trashCan = sender as TrashCan;
        PlaySound(audioClipsRefrencesSO.trash, trashCan.transform.position);
    }

    private void BaseWorkbench_OnAnyObjectPlacedHere(object sender, EventArgs e) {
        BaseWorkbench baseWorkbench = sender as BaseWorkbench;
        PlaySound(audioClipsRefrencesSO.objectDrop, baseWorkbench.transform.position);
    }

    private void PlayerController_OnGrabbedSomething(object sender, EventArgs e) {
        PlaySound(audioClipsRefrencesSO.objectPickup, PlayerController.Instance.transform.position);
    }

    private void Anvil_OnAnyHammer(object sender, EventArgs e) {
        Anvil anvil = sender as Anvil;
        PlaySound(audioClipsRefrencesSO.anvil, anvil.transform.position);
    }

    private void DeliveryManager_OnOrderFailed(object sender, EventArgs e) {
        LoadingDock loadingDock = LoadingDock.Instance;
        PlaySound(audioClipsRefrencesSO.orderFail, loadingDock.transform.position);
    }

    private void DeliveryManager_OnOrderSuccess(object sender, EventArgs e) {
        LoadingDock loadingDock = LoadingDock.Instance;
        PlaySound(audioClipsRefrencesSO.orderSuccess, loadingDock.transform.position);
    }

    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f){
        PlaySound(audioClipArray[UnityEngine.Random.Range(0, audioClipArray.Length)], position, volume);
    }

    public void PlayFootstepsSound(Vector3 position, float volume){
        PlaySound(audioClipsRefrencesSO.footstep, position, volume);
    }
    private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f){
        AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
    }

    public void ChangeVolume(){
        volume += .1f;
        if(volume > 1f){
            volume = 0;
        }

        PlayerPrefs.SetFloat(SOUND_EFFECTS_VOLUME, volume);
        PlayerPrefs.Save();
    }

    public float GetVolume(){
        return volume;
    }
}
