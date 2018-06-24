using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Skill  {

    public virtual void Initialize(Transform point, LayerMask attackTo) { }

    public virtual void Execute() { }
   
}
