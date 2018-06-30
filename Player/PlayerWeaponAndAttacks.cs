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

    public bool Attacking { get; private set; }
    public bool IsBlocking = false;
    bool attackButtonPressed = false;

    public PlayerMovement movement;
    public CharacterStats playerStats;

    ComboAttacks[] attacksBehaviors;

    public Transform rightHandWeaponPivot;
    public Transform leftHandWeaponPivot;


    public Transform Target;
    EnemyAIbase TargetEnemy = null;
    public bool Targeted
    {
        get
        {
            return Target != null;   // && !targetHealth.Died && Vector3.SqrMagnitude(Target.position - transform.position) < 16;
        }
    }
    List<EnemyAIbase> enemyesInBattle = new List<EnemyAIbase>();

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

        BattleManager.EnemyInBattleEventHandler += InBattle;
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

        BattleManager.EnemyInBattleEventHandler -= InBattle;
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

            if (!Attacking)
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
                Attacking = bowAttacking;
            } 

            if(Attacking && CrossPlatformInputManager.GetButtonUp("Attack"))
            {
                animator.SetBool(equipedWeapon.AttackingBool, false);
            }
        }


        Targeting();
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
    void InBattle(EnemyAIbase enemy)
    {
        enemyesInBattle.Add(enemy);
    }

    void Targeting()
    {
        

        for (int i = 0; i < enemyesInBattle.Count; i++)
        {
            if (TargetEnemy == null)
            {
                TargetEnemy = enemyesInBattle[i];
            }
            else if (enemyesInBattle[i] != null)
            {
                bool distanceCheck = Vector3.SqrMagnitude(TargetEnemy.transform.position - transform.position)
                    > Vector3.SqrMagnitude(enemyesInBattle[i].transform.position - transform.position);

                bool angleCheck = Vector3.Angle(transform.forward, TargetEnemy.transform.position - transform.position)
                    > Vector3.Angle(transform.forward, enemyesInBattle[i].transform.position - transform.position);

                Debug.Log(distanceCheck + "   " + angleCheck);

                if (distanceCheck || angleCheck)
                    TargetEnemy = enemyesInBattle[i];
            }
        }

        if (TargetEnemy != null)
        {
            if (TargetEnemy.Died || TargetEnemy.state == EnemyAIbase.UnitStates.UNACTIVE)
            {
                enemyesInBattle.Remove(TargetEnemy);
                TargetEnemy = null;
                Target = null;
                
            }
            else
            {
                Debug.Log("Targeted");
                Target = TargetEnemy.transform;
            }
        }
        else
        {
            Target = null;
        }

    }


    private void Damaged(int damage)
    {
        //IsAttacking = false;
       // animator.SetBool(equipedWeapon.AttackingBool, false);
    }

    void AttackPower()
    {
        //     -------    ПЕРВАЯ, ВТОРАЯ И ТРЕТЬЯ АТАКА      -------
        animator.SetTrigger("attack_power");

    }
    void AttackFast()
    {
        //Debug.Log(currentAttackID);
        if (!Attacking)
        {          
            Attacking = true;
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
            Attacking = false;

        if (attackID == -1)
        {
            movement.TargetedWalk = false;
            Attacking = false;
        }
    }

    public int GetWeaponsCount()
    {
        return weapons.Count;
    }

   

   

    public bool CurrentWeaponRanged()
    {
        return equipedWeapon.GetType() == typeof(WeaponRanged);
    }
}
