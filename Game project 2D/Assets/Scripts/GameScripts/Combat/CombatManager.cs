using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour {

    //Variables
    public static CombatManager combat;

    //Player
    public HealthBarScript PlayerHealthBar;

    //Prefabs
    public GameObject HealthBar_Prefab;
    public GameObject Character_Prefab;

    //Characters
    private Dictionary<int, CombatCharacter> Characters_Dictionary = new Dictionary<int, CombatCharacter>();

    private void Awake() {
        combat = this;
        

    }

    //Generate Scene
    public void GenerateCombatScene(int enemycount) {
        //Generate Enemies
        for(int i = 0; i < enemycount; i++) {
            //Generate Character
            GameObject temp_char = Instantiate(Character_Prefab);
            CombatCharacter temp_charscript = temp_char.GetComponent<CombatCharacter>();
            //Generate HealthBar
            GameObject temp_hpbar = Instantiate(HealthBar_Prefab);

            //stuff
            temp_charscript.CombatID = i;
            temp_charscript.HealthBar = temp_hpbar.GetComponent<HealthBarScript>();

            //Note to self, create scriptable objects / character profiles

            //Add to list
            Characters_Dictionary.Add(i, temp_charscript);
        }
    }

}
