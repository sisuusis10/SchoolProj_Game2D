using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextHandler : MonoBehaviour {

    //Variables
    public static TextHandler handler;
    public bool IsVisible = false, DoneBuildingText = false;

    //Text
    private string Text_Input;
    public TextMeshProUGUI UI_Text;
    public GameObject Indicator;

    //Components
    public Image Box_Outter, Box_Inner;

    private void Awake() {
        handler = this;
        SetText(""); //Hide on start
    }

    private void Update() {
        Vector2 _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if(_input != Vector2.zero && IsVisible) {
            SetText("");
        }
    }

    private void UpdateVisibility() {
        //get colors
        Color out_color = Box_Outter.color;
        Color in_color = Box_Inner.color;

        //Visibility States
        switch (IsVisible) {
            case true:
                //set alpha
                out_color.a = 1f;
                in_color.a = 1f;
                //Apply Colors
                Box_Outter.color = out_color;
                Box_Inner.color = in_color;
                return;
            case false:
                //set alpha
                out_color.a = 0f;
                in_color.a = 0f;
                //Apply Colors
                Box_Outter.color = out_color;
                Box_Inner.color = in_color;
                //Hide indicator
                Indicator.SetActive(false);
                return;
        }
    }

    public void SetText(string text) {
        if(text == "") { //if Input is empty, stop coroutine & hide TextBox
            UI_Text.text = text;
            StopAllCoroutines();
            IsVisible = false;
        } else { //if Input isnt empty, TextBox start coroutin and make visible
            Text_Input = text;
            StartCoroutine(TypeWriter());
            IsVisible = true;
        }
        //Update Visibility
        UpdateVisibility();
    }

    private IEnumerator TypeWriter() {
        //Split
        string final_text = "";
        char[] c = Text_Input.ToCharArray();
        char last_c = 'a';
        //BuildText
        foreach (char _c in c) {
            DoneBuildingText = false;
            //Set custom Delay
            float delay = 0.07f;
            switch (last_c) {
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
                    delay = 0.1f;
                    break;
                case ' ':
                    delay = Time.deltaTime;
                    break;
            }
            //Set last c
            last_c = _c;
            //Wait
            yield return new WaitForSeconds(delay);

            //Build and Display text
            final_text += _c;
            UI_Text.text = final_text;
        }
        //Is Done
        DoneBuildingText = true;

        //Make Indicator Appear
        Indicator.SetActive(true);
    }

}
