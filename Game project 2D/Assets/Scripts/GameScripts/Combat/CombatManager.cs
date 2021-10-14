using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    //Variables
    public static CombatManager combat;

    //Player
    public HealthBarScript PlayerHealthBar;

    //Components
    public Canvas UI_Canvas;

    //Prefabs
    public GameObject HealthBar_Prefab;
    public GameObject Character_Prefab;

    //Characters
    public CharacterProfile_Scriptable[] CharacterProfiles;
    private Dictionary<int, CombatCharacter> Characters_Dictionary = new Dictionary<int, CombatCharacter>();

    private void Awake() {
        combat = this;
    }

    //Debug
    private void Start() {
        GenerateCombatScene(CharacterProfiles);
    }

    //Generate Scene
    public void GenerateCombatScene(CharacterProfile_Scriptable[] profiles) {
        int enemycount = profiles.Length;
        //Generate Enemies
        for (int i = 0; i < enemycount; i++) {
            //Generate Character
            GameObject temp_char = Instantiate(Character_Prefab);
            CombatCharacter temp_charscript = temp_char.GetComponent<CombatCharacter>();
            
            //Generate HealthBar
            GameObject temp_hpbar = Instantiate(HealthBar_Prefab);
            temp_hpbar.transform.SetParent(UI_Canvas.transform);
            temp_hpbar.transform.position = UI_Canvas.transform.position;

            //Initialize Character
            temp_charscript.CombatID = i;
            temp_charscript.HealthBar = temp_hpbar.GetComponent<HealthBarScript>();
            temp_charscript.Profile = profiles[i];
            temp_charscript.Initialize();

            //Add to list
            Characters_Dictionary.Add(i, temp_charscript);
        }
    }

}
