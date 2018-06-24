using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class EnemyAIbase : CharacterStats {

    public delegate void BattleEvents(EnemyAIbase enemy);
    public static event BattleEvents EnemyCreatedEventHandler;
    public static event BattleEvents EnemyLookAtPlayerEventHandler;
    public static event BattleEvents EnemyDiedEventHandler;

    public float Damage = 5;
    public float HealthMaxDelta = 10;

    public GameObject damageViewer;

    protected readonly Collider[] allies = new Collider[3];
    protected NavMeshAgent agent;  
    private ComboAttacks[] attacksBehaviors;

    
    public bool FollowPlayer => target != null;
    protected bool isAttackStarted = false;

    protected Transform target;
    public float attackSpeed = 1f;
    public float alliesDetectionRange = 5;

    protected float UnitSize = 1;

   

    public Attack[] attacks;
    public GameObject targetView;

    public enum UnitStates
    {
        UNACTIVE,
        WAIT,      
        MOVING,
        TELEPORTATION,
        ATTACKING,
        SKILLUSE
    }
    public UnitStates state = UnitStates.UNACTIVE;
    
    protected void SeeEnemy()
    {
        EnemyLookAtPlayerEventHandler(this);
    }
    protected override void Start () {
        base.Start();

        currentHealth = health = health + Random.Range(-HealthMaxDelta, HealthMaxDelta);

        Instantiate(damageViewer, transform);

        agent = GetComponent<NavMeshAgent>();
        unitCollider = GetComponent<CapsuleCollider>();
        UnitSize = 0.5f + transform.lossyScale.x;

        animator.SetFloat("attackSpeed", attackSpeed);

        for (int i = 0; i < attacks.Length; i++)
            attacks[i].Initialize(enemiesMask, transform);

        EnemyCreatedEventHandler(this);
    }
    private void OnEnable()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
            attacksBehaviors = animator.GetBehaviours<ComboAttacks>();
        }
        for (int i = 0; i < attacksBehaviors.Length; i++)
        {
            attacksBehaviors[i].AttackStartedHandler += AttackStarted;
            attacksBehaviors[i].AttackEndedHandler += AttackEnded;
        }
        CharacterDamagedEventHandler += TakeDamage;
        CharacterDieEventHandler += IsDie;
    }
    private void OnDisable()
    {
        for (int i = 0; i < attacksBehaviors.Length; i++)
        {
            attacksBehaviors[i].AttackStartedHandler -= AttackStarted;
            attacksBehaviors[i].AttackEndedHandler -= AttackEnded;
        }
        CharacterDamagedEventHandler -= TakeDamage;
        CharacterDieEventHandler -= IsDie;
    }
    
    public void IsDie()
    {        
        agent.enabled = false;
        GetComponent<Collider>().enabled = false;

        EnemyDiedEventHandler(this);
        Destroy(gameObject, 5);
    }
    
    public void TakeDamage(int damage)
    {
        isAttackStarted = false;
        
        if (!FollowPlayer)
        {
            int targetsCount = Physics.OverlapSphereNonAlloc(transform.position, alliesDetectionRange * 3, allies, enemiesMask);

            if (targetsCount > 0)
                target = allies[0].transform;

            state = UnitStates.WAIT;
        }
    }
    
    void AttackStarted(int ID)
    {
        if (state != UnitStates.ATTACKING)
            return;

        Debug.Log("Enemy attack");

        attacks[0].DetectUnits(Damage * 0.5f + Random.value * Damage);




    }
    void AttackEnded(int ID)
    {
        isAttackStarted = false;
    }

    public void Targeted()
    {
        //if (state != UnitStates.TELEPORTATION)
            //targetView.SetActive(!targetView.activeSelf);
    }   
    
}
