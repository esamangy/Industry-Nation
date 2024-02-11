using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

    private enum Mode{
        LookAt,
        LookAtInverted,
        CameraForward,
        CamerForwardInverted,
    }

    [SerializeField] private Mode mode = Mode.LookAt;

    private void LateUpdate() {
        switch(mode){
            case Mode.LookAt:
                transform.LookAt(Camera.main.transform);
                break;
            case Mode.LookAtInverted:
                Vector3 dirFromCamera = transform.position - Camera.main.transform.position;
                transform.LookAt(transform.position + dirFromCamera);
                break;
            case Mode.CameraForward:
                transform.forward = Camera.main.transform.forward;
                break;
            case Mode.CamerForwardInverted:
                transform.forward = -Camera.main.transform.forward;
                break;
        }
    }
}
