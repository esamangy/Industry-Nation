using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LogCanterVisual : MonoBehaviour {

    [SerializeField] private LogCanter logCanter;
    [SerializeField] private List<GameObject> workingLights;
    [SerializeField] private List<GameObject> finishedLights;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material greenMaterial;
    private MeshRenderer meshRenderer;

    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    private void Start() {
        logCanter.OnStateChanged += logCanter_OnStateChanged;
    }

    private void logCanter_OnStateChanged(object sender, LogCanter.OnStateChangedEventArgs e) {
        if(e.state == LogCanter.State.Idle){
            for(int i = 0; i < 2; i ++){
                workingLights[i].SetActive(false);
                finishedLights[i].SetActive(false);
            }
            Material[] materials = meshRenderer.materials;
            materials[3] = yellowMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        } else if(e.state == LogCanter.State.Canting){
            for(int i = 0; i < 2; i ++){
                workingLights[i].SetActive(true);
                finishedLights[i].SetActive(false);
            }
            Material[] materials = meshRenderer.materials;
            materials[3] = yellowMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        } else if(e.state == LogCanter.State.Canted){
            for(int i = 0; i < 2; i ++){
                workingLights[i].SetActive(false);
                finishedLights[i].SetActive(true);
            }
            Material[] materials = meshRenderer.materials;
            materials[3] = greenMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        }
    }
}
