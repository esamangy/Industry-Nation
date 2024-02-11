using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayEffects : MonoBehaviour {
    [SerializeField] private ParticleSystem[] particleSystems;
    private void PlayAllParticleEffects(){
        foreach (ParticleSystem system in particleSystems) {
            system.Play();
        }
    }
}
