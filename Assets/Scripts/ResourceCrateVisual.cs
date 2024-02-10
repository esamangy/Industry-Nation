using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CrateVisual : MonoBehaviour {
    private const string OPEN_CLOSE = "OpenClose";
    [SerializeField] private ResourceCrate resourceCrate;
    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Start(){
        resourceCrate.OnPlayerGrabbedObject += ResourceCrate_OnPlayerGrabbedObject;
    }

    private void ResourceCrate_OnPlayerGrabbedObject(object sender, EventArgs e) {
        animator.SetTrigger(OPEN_CLOSE);
    }
}
