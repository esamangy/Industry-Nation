using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialSanderAudio : MonoBehaviour {
    [SerializeField] private IndustrialSander industrialSander;
    private AudioSource audioSource;
    private float playWarningSoundThreshold = .5f;
    private bool playWarningSound;
    private float warningSoundTimer;
    private float warningSoundTimerMax = .2f;
    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        industrialSander.OnStateChanged += IndustrialSander_OnStateChanged;
        industrialSander.OnProgressChanged += IndustrialSander_OnProgressChanged;
    }

    private void IndustrialSander_OnStateChanged(object sender, IndustrialSander.OnStateChangedEventArgs e) {
        bool playSound = e.state == IndustrialSander.State.Sanding || e.state == IndustrialSander.State.Sanded;
        if(playSound){
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }

    private void IndustrialSander_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        playWarningSound = industrialSander.IsSanded() && e.progressNormalized >= playWarningSoundThreshold;
    }

    private void Update() {
        if(playWarningSound){
            warningSoundTimer -= Time.deltaTime;
            if(warningSoundTimer <= 0f){
                warningSoundTimer = warningSoundTimerMax;

                SoundManager.Instance.PlayWarningSound(industrialSander.transform.position);
            }
        }
    }
}
