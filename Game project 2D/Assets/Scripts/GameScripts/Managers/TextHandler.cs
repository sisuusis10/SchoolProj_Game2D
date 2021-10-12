using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextHandler : MonoBehaviour {

    //Variables
    public static TextHandler handler;
    public bool IsVisible = false, DoneBuildingText = false;

    //Text
    private string Text_Input;
    public TextMeshProUGUI UI_Text;

    private void Awake() {
        handler = this;
        SetText("");
    }

    private void Update() {
        //Visibility States
        switch(IsVisible) {
            case true:

                break;
            case false:
                
                break;
        }
    }

    public void SetText(string text) {
        Text_Input = text;
        StartCoroutine(TypeWriter());
    }

    private IEnumerator TypeWriter() {
        //Split
        string final_text = "";
        char[] c = Text_Input.ToCharArray();

        //BuildText
        foreach (char _c in c) {
            DoneBuildingText = false;
            //Set custom Delay
            float delay = 0.1f;
            switch (_c) {
                case '.':
                    delay = 0.2f;
                    break;
                case '!':
                    delay = 0.2f;
                    break;
                case '?':
                    delay = 0.15f;
                    break;
                case ',':
                    delay = 0.15f;
                    break;
            }
            //Wait
            yield return new WaitForSeconds(delay);

            //Build and Display text
            final_text += _c;
            UI_Text.text = final_text;
        }
        //Is Done
        DoneBuildingText = true;
    }

}
