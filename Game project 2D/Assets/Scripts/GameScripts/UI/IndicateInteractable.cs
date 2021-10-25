using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IndicateInteractable : MonoBehaviour {
    //Variables
    public static IndicateInteractable set;

    //Components
    private TextMeshProUGUI text;

    public bool IsVisible = false;
    private float Alpha, targetalpha = 0f;
    private Color textColor = new Color(1f, 1f, 1, 0f);

    private Vector2 pos;

    private void Awake() {
        set = this;
        text = this.GetComponent<TextMeshProUGUI>();
        pos = text.transform.position;
    }

    private void FixedUpdate() {
        switch(IsVisible) {
            case true:
                targetalpha = 1f;        
                break;
            case false:
                targetalpha = 0f;
                break;
        }
        //Lerp
        Alpha = Mathf.Lerp(Alpha, targetalpha, 0.1f);
        textColor.a = Alpha;
        //set
        text.color = textColor;
        //Lerp Pos
        Vector2 temp_pos = this.transform.position;
        float Y_offset = pos.y + (5 * (Alpha * 10));
        temp_pos.y = Y_offset;
        this.transform.position = temp_pos;
    }

}
