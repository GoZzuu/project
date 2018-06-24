using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityCaster : MonoBehaviour
{

    protected ParticleSystem BaseSkillFX;

    public CharacterStats Caster
    {
        get
        {
            return caster;
        }
    }

    protected CharacterStats caster;

    public virtual void CastSequince()
    {
        //var ability = CreateInstance<TeleportationAbility>();
        StartCoroutine(CastingSkill());
    }

    public virtual void CastSequince(Vector3 onPoint) { }

    public virtual void CastSequince(CharacterStats onTarget) { }

    public virtual void Init(CharacterStats caster, Ability ability)
    {
        this.caster = caster;
        BaseSkillFX = Instantiate(ability.baseAbilityFX, transform).GetComponent<ParticleSystem>();
    }

    public virtual IEnumerator CastingSkill()
    {
        yield return null;
    }
}
