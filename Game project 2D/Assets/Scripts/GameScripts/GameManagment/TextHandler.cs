using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TextHandler : MonoBehaviour {

    //Variables
    public static TextHandler handler;
    public bool IsVisible = false, DoneBuildingText = false;
    private bool Initiate_Interaction = false, Interaction = false;

    //mode
    public enum TextModes { InActive, Default, Character, Selection, Quit }
    public TextModes TextMode;
    //Text
    private string[] Text_Input;
    private string[] LastText_Input;

    private int TextIndex = 0;

    public TextMeshProUGUI UI_Text;
    public GameObject Indicator;

    //Components
    public Image Box_Outter, Box_Inner;
    private PlayerController _player;
    private AudioSource _audiosource;
    //sfx probably place holder
    public AudioClip defaultsound;
    //Quick array
    public static string[] QuickArray(string _string) {
        string[] StringArray = new string[1] { _string };
        return StringArray;
    }
    private void Awake() {
        handler = this;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        _audiosource = this.GetComponent<AudioSource>();
        SetText(QuickArray(""), TextModes.Quit); //Hide on start
    }

    private void Update() {
        TextModeManagement();
    }

    //Modes
    private int PlayerSelected_Index = 0, PlayerSelect_MemIndex;
    private bool MovSelect = false;
    private int initiate_selectmode = 1;
    private int Quit_CoolDown, Quit_CoolDownMax = 5;

    private void TextModeManagement() {
        if(Quit_CoolDown > 0) {
            Quit_CoolDown--;
        } else {
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
                    if (Initiate_Interaction) { //Skip
                        Interaction = true;
                    } else if (!Initiate_Interaction && DoneBuildingText && IsVisible) { //Next Action
                        SetText(QuickArray(""), TextModes.Quit); //Hide TextBox
                    }
                }
                break;

                //Character / NPC Text
            case TextModes.Character:
                //Code
                break;

                //Player Text Option Selection
            case TextModes.Selection:
                //Generate
                if (PlayerSelect_MemIndex != PlayerSelected_Index || !Initiate_Interaction) {
                        UI_Text.alignment = TextAlignmentOptions.Center;
                        UI_Text.wordSpacing = 70;
                    string DisplayText = "";
                    for (int i = 0; i < Text_Input.Length; i++) {
                        if (i != PlayerSelected_Index) {
                            DisplayText += Text_Input[i] + " ";
                        } else {
                            DisplayText += "<s>" + Text_Input[i] + "</s>" + " ";
                        }
                    }
                    //Initiation
                    if (!Initiate_Interaction) {
                        Initiate_Interaction = true;
                        _player.IsLocked = true;
                    }
                    initiate_selectmode = 0;
                    //Apply
                    UI_Text.text = DisplayText;
                }

                //Set mem index
                PlayerSelect_MemIndex = PlayerSelected_Index;
                //Player Input
                if(Input.GetAxisRaw("Horizontal") > 0f) {
                    if(!MovSelect)
                        PlayerSelected_Index++;
                    MovSelect = true;
                } else if (Input.GetAxisRaw("Horizontal") < 0f) {
                    if (!MovSelect)
                        PlayerSelected_Index--;
                        MovSelect = true;
                } else {
                    MovSelect = false;
                }
                //Player Interaction
                if(Input.GetKeyDown(KeyCode.E) && !DoneBuildingText) {
                        SetText(QuickArray(""), TextModes.Default);
                        Interaction = true;
                        _player.IsLocked = false;
                }
                //Clamp
                if(PlayerSelected_Index > Text_Input.Length-1) {
                    PlayerSelected_Index = 0;
                } else if(PlayerSelected_Index < 0) {
                    PlayerSelected_Index = Text_Input.Length-1;
                }
                break;
        }
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
                TextMode = TextModes.Default;
                Quit_CoolDown = Quit_CoolDownMax;
                UI_Text.alignment = TextAlignmentOptions.TopLeft;
                UI_Text.wordSpacing = 0;
                DoneBuildingText = false;
                return;
        }
    }

    
    public void SetText(string[] text, TextModes _mode = TextModes.InActive) {
        if(Quit_CoolDown <= 0) {
        //Return mode
        if(_mode != TextModes.InActive) {
            TextMode = _mode;
        }
        if(text[0] == "") { //if Input is empty, stop coroutine & hide TextBox
            UI_Text.text = text[0];
            StopAllCoroutines();
            IsVisible = false;
            Initiate_Interaction = false;
        } else if(LastText_Input != Text_Input && TextMode != TextModes.Quit) { //if Input isnt empty, TextBox start coroutin and make visible
            initiate_selectmode = 1;
            StopAllCoroutines(); //Make sure no coroutines are active
            Text_Input = text;
            LastText_Input = Text_Input;

            //check if the text mode is default
            if (TextMode == TextModes.Default) {
                StartCoroutine(TypeWriter()); //Start Type writer effect
            }
            IsVisible = true;
            Initiate_Interaction = false;
        }
        //Update Visibility
        UpdateVisibility();
        }
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
            _audiosource.clip = defaultsound;
            _audiosource.Play();

            //Build and Display text
            if(!Interaction) { //Has not been skipped
                final_text += _c;
                UI_Text.text = final_text;
            } else { //Has been skipped
                final_text = Text_Input[TextIndex];
                UI_Text.text = final_text;
                Interaction = false;
                break;
            }
            Initiate_Interaction = true;
        }
        print("done");
        //Is Done
        Initiate_Interaction = false;
        DoneBuildingText = true;

        //Make Indicator Appear
        Indicator.SetActive(true);
    }

}
