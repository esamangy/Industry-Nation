using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnvilVisual : MonoBehaviour {
    private const string HAMMER = "Hammer";
    [SerializeField] private Anvil anvil;
    private Animator animator;

    private void Awake(){
        animator = GetComponent<Animator>();
    }

    private void Start(){
        anvil.OnHammer += Anvil_OnPlayerGrabbedObject;
    }

    private void Anvil_OnPlayerGrabbedObject(object sender, EventArgs e) {
        animator.SetTrigger(HAMMER);
    }

    private void OnDisable() {
        anvil.OnHammer -= Anvil_OnPlayerGrabbedObject;
    }
}
