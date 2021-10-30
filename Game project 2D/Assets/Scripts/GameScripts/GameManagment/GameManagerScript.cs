using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour {

    //Variables
    public static GameManagerScript game;
    public static ControlsManager controls;


    //Components
    public CameraController Cam;

    //Prefabs
    public GameObject CombatManagerPrefab, TransitionPrefab, CanvasPrefab;

    //positions
    private Vector3 Combat_Pos;

    //Objects
    private GameObject PlayerObj, CombatManagerObj, TransitionObj;
    private GameObject MainCanvasObj;

    private void Awake() {
        game = this;
        controls = this.GetComponent<ControlsManager>();

        //Set combat position
        Combat_Pos = new Vector3(0f, 0f, 25f);

        //Start Checks
        StartCheck();
    }

    public void StartCheck() {
        //Get cam
        Cam = Camera.main.GetComponent<CameraController>();
        
        //Get player
        if(GameObject.FindObjectOfType<PlayerController>()) {
            PlayerObj = GameObject.FindObjectOfType<PlayerController>().gameObject;
        }
        //Check for Main Canvas
        if (!GameObject.FindGameObjectWithTag("MainCanvas")) {
            GameObject prefab_canvas = Instantiate(CanvasPrefab);
            MainCanvasObj = prefab_canvas;
        }

        //Check for Combat Prefab
        if (!GameObject.FindGameObjectWithTag("CombatManager")) {
            GameObject prefab_combat = Instantiate(CombatManagerPrefab, Combat_Pos, Quaternion.identity);
            CombatManagerObj = prefab_combat;
        }
        //Check for Transition Prefab
        if (!GameObject.FindObjectOfType<TransitionHandler>()) {
            GameObject prefab_transition = Instantiate(TransitionPrefab);
            TransitionObj = prefab_transition;
        }
    }

    private void FixedUpdate() {
    }
    
    public void SetMainGameState(bool IsVisible) {
        PlayerObj.SetActive(IsVisible);
        MainCanvasObj.SetActive(IsVisible);
    }

}
