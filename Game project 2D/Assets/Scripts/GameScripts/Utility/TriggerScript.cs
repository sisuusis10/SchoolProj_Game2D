using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerScript : MonoBehaviour {

    public void SetActive() {
        OnActive();
        print("boop");
    }
    protected virtual void OnActive() {

    }

}
