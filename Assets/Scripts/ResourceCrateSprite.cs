using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ResourceCrateSprite : MonoBehaviour {
    [SerializeField] private ResourceCrate resourceCrate;
    private SpriteRenderer spriteRenderer;
    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start(){
        if(resourceCrate.GetFactoryObjectSO() == null || spriteRenderer == null){
            return;
        }
        spriteRenderer.sprite = resourceCrate.GetFactoryObjectSO().sprite;
    }
}
