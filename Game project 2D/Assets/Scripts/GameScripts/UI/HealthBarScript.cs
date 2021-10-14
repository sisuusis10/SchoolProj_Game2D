using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour {

    //Variables
    private string Name;
    private int HP_Current, HP_Max;
    private float Scale_Current = 1f, Scale_Target = 1f;

    //Components
    public Image Image_HPBar;
    public TextMeshProUGUI HP_Text, Char_Name;

    public void initialize(string _name, int _hp, int _hpMax) {
        //Set Name
        Name = _name;
        Char_Name.text = _name;

        //HP
        HP_Current = _hp;
        HP_Max = _hpMax;
        
        //Set text
        HP_Text.text = _hp + " / " + _hpMax;
    }

    public void UpdateDisplay(int _hp) {
        Scale_Target = _hp / HP_Max;
        HP_Text.text = HP_Current + " / " + HP_Max; //Update text
    }

    private void FixedUpdate() {
        if(Scale_Current != Scale_Target) {
            Scale_Current = Mathf.Lerp(Scale_Current, Scale_Target, 0.1f);
            Image_HPBar.transform.localScale = new Vector2(Scale_Current, 1f);
        }
    }
}
