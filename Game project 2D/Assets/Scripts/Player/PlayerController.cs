using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //Variables
    public bool IsLocked = false;

    //Components
    private Rigidbody2D _Rigidbody;

    //Movement
    [SerializeField]
    private Vector3 Velocity;
    private float Speed, Walk_Speed = 140f, Run_Speed = 200f, Speed_Lerp = 0.5f;

    //States
    public enum MovementStates { Idle, Walking, Running };
    public MovementStates MovState;

    //Player Combat Data
    public int Health = 100;
    public int Health_Max = 100;
    
    //Direction
    [SerializeField]
    private Vector2 PlayerDirection;

    //Detection System
    public DetectionBoxScript DetectionBox;


    // Start is called before the first frame update
    private void Start() {
        //Initialization


        //Get Components
        _Rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate() {
        if(!IsLocked) {
            MovementCode();
            ObjectDetection();
        } else {
            _Rigidbody.velocity = Vector2.zero;
        }
    }
    private void Update() {
        if (!IsLocked) {
            ObjectDetection();
        }
    }

    //Movement Management
    public void MovementCode() {
        //Movement States
        switch(MovState) {
            case MovementStates.Idle:
                Speed = Mathf.Lerp(Speed, 0f, Speed_Lerp);
                break;
            case MovementStates.Walking:
                Speed = Mathf.Lerp(Speed, Walk_Speed, Speed_Lerp);
                break;
            case MovementStates.Running:
                Speed = Mathf.Lerp(Speed, Run_Speed, Speed_Lerp);
                break;
        }

        //Set movement
        Vector2 _input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Velocity = _input.normalized * Speed * Time.fixedDeltaTime;
        _Rigidbody.velocity = Velocity;

        //Find Forward Direction
        if(Input.GetAxisRaw("Vertical") < 0f) {
            PlayerDirection = Vector2.down;
        } else if(Input.GetAxisRaw("Vertical") > 0f) {
            PlayerDirection = Vector2.up;
        } else if (Input.GetAxisRaw("Horizontal") > 0f) {
            PlayerDirection = Vector2.right;
        } else if (Input.GetAxisRaw("Horizontal") < 0f) {
            PlayerDirection = Vector2.left;
        }

        //Set mov state
        if (_input.magnitude == 0f) {
            MovState = MovementStates.Idle;
        } else {
            MovState = (GameManagerScript.controls.InputState(ControlsManager.InputTypes.Sprint, true) ? MovementStates.Running : MovementStates.Walking);
        }
    }

    //Object detection
    public void ObjectDetection() {
        //Set position to player direction
        Vector3 BoxPosition = this.transform.position + new Vector3(PlayerDirection.x, PlayerDirection.y, 0f);
        DetectionBox.transform.position = BoxPosition;

        if (GameManagerScript.controls.InputState(ControlsManager.InputTypes.Interact, false)) {
            DetectionBox.ActivateTrigger();
        }
    }

}
