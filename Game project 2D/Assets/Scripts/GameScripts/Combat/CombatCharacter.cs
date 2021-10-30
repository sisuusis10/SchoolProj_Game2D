using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour {

    //Variables
    public string Character_Name;
    public int HP, HP_Max;

    public bool IsPlayer = false;
    private bool In = false;
    //Combat System
    public int CombatID;

    //Components
    public HealthBarScript HealthBar;

    //Profile
    public CharacterProfile_Scriptable Profile;

    //Initialize
    public void Initialize(CombatManager.Gen_Styles _style, HealthBarScript _healthScript) {
        //Set variables
        Character_Name = Profile.CharacterName;
        HP = Profile.Health;
        HP_Max = Profile.Health;

        HealthBar = _healthScript;

        //Position
        Vector3 _pos = Vector3.zero;
        Vector2 _size = this.GetComponent<SpriteRenderer>().size;
        switch (_style) {
            case CombatManager.Gen_Styles.FromTop_Down:
                Vector2 Offset = new Vector2(1f, 1.5f + _size.y);
                //Apply Values
                _pos = this.transform.position + new Vector3(Offset.x, -2f + (Offset.y * CombatID), 0f);
                //Return
                break;
            case CombatManager.Gen_Styles.Sine:

                //Return
                break;
        }

        this.transform.position = _pos;
        //initialize health bar
        HealthBar.initialize(Character_Name, HP, HP_Max);
    }

    public void DealDamage(int Amount) {
        HP -= Amount;
        if (HP <= 0) {
            HP = 0;
            Kill();
        }
        HealthBar.UpdateDisplay(HP, HP_Max);
    }

    public void Kill() {
        if(IsPlayer) {
            TransitionHandler.transitions.SetTransition(TransitionHandler.Transitions.Fade, TransitionHandler.Directions.Transition_In);
        }
    }
}
