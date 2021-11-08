using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_DialogueScript : TriggerScript {

    public CharacterProfile_Scriptable Profile;

    public string[] PlaceHolderDialogue;

    protected override void OnActive() {
        base.OnActive();
        TextHandler.handler.SetText(PlaceHolderDialogue, TextHandler.TextModes.Selection);
    }
}
