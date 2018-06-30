using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlastCaster : AbilityCaster
{
    BlastAbility ability;
    Collider[] attackTo = new Collider[15];

    public void Initial(CharacterStats caster, BlastAbility ability)
    {
        base.Init(caster, ability);

        this.ability = ability;
    }


    public override IEnumerator CastingSkill()
    {
        //casting

        //set animation

        yield return new WaitForSeconds(ability.CastTime);
        BaseSkillFX.Play();

        int colCount = Physics.OverlapSphereNonAlloc(transform.position, ability.Radius, attackTo, caster.enemiesMask);

        if (colCount > 0)
            for (int i = 0; i < colCount; i++)
            {
                var h = attackTo[i].GetComponent<CharacterStats>();
                if (h)
                {
                    
                    h.GetDamage(ability.MaxDamage, true);
                }
            }



        //state = UnitStates.ATTACKING;
    }
}
