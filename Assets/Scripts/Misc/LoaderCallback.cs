using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoaderCallback : MonoBehaviour {

    private bool isfirstUpdate = true;

    private void Update() {
        if(isfirstUpdate){ // this is technically unnecessary but its here just to make the purpose clear
            isfirstUpdate = false;

            Loader.LoaderCallback();
        }
    }
}
