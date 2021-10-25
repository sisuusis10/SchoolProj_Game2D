using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TransitionHandler : MonoBehaviour {

    //Variables
    public static TransitionHandler transitions;

    //Components
    public Image BlackOverlay;

    //Transition
    [SerializeField]
    private float Current_Alpha = 1f;
    private Color Overlay_Color = new Color(0f, 0f, 0f, 1f);
    
    //Target
    [SerializeField]
    private float TargetAlpha;
    private float LerpSpeed;

    //Transition Types
    public enum Transitions { None, Fade, Fade_ZoomEffect, Circle }
    public enum Directions { Transition_In, Transition_Out, Both_Auto}

    //Current Transition
    public Transitions CurrentTransition = Transitions.None;
    private bool Auto = false;

    private float Zoom_Start, Zoom;

    private void Awake() {
        transitions = this;
        Zoom_Start = Camera.main.orthographicSize;
        Zoom = Zoom_Start;
        //Hide
        SetTransition(Transitions.Fade, Directions.Transition_Out);
    }

    public void SetTransition(Transitions trans, Directions direction, float speed = 0.01f) {
        CurrentTransition = trans;
        LerpSpeed = speed;

        switch (direction) {
            case Directions.Transition_In:
                TargetAlpha = 1f;
                break;
            case Directions.Transition_Out:
                TargetAlpha = 0f;
                break;
            case Directions.Both_Auto:
                TargetAlpha = 1f;
                Auto = true;
                break;
        }
    }

    private void FixedUpdate() {
        //Transition
        if(IsTransitioning()) {
            switch(CurrentTransition) {
                case Transitions.Fade:
                    //apply
                    Overlay_Color.a = Current_Alpha;
                    BlackOverlay.color = Overlay_Color;
                    break;
                case Transitions.Fade_ZoomEffect:
                    Zoom = Zoom_Start + ((Zoom * 0.2f) * Current_Alpha);
                    //apply
                    Overlay_Color.a = Current_Alpha;
                    BlackOverlay.color = Overlay_Color;
                    Camera.main.orthographicSize = Zoom;
                    break;
            }
            
        }
    }

    public bool IsTransitioning() {
        if(Current_Alpha != TargetAlpha || Auto) {
            //Lerp
            Current_Alpha = Mathf.MoveTowards(Current_Alpha, TargetAlpha, LerpSpeed);
            //Check if auto
            if(Auto && Current_Alpha == TargetAlpha) {
                TargetAlpha = 0f;
                Auto = false;
            }
            return true;
        }

        CurrentTransition = Transitions.None;
        return false;
    }

}
