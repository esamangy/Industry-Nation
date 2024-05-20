using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndustrialSanderAudio : MonoBehaviour {
    [SerializeField] private IndustrialSander industrialSander;
    private AudioSource audioSource;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() {
        industrialSander.OnStateChanged += IndustrialSander_OnStateChanged;

    }

    private void IndustrialSander_OnStateChanged(object sender, IndustrialSander.OnStateChangedEventArgs e) {
        bool playSound = e.state == IndustrialSander.State.Sanding || e.state == IndustrialSander.State.Sanded;
        if(playSound){
            audioSource.Play();
        } else {
            audioSource.Pause();
        }
    }
}
