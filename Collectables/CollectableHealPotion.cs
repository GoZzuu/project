using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHealPotion : CollectableItem {

    public float HealPower = 30;
   
    public override void CollectItem(Collider other)
    {
        base.CollectItem(other);
        other.GetComponent<CharacterStats>().Heal(HealPower);
       
    }
}
