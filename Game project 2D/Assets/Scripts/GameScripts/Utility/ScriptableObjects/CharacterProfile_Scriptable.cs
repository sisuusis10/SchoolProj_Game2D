using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Character Profile", menuName = "ScriptableObjects/Character profile")]
public class CharacterProfile_Scriptable : ScriptableObject {
    //Variables
    public string CharacterName;

    public int Health;

    public float Strenght;

}
