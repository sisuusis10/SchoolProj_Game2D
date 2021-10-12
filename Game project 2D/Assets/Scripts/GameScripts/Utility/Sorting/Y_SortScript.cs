using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Y_SortScript : MonoBehaviour {

    //Variables
    public bool Active;
    
    //Parent
    private Transform Parent;

    //Z positions
    private float Z_Position, Current_Z, Z_Offset = 0.1f;

    private void Awake() {
        //Get parent
        Parent = this.transform.parent;
        
        //Set positions
        Current_Z = Parent.position.z;
        Z_Position = Parent.position.z - 1;
    }

    private void Update() {
        if(Active) {
            //Sort Behind
            if (Current_Z != Z_Position + Z_Offset) {
                Current_Z = Z_Position + Z_Offset;
                UpdateSort();
            }
        } else { 
            //Sort Infornt
            if (Current_Z != Z_Position - Z_Offset) {
                Current_Z = Z_Position - Z_Offset;
                UpdateSort();
            }
        }
    }

    private void UpdateSort() {
        Vector3 newpos = new Vector3(Parent.position.x, Parent.position.y, Current_Z);
        Parent.position = newpos;
    }

    //Collider
    private void OnTriggerEnter2D(Collider2D collision) {
        Z_Position = collision.transform.position.z;
        Active = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Active = false;
    }

}
