using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatTrigger : TriggerScript {

    public CharacterProfile_Scriptable[] Profiles;

    public CombatManager.Gen_Styles Generate_Style;

    protected override void OnActive() {
        base.OnActive();
        CombatManager.combat.Men_Initiate(Generate_Style, Profiles);
    }
}
