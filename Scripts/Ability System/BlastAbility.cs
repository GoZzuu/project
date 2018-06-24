using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/Blast")]
public class BlastAbility : Ability
{
    public float MinDamage = 5;
    public float MaxDamage = 10;

    public float Radius = 3f;

    public float CastTime = 0.5f;

    public override AbilityCaster Initialize(CharacterStats caster)
    {
        var abilityR = caster.gameObject.AddComponent<BlastCaster>();
        abilityR.Initial(caster, this);
        abilityRuner = abilityR;
        return abilityRuner;
    }

    public override void Cast()
    {
        abilityRuner.CastSequince();
    }

    public override void Cast(Vector3 onPoint)
    {
        throw new System.NotImplementedException();
    }

    public override void Cast(CharacterStats onTarget)
    {
        throw new System.NotImplementedException();
    }

    
}
