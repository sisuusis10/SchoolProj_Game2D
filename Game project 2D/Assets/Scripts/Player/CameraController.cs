using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Variables
    public Transform PlayerTransform;

    private Vector3 LerpPos;
    private float Lerp = 0.1f;

    private void Start() {
        
    }
    private void FixedUpdate() {
        LerpPos = Vector3.Lerp(LerpPos, PlayerTransform.position, Lerp);
        LerpPos.z = -10f;
        this.transform.position = LerpPos;
    }

}
