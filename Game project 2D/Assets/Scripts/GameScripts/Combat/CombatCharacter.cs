using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatCharacter : MonoBehaviour {

    //Variables
    public string Character_Name;
    public int HP, HP_Max;

    //Combat System
    public int CombatID;

    //Components
    public HealthBarScript HealthBar;

    //Profile
    public CharacterProfile_Scriptable Profile;

    //Initialize
    public void Initialize() {
        //Set variables
        Character_Name = Profile.CharacterName;
        HP = Profile.Health;
        HP_Max = Profile.Health;

        //initialize health bar
        HealthBar.initialize(Character_Name, HP, HP_Max);
    }

}
