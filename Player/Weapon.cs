using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Weapon
{
    public string WeaponName = "Sword";
    public enum WeaponType
    {
        MELEE,
        RANGED
    }
    public WeaponType weaponType = WeaponType.MELEE;

    public GameObject rightHandWeaponPrefab;
    public GameObject leftHandWeaponPrefab;
    public Attack[] weaponAttacks;

    public GameObject bulletPrefab;
    /*
    public void Equip(PlayerWeaponAndAttacks weapon)
    {
        //TO DO:   Set Models
        weapon.equipedWeapon = this;
        weapon.EquipedWeaponName = WeaponName;
     
        if(rightHandWeaponPrefab)
        rightHandWeaponPrefab.SetActive(true);
        if (leftHandWeaponPrefab)
            leftHandWeaponPrefab.SetActive(true);
        //player.GetComponent<Animator>().runtimeAnimatorController = weaponAC;
        //weapon.eqipedWeapon = weaponAttacks;
    }
    public void Unequip()
    {
        if (rightHandWeaponPrefab)
            rightHandWeaponPrefab.SetActive(false);
        if (leftHandWeaponPrefab)
            leftHandWeaponPrefab.SetActive(false);
    }*/
}
