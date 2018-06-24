using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Weapon/Ranged")]
public class WeaponRanged : WeaponBase {

    WeaponType weaponType = WeaponType.RANGED;

    public GameObject projectilePrefab;

    ProjectilePool projectilesPool;

    public override void Initialize(CharacterStats holder, LayerMask attackTo, Transform leftHand = null, Transform rightHand = null)
    {
        base.Initialize(holder, attackTo, leftHand, rightHand);
        
        projectilesPool = ProjectilePool.CreatePool(holder.transform, projectilePrefab, this);
        Debug.Log("Weapon Initialized ranged " + projectilesPool.name);
    }

    public override void Attack(int attackId)
    {
        // sound here
        if (HitSounds != null)
            audioController.PlaySound(HitSounds);

        projectilesPool.PoolObject();
    }
}
