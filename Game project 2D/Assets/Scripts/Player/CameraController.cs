using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    //Variables
    public static CameraController cam;

    public Transform PlayerTransform;

    private Vector3 LerpPos;
    private float Lerp = 0.1f;

    //SceneView
    public enum SceneView { Default, Combat }
    public SceneView CurrentSceneView = SceneView.Default;

    public Vector3 CombatScenePos;

    private void Awake() {
        cam = this;
    }

    private void FixedUpdate() {
        switch(CurrentSceneView) {
            case SceneView.Default:
                LerpPos = Vector3.Lerp(LerpPos, PlayerTransform.position, Lerp);
                LerpPos.z = -10f;
                this.transform.position = LerpPos;
                break;
            case SceneView.Combat:
                this.transform.position = CombatScenePos;
                break;
        }
    }

}
