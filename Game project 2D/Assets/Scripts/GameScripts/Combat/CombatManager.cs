using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatManager : MonoBehaviour {

    //Variables
    public static CombatManager combat;

    public bool IsVisible, IsActive = false;

    //Player
    public HealthBarScript PlayerHealthBar;
    public CombatCharacter PlayerCharacter;
    public GameObject Menu_BG;
    
    //Components
    public Canvas UI_Canvas;
    public GameObject ComponentsObject;
    //Prefabs
    public GameObject HealthBar_Prefab;
    public GameObject Character_Prefab;

    //Characters
    public CharacterProfile_Scriptable[] CharacterProfiles;
    private Dictionary<int, CombatCharacter> Characters_Dictionary = new Dictionary<int, CombatCharacter>();

    //GenerateType
    public enum Gen_Styles { FromTop_Down, Sine }

    //MemoryBank
    private Gen_Styles Mem_Style;
    private CharacterProfile_Scriptable[] Mem_Profiles;

    private List<GameObject> ObjectList = new List<GameObject>();

    //State Triggers
    private enum CombatStates { None, Initiate, Active, Exit }
    private CombatStates CurrentCombatState;

    //Turns
    private bool IsPlayerTurn = true;

    private void Awake() {
        combat = this;
        SetVisibility(false);
    }

    //Initiate
    public void Men_Initiate(Gen_Styles style, CharacterProfile_Scriptable[] profiles) {
        Mem_Style = style;
        Mem_Profiles = profiles;
        print("initiated");
        //Switch State
        SetState(CombatStates.Initiate);
    }

    private void FixedUpdate() {
        //State
        if (CurrentCombatState == CombatStates.Initiate) {
            if (TransitionHandler.transitions.CurrentTransition == TransitionHandler.Transitions.None) {
                SetVisibility(true);
                if (IsVisible) {
                    SwitchScene(CameraController.SceneView.Combat);
                    SetState(CombatStates.Active);
                }
            }
        }

        //Exit Combat state code
        if (CurrentCombatState == CombatStates.Exit) {
            if (TransitionHandler.transitions.CurrentTransition == TransitionHandler.Transitions.None) {
                SetVisibility(false);
                if (!IsVisible) {
                    SwitchScene(CameraController.SceneView.Default);
                    SetState(CombatStates.None);
                    TransitionHandler.transitions.SetTransition(TransitionHandler.Transitions.Fade_ZoomEffect, TransitionHandler.Directions.Transition_Out, 0.1f);
                    ExitCombat();
                }
            }
        }
    }

    //Switch State
    private void SwitchScene(CameraController.SceneView ViewState) {
        switch(ViewState) {
            case CameraController.SceneView.Default:
                CameraController.cam.CurrentSceneView = CameraController.SceneView.Default;
                break;
            case CameraController.SceneView.Combat:
                CameraController.cam.CombatScenePos = new Vector3(ComponentsObject.transform.position.x, ComponentsObject.transform.position.y, ComponentsObject.transform.position.z - 10f);
                CameraController.cam.CurrentSceneView = CameraController.SceneView.Combat;
                GenerateCombatScene(Mem_Style, Mem_Profiles);
                break;
        }
    }

    private void SetState(CombatStates _state) {
        switch(_state) {
            case CombatStates.Initiate: //Start Transition
                TransitionHandler.transitions.SetTransition(TransitionHandler.Transitions.Fade_ZoomEffect, TransitionHandler.Directions.Transition_In, 0.1f);
                break;
            case CombatStates.Active: //Active state
                IsActive = true;
                break;
            case CombatStates.Exit: //Start Transition
                TransitionHandler.transitions.SetTransition(TransitionHandler.Transitions.Fade_ZoomEffect, TransitionHandler.Directions.Transition_In, 0.1f);
                IsActive = false;
                break;
        }
        CurrentCombatState = _state; //Set state
    }

    private void Update() {
        //Active Combat state code
        if (CurrentCombatState == CombatStates.Active) {
            Combat();
        }
    }

    //Variables
    private float Turn_timer, Turn_TimerMax = 2f;
    //Active Code
    public void Combat() {
        //Turn Timer
        if (Turn_timer < Turn_TimerMax) {
            Turn_timer += Time.deltaTime;
        } else {
            if (GameManagerScript.controls.InputState(ControlsManager.InputTypes.Interact, false) && IsPlayerTurn) {
                SetTurn(false);
            } else {
                SetTurn(true);
                    PlayerCharacter.DealDamage(10);
            }
        }

        //!! DEBUG !!
        if (GameManagerScript.controls.InputState(ControlsManager.InputTypes.Pause, false)) {
            SetState(CombatStates.Exit);
        }

    }

    private void SetTurn(bool player_turn) {
        IsPlayerTurn = player_turn;
        Menu_BG.SetActive(player_turn);
        Turn_timer = 0f;
    }

    //Set Visibility
    public void SetVisibility(bool _IsVisible) {
        ComponentsObject.SetActive(_IsVisible);
        IsVisible = _IsVisible;
        GameManagerScript.game.SetMainGameState((_IsVisible) ? false : true); //Invert Visibility state of combat scene and game scene
    }

    //Exit Combat
    public void ExitCombat() {
        //Update player data
        GameManagerScript.player.Health = PlayerCharacter.HP;

        //Clear Combat Scene
        Characters_Dictionary.Clear();
        foreach (GameObject g in ObjectList) {
            Destroy(g);
        }
    }

    //Generate Scene
    public void GenerateCombatScene(Gen_Styles style, CharacterProfile_Scriptable[] profiles) {
        //Retrive Player data
        PlayerCharacter.HP_Max = GameManagerScript.player.Health_Max;
        PlayerCharacter.HP = GameManagerScript.player.Health;

        //Counter
        int enemycount = profiles.Length;
        //Style
        Vector2 GenPos = Vector2.zero;

        //Generate Enemies
        for (int i = 0; i < enemycount; i++) {
            //Generate Character
            GameObject temp_char = Instantiate(Character_Prefab, ComponentsObject.transform);

            //Get script
            CombatCharacter temp_charscript = temp_char.GetComponent<CombatCharacter>();
            
            //Generate HealthBar
            GameObject temp_hpbar = Instantiate(HealthBar_Prefab);
            temp_hpbar.transform.SetParent(UI_Canvas.transform);
            temp_hpbar.GetComponent<RectTransform>().localScale = PlayerHealthBar.GetComponent<RectTransform>().localScale; //Get scale from player'S healthbar

            float X = UI_Canvas.GetComponent<RectTransform>().position.x + UI_Canvas.GetComponent<RectTransform>().rect.xMax;
            float X_Offset = (temp_hpbar.GetComponent<Image>().rectTransform.rect.width * 1.5f);
            float Y = PlayerHealthBar.transform.position.y; //Get Y position from player's healthbar
            float Z = UI_Canvas.GetComponent<RectTransform>().position.z;
            temp_hpbar.transform.position = new Vector3(X - (X_Offset * 1+i), Y, Z);

            //Initialize Character
            temp_charscript.CombatID = i;
            temp_charscript.Profile = profiles[i];
            temp_charscript.Initialize(style, temp_hpbar.GetComponent<HealthBarScript>());

            //Add to list
            Characters_Dictionary.Add(i, temp_charscript);
            ObjectList.Add(temp_hpbar);
            ObjectList.Add(temp_char);
        }
        //Done generating
        TransitionHandler.transitions.SetTransition(TransitionHandler.Transitions.Fade_ZoomEffect, TransitionHandler.Directions.Transition_Out, 0.1f);
        //Set Combat to active
        IsActive = true;
    }

    

}
