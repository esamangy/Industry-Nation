using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour {
    private PlayerController player;
    private float footstepTimer;
    [SerializeField] private float footstepTimerMax = .1f;
    [SerializeField] private float volume = 1f;
    private void Awake() {
        player = GetComponent<PlayerController>();
    }

    private void Update() {
        footstepTimer -= Time.deltaTime;
        if(footstepTimer < 0f){
            footstepTimer = footstepTimerMax;

            if(player.IsWalking()){
                SoundManager.Instance.PlayFootstepsSound(player.transform.position, volume);
            }
        }
    }
}
