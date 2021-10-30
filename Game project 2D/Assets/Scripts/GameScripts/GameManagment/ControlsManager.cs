using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsManager : MonoBehaviour {

    public KeyCode InteractKey = KeyCode.E, SprintKey = KeyCode.LeftShift, PauseKey = KeyCode.Escape;

    public enum InputTypes { Interact, Sprint, Pause };

    public bool InputState(InputTypes _input, bool _continuous) {
        KeyCode tempkey = new KeyCode();
        switch (_input) { //Get input
            case InputTypes.Interact:
                tempkey = InteractKey;
                break;
            case InputTypes.Sprint:
                tempkey = SprintKey;
                break;
            case InputTypes.Pause:
                tempkey = PauseKey;
                break;
        }
        //Get value
        return StateValue(tempkey, _continuous);
    }

    private bool StateValue(KeyCode _key, bool continuous) {
        if(continuous && Input.GetKey(_key)) {
            return true;
        } else if(Input.GetKeyDown(_key)) {
            return true;
        }
        return false;
    }
}
