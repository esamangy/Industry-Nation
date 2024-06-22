using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ProgressBarUI))]
public class IndustrialSanderRuinWarningUI : MonoBehaviour {
    [SerializeField] private IndustrialSander industrialSander;
    private ProgressBarUI progressBarUI;
    private float playWarningSoundThreshold = .5f;
    private void Awake() {
        progressBarUI = GetComponent<ProgressBarUI>();
    }
    private void Start() {
        industrialSander.OnProgressChanged += IndustrialSander_OnProgressChanged;

        progressBarUI.ToggleWarning(false);
    }

    private void IndustrialSander_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
        bool show = industrialSander.IsSanded() && e.progressNormalized >= playWarningSoundThreshold;
        progressBarUI.ToggleWarning(show);
    }
}
