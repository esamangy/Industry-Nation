using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour{

    private Vector2 MoveDir;

    private void OnMove(InputValue value){
        MoveDir = value.Get<Vector2>();
    }

    public Vector2 GetMovementVectorNormalized(){
        return MoveDir.normalized;
    }

}
