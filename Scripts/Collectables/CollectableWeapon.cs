using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableWeapon : CollectableItem {

    public WeaponBase weapon;

    public override void CollectItem(Collider other)
    {
        base.CollectItem(other);

        UIManager.instance.SetText(weapon.name + " collected!");

        other.GetComponent<PlayerWeaponAndAttacks>().AddWeapon(weapon);
    }
}
