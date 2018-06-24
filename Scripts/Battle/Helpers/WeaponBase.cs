using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType
{
    MELEE,
    RANGED
}

[CreateAssetMenu(menuName = "Weapon/Melee")]
public class WeaponBase : ScriptableObject {

    public float MaxDamage = 10;
    public float MinDamage = 5;

    public WeaponType WType { get; } = WeaponType.MELEE;

    public GameObject rightHandWeaponModel;
    public GameObject leftHandWeaponModel;

    public Attack[] WeaponAttacks;
    public string AttackTriggerName = "Attack";
    public string AttackingBool = "";

    public AudioClip[] EquipSounds;
    public AudioClip[] StartAttackSounds;
    public AudioClip[] HitSounds;

    GameObject rightHandModel;
    GameObject leftHandModel;

    protected CharacterSounds audioController;

    public static WeaponBase CreateWeapon(float minDamage, float maxDamage, Attack[] attacks, string attackTriggerName)
    {
        var weapon = CreateInstance<WeaponBase>();
        weapon.MaxDamage = maxDamage;
        weapon.MinDamage = minDamage;
        weapon.WeaponAttacks = attacks;
        weapon.AttackTriggerName = attackTriggerName;
        return weapon;
    }

    public virtual void Initialize(CharacterStats holder, LayerMask attackTo, Transform leftHand = null, Transform rightHand = null)
    {
        // AUDIO ???
        audioController = holder.GetComponent<CharacterSounds>();

        // instatiate models
        if (rightHandWeaponModel && rightHand)
        {
            rightHandModel = Instantiate(rightHandWeaponModel, rightHand);
            rightHandModel.SetActive(false);
        }
        if (leftHandWeaponModel && leftHand)
        {
            leftHandModel = Instantiate(leftHandWeaponModel, leftHand);
            leftHandModel.SetActive(false);
        }

        for (int i = 0; i < WeaponAttacks.Length; i++)
            WeaponAttacks[i].Initialize(attackTo, holder.transform);
    }

    public virtual void Equip(PlayerWeaponAndAttacks weapon)
    {
        //TO DO:   Set Models
        weapon.equipedWeapon = this;
        weapon.EquipedWeaponName = name;

        if (EquipSounds != null)
            audioController.PlaySound(EquipSounds);

        if (rightHandModel)
            rightHandModel.SetActive(true);
        if (leftHandModel)
            leftHandModel.SetActive(true);
        //player.GetComponent<Animator>().runtimeAnimatorController = weaponAC;
        //weapon.eqipedWeapon = weaponAttacks;
    }
    public virtual void Unequip()
    {
        if (rightHandModel)
            rightHandModel.SetActive(false);
        if (leftHandModel)
            leftHandModel.SetActive(false);
    }

    public virtual void StartAttack()
    {
        if(StartAttackSounds != null)
        audioController.PlaySound(StartAttackSounds);
    }

    public virtual void Attack(int attackId)
    {
        if (HitSounds != null)
            audioController.PlaySound(HitSounds);

        WeaponAttacks[attackId].DetectUnits(Random.Range(MinDamage, MaxDamage));
    }
}
