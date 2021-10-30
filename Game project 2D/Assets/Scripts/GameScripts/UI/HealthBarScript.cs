using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour {

    //Variables
    private string Name;
    [SerializeField]
    private float Scale_Current = 1f, Scale_Target = 1f;

    //Components
    public Image Image_HPBar;
    public TextMeshProUGUI HP_Text, Char_Name;

    public void initialize(string _name, int _hp, int _hpMax) {
        //Set Name
        Name = _name;
        Char_Name.text = _name;

        //Update display info
        UpdateDisplay(_hp, _hpMax);
    }

    public void UpdateDisplay(float _hp, float hp_max) {
        HP_Text.text = _hp + " / " + hp_max; //Update text
        Scale_Target = _hp / hp_max;
    }

    private void Update() {
        print(transform.name+" : "+Scale_Target);
        if (Scale_Current != Scale_Target) {
            Scale_Current = Mathf.Lerp(Scale_Current, Scale_Target, 0.01f);
            Image_HPBar.transform.localScale = new Vector2(Scale_Current, 1f);
        }
    }
}
