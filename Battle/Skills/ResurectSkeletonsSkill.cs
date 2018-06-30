using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResurectSkeletonsSkill : Skill {

    public override void Initialize(Transform point, LayerMask attackTo)
    {
       
    }

    public override void Execute()
    {
        Debug.Log("Revive skeletons");
    }
}
