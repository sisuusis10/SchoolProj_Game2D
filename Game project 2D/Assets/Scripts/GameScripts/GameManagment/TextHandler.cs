using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextHandler : MonoBehaviour {

    //Variables
    public static TextHandler handler;
    public bool IsVisible = false, DoneBuildingText = false;
    private bool IsSkippable = false, Skip = false, Action_Hide = false;

    //mode
    public enum TextModes { InActive, Default, Character, Selection }
    public TextModes TextMode;
    //Text
    private string[] Text_Input;
    private int TextIndex = 0;

    public TextMeshProUGUI UI_Text;
    public GameObject Indicator;

    //Components
    public Image Box_Outter, Box_Inner;

    //Quick array
    public static string[] QuickArray(string _string) {
        string[] StringArray = new string[1] { _string };
        return StringArray;
    }
    private void Awake() {
        handler = this;
        SetText(QuickArray("")); //Hide on start
    }

    private void Update() {
        TextModeManagement();
    }

    //Modes
    private int PlayerSelected_Index = 0;
    private bool Select = false;
    private void TextModeManagement() {
        switch(TextMode) {
            //Default Text Functionality
            case TextModes.Default:
                //Hide Text box
                Vector2 _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
                if (_input != Vector2.zero && IsVisible) {
                    SetText(QuickArray(""));
                }
                //Skip text / player input
                if (Input.GetKeyDown(KeyCode.E)) {
                    if (IsSkippable) { //Skip
                        Skip = true;
                    } else if (!IsSkippable && DoneBuildingText && IsVisible) { //Next Action
                        SetText(QuickArray("")); //Hide TextBox
                        Action_Hide = true;
                    }
                }
                break;
                //Character / NPC Text
            case TextModes.Character:
                //Code
                break;
                //Player Text Option Selection
            case TextModes.Selection:
                //Player Input
                if(Input.GetAxisRaw("Horizontal") > 0f) {
                    if(!Select)
                    PlayerSelected_Index++;
                    Select = true;
                } else if (Input.GetAxisRaw("Horizontal") < 0f) {
                    if (!Select)
                    PlayerSelected_Index--;
                    Select = true;
                } else {
                    Select = false;
                }
                //Generate
                string DisplayText = "";
                for (int i = 0; i < Text_Input.Length; i++) {
                    if(i != PlayerSelected_Index) {
                        DisplayText += Text_Input[i] + " ";
                    } else {
                        DisplayText += "<s>" + Text_Input[i] + "</s>" + " ";
                    }
                }
                UI_Text.text = DisplayText;
                break;
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
                //Clear Input text
                Text_Input = new string[0];
                Action_Hide = false;
                return;
        }
    }

    public void SetText(string[] text, TextModes _mode = TextModes.InActive) {
        //Return mode
        if(_mode != TextModes.InActive) {
            TextMode = _mode;
        }
        if(text[0] == "") { //if Input is empty, stop coroutine & hide TextBox
            UI_Text.text = text[0];
            StopAllCoroutines();
            IsVisible = false;
        } else if(text != Text_Input && !Action_Hide) { //if Input isnt empty, TextBox start coroutin and make visible
            StopAllCoroutines(); //Make sure no coroutines are active
            Text_Input = text;
            StartCoroutine(TypeWriter());
            IsVisible = true;
            IsSkippable = false;
        }
        //Update Visibility
        UpdateVisibility();
    }

    private IEnumerator TypeWriter() {
        //Split
        string final_text = "";
        char[] c = Text_Input[TextIndex].ToCharArray();
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
            if(!Skip) { //Has not been skipped
                final_text += _c;
                UI_Text.text = final_text;
            } else { //Has been skipped
                final_text = Text_Input[TextIndex];
                UI_Text.text = final_text;
                Skip = false;
                break;
            }
            IsSkippable = true;
        }
        print("done");
        //Is Done
        IsSkippable = false;
        DoneBuildingText = true;

        //Make Indicator Appear
        Indicator.SetActive(true);
    }

}
