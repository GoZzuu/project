using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : ScriptableObject {

    protected AbilityCaster abilityRuner;
    public GameObject baseAbilityFX;

    public abstract AbilityCaster Initialize(CharacterStats caster);

    public abstract void Cast();
    public abstract void Cast(Vector3 onPoint);
    public abstract void Cast(CharacterStats onTarget);
}
