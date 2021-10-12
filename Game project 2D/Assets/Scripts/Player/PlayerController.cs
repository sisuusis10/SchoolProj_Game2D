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
    private float Speed, Walk_Speed = 140f, Run_Speed = 200f, Speed_Lerp = 0.1f;

    [SerializeField]
    private Vector2 PlayerDirection;

    //Raycast Detection
    private List<RaycastHit2D> hit_Results = new List<RaycastHit2D>();
    
    public enum MovementStates { Idle, Walking, Running };
    public MovementStates MovState;

    // Start is called before the first frame update
    void Start() {
        _Rigidbody = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if(!IsLocked) {
            MovementCode();
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
            MovState = (Input.GetKey(KeyCode.LeftShift) ? MovementStates.Running : MovementStates.Walking);
        }
    }

    //
    public void ObjectDetection() {
        //Physics2D.Raycast(this.transform.position, PlayerDirection, out hit, 10f);
        TriggerScript trigger = null; //= hit.collider.GetComponent<TriggerScript>();
        print("sad " + trigger);

        //Does Object have trigger script?
        if (trigger && Input.GetKey(KeyCode.E)) {
            trigger.SetActive();
            print("asdasdasd");
        }
    }

}
