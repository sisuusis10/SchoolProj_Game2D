using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionBoxScript : MonoBehaviour {

    private string CurrentObject_Name;
    public TriggerScript TriggerObject;

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.GetComponent<TriggerScript>()) {
            print(collision.gameObject.name);
            //Add Values
            CurrentObject_Name = collision.gameObject.name;
            TriggerObject = collision.gameObject.GetComponent<TriggerScript>();
            //Indicate
            IndicateInteractable.set.IsVisible = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if(CurrentObject_Name != "" && collision.gameObject.name == CurrentObject_Name) {
            print("Removed "+CurrentObject_Name);
            //Reset Values
            TriggerObject = null;
            CurrentObject_Name = "";
            //Indicate
            IndicateInteractable.set.IsVisible = false;
        }
    }

    public void ActivateTrigger() {
        if(TriggerObject != null) {
            TriggerObject.SetActive();
        } else {
            print("no trigger");
        }
    }

}
