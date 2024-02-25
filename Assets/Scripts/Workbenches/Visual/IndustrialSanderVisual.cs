using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IndustrialSanderVisual : MonoBehaviour {
    [SerializeField] private IndustrialSander industrialSander;
    [SerializeField] private GameObject workingLights;
    [SerializeField] private GameObject warningLights;
    [SerializeField] private GameObject ruinedLights;
    [SerializeField] private Material yellowMaterial;
    [SerializeField] private Material redMaterial;
    [SerializeField] private Material RuinMaterial;
    private MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    private void Start() {
        industrialSander.OnStateChanged += IndustrialSander_OnStateChanged;
    }

    private void IndustrialSander_OnStateChanged(object sender, IndustrialSander.OnStateChangedEventArgs e) {
        if(e.state == IndustrialSander.State.Idle){
            workingLights.SetActive(false);
            warningLights.SetActive(false);
            ruinedLights.SetActive(false);
            Material[] materials = meshRenderer.materials;
            materials[3] = yellowMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        } else if(e.state == IndustrialSander.State.Sanding){
            workingLights.SetActive(true);
            warningLights.SetActive(false);
            ruinedLights.SetActive(false);
            Material[] materials = meshRenderer.materials;
            materials[3] = yellowMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        } else if(e.state == IndustrialSander.State.Sanded){
            workingLights.SetActive(false);
            warningLights.SetActive(true);
            ruinedLights.SetActive(false);
            Material[] materials = meshRenderer.materials;
            materials[3] = redMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        } else {
            workingLights.SetActive(false);
            warningLights.SetActive(false);
            ruinedLights.SetActive(true);
            Material[] materials = meshRenderer.materials;
            materials[3] = RuinMaterial;
            meshRenderer.SetMaterials(materials.ToList());
        }
    }
}
