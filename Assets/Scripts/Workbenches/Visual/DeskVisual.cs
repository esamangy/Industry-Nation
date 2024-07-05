using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskVisual : MonoBehaviour {
    [SerializeField] private Desk desk;
    [SerializeField] private ParticleSystem questionMarks;
    [SerializeField] private ParticleSystem exclamationPoints;
    [SerializeField] private Animator phoneAnimator;
    [SerializeField] private AudioSource phoneAudioSource;
    [SerializeField] private float slowestParticleEmission;
    [SerializeField] private float fastestParticleEmission;
    [SerializeField] private float questionToExclamationChangeThreshold;
    private float ringingTimerMax;
    private int lastRing;
    private int currentRing;
    private void Awake() {
        desk.OnInteractAlt += Desk_OnInteractAlt;
        desk.OnRingingTimerChanged += Desk_OnRingingTimerChanged;
    }

    private void Desk_OnInteractAlt(object sender, BaseWorkbench.PlayerEventArgs e) {
        StopEmittingParticles();
        StopRinging();
    }

    private void Start() {
        ringingTimerMax = desk.GetRingingTimerMax();
    }

    private void Desk_OnRingingTimerChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        currentRing = Mathf.RoundToInt(e.progressNormalized * ringingTimerMax);
        if(e.progressNormalized == 0 || e.progressNormalized == 1) {
            questionMarks.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            exclamationPoints.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        } else if(e.progressNormalized < questionToExclamationChangeThreshold){
            if(!questionMarks.isPlaying){
                questionMarks.Play();
            }
            ParticleSystem.EmissionModule emi = questionMarks.emission;
            emi.rateOverTime = Mathf.Lerp(slowestParticleEmission, fastestParticleEmission, e.progressNormalized/*  / questionToExclamationChangeThreshold */);
            exclamationPoints.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        } else {
            questionMarks.Stop(false, ParticleSystemStopBehavior.StopEmitting);
            if(!exclamationPoints.isPlaying){
                exclamationPoints.Play();
            }
            ParticleSystem.EmissionModule emi = exclamationPoints.emission;
            emi.rateOverTime = Mathf.Lerp(slowestParticleEmission, fastestParticleEmission, e.progressNormalized/*  / questionToExclamationChangeThreshold */);
        }

        if(currentRing != lastRing && currentRing != Mathf.RoundToInt(ringingTimerMax)){
            phoneAnimator.SetTrigger("Ring");
            if(!phoneAudioSource.isPlaying){
                phoneAudioSource.volume = SoundManager.Instance.GetVolume();
                phoneAudioSource.Play();
            }
            lastRing = currentRing;
        }
    }

    private void StopRinging(){
        phoneAudioSource.Stop();
    }

    public void StopEmittingParticles(){
        questionMarks.Stop(false, ParticleSystemStopBehavior.StopEmitting);
        exclamationPoints.Stop(false, ParticleSystemStopBehavior.StopEmitting);
    }
}
