using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerWeaponAndAttacks : MonoBehaviour {

    Animator animator;

    public float AttackAnimationSpeed = 1.15f;

    public string EquipedWeaponName;
    [HideInInspector]
    public WeaponBase equipedWeapon;
    public List<WeaponBase> weapons = new List<WeaponBase>();
    
    public int currentWeaponID = 0;
    public int currentAttackID = 0;

    public bool IsAttacking;
    public bool IsBlocking = false;
    bool attackButtonPressed = false;

    public PlayerMovement movement;
    public CharacterStats playerStats;

    ComboAttacks[] attacksBehaviors;
    
    bool isDie = false;

    public Transform rightHandWeaponPivot;
    public Transform leftHandWeaponPivot;




    private void OnEnable()
    {
        if (!animator || !playerStats)
        {
            playerStats = GetComponent<CharacterStats>();
            animator = GetComponent<Animator>();
            animator.SetFloat("AttackSpeed", AttackAnimationSpeed);

            attacksBehaviors = animator.GetBehaviours<ComboAttacks>();
        }
        for (int i = 0; i < attacksBehaviors.Length; i++)
        {
            attacksBehaviors[i].AttackStartedHandler += AttackStarted;
            attacksBehaviors[i].AttackEndedHandler += AttackEnded;
        }

        playerStats.CharacterDamagedEventHandler += Damaged;
        //playerStats.CharacterDieEventHandler += Damaged;
    }
    private void OnDisable()
    {
        for (int i = 0; i < attacksBehaviors.Length; i++)
        {
            attacksBehaviors[i].AttackStartedHandler -= AttackStarted;
            attacksBehaviors[i].AttackEndedHandler -= AttackEnded;
        }
        playerStats.CharacterDamagedEventHandler -= Damaged;
    }

    private void Damaged(int damage)
    {
        //IsAttacking = false;
       // animator.SetBool(equipedWeapon.AttackingBool, false);
    }

    void Start () {
        //rightHandWeaponPivot = transform.FindChild("rightHandWeaponHandler");
        //leftHandWeaponPivot = transform.FindChi("leftHandWeaponHandler");
        movement = GetComponent<PlayerMovement>();
        playerStats = GetComponent<PlayerStats>();


        foreach (var weapon in weapons)    
            weapon.Initialize(playerStats, playerStats.enemiesMask, leftHandWeaponPivot, rightHandWeaponPivot);

        weapons[currentWeaponID].Equip(this);              
    }
    private void FixedUpdate()
    {

        if (CrossPlatformInputManager.GetButtonUp("SwitchWeapon"))
            SwitchWeapon();

       

        if (!CurrentWeaponRanged())
        {
            //player.TargetingRange = 3f;
            if (CrossPlatformInputManager.GetButtonUp("Attack"))//|| Input.GetButtonDown("Fire1") )
                AttackFast();

            if (!IsAttacking)
            {
                IsBlocking = CrossPlatformInputManager.GetButton("Defence");
                movement.TargetedWalk = IsBlocking;
                animator.SetBool("IsBlocking", IsBlocking);
            }
        }
        else
        {
            //player.TargetingRange = 10f;
            bool bowAttacking = CrossPlatformInputManager.GetButton("Attack");

            if (bowAttacking)
            {
                equipedWeapon.StartAttack();
                animator.SetBool(equipedWeapon.AttackingBool, bowAttacking);

                movement.TargetedWalk = bowAttacking;
                IsAttacking = bowAttacking;
            } 

            if(IsAttacking && CrossPlatformInputManager.GetButtonUp("Attack"))
            {
                animator.SetBool(equipedWeapon.AttackingBool, false);
            }
        }
    }

    public void SwitchWeapon()
    {
        if (weapons.Count == 1)
            return;

        weapons[currentWeaponID].Unequip();

        currentWeaponID++;

        if (currentWeaponID == weapons.Count)
            currentWeaponID = 0;

        Debug.Log("Switching weapon");

        weapons[currentWeaponID].Equip(this);
    }

    void AttackPower()
    {
        //     -------    ПЕРВАЯ, ВТОРАЯ И ТРЕТЬЯ АТАКА      -------
        animator.SetTrigger("attack_power");

    }
    void AttackFast()
    {
        //Debug.Log(currentAttackID);
        if (!IsAttacking)
        {          
            IsAttacking = true;
            animator.SetTrigger(equipedWeapon.WeaponAttacks[currentAttackID].triggerName);
            equipedWeapon.StartAttack();
        }
        else
            attackButtonPressed = true;
    }

    void AttackStarted(int attackID)
    {      
        if (attackID != -1)
        {        
            //playerStats.Attacking();

            currentAttackID = attackID;
            equipedWeapon.Attack(attackID); 

            if (attackButtonPressed)
            {
                currentAttackID++;

                if (currentAttackID >= equipedWeapon.WeaponAttacks.Length)
                    currentAttackID = 0;

                //Debug.Log(currentAttackID);
                animator.SetTrigger(equipedWeapon.AttackTriggerName);
                attackButtonPressed = false;
                
            }
        }
        else
        {
           
            equipedWeapon.Attack(attackID);         
        }
    }
    void AttackEnded(int attackID)
    {
        if (currentAttackID == attackID)
            IsAttacking = false;

        if (attackID == -1)
        {
            movement.TargetedWalk = false;
            IsAttacking = false;
        }
    }

    public int GetWeaponsCount()
    {
        return weapons.Count;
    }

    public void AddWeapon(WeaponBase weapon)
    {
        weapon.Initialize(playerStats, playerStats.enemiesMask, leftHandWeaponPivot, rightHandWeaponPivot);
        weapons.Add(weapon);       
    }  

    public bool CurrentWeaponRanged()
    {
        return equipedWeapon.GetType() == typeof(WeaponRanged);
    }
}
