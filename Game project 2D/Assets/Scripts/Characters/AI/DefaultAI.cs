using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultAI : MonoBehaviour {

    //Variables
    private Vector2 y_states;
    private float Y_pos;
    private bool ismoving;

    private bool Direction;
    private float timer;


    private void Start() {
        y_states = new Vector2(this.transform.position.y + 10, this.transform.position.y);
    }

    //FixedUpdate
    private void FixedUpdate() {
        if(timer <= 0f) {
            Y_pos = (Direction) ? y_states.x : y_states.y;
            timer = 1f;
        } else {
            timer -= Time.fixedDeltaTime;
        }

    }

}
