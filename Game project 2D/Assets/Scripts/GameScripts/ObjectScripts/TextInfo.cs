using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextInfo : TriggerScript {

    public string[] DisplayText;

    protected override void OnActive() {
        base.OnActive();

        TextHandler.handler.SetText(DisplayText, TextHandler.TextModes.Default);
    }

}
