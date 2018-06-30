using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Teleportation")]
public class TeleportationAbility : Ability {

    public float teleportationSpeed = 1;
    //public GameObject TeleportationFX;

    //private TeleportationCaster abilityRuner;


    public override void Cast()
    {
        abilityRuner.CastSequince();
    }

    public override void Cast(Vector3 onPoint)
    {
        
    }

    public override void Cast(CharacterStats onTarget)
    {
        
    }

    public override AbilityCaster Initialize(CharacterStats caster)
    {
        var abilityR = caster.gameObject.AddComponent<TeleportationCaster>();
        abilityR.Initial(caster, this);
        abilityRuner = abilityR;
        return abilityRuner;
    }
}
