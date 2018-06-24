using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DeadWaveSkill : Skill
{
    public float Range = 2f;
    public float Damage = 20;

    public GameObject SkillEffect;

    Transform transform;
    LayerMask attacksLayer;
    Collider[] attackTo = new Collider[5];

    public override void Initialize(Transform transform, LayerMask attackTo)
    {
        this.transform = transform;
        this.attacksLayer = attackTo;
    }

    public override void Execute()
    {
        int colCount = Physics.OverlapSphereNonAlloc(transform.position, Range, attackTo, attacksLayer);
        GameObject.Instantiate(SkillEffect, transform);

        if (colCount > 0)
            for (int i = 0; i < colCount; i++)
            {
                var h = attackTo[i].GetComponent<CharacterStats>();
                if (h)
                {
                    h.GetDamage(Damage, false,0.5f);
                }
            }
    } 
}
