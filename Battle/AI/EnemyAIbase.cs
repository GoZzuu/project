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

    // all allies massive later
    Transform player;

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

    protected override void Start()
    {
        base.Start();

        currentHealth = health = health + Random.Range(-HealthMaxDelta, HealthMaxDelta);

        agent = GetComponent<NavMeshAgent>();
        unitCollider = GetComponent<CapsuleCollider>();

        UnitSize = 0.5f + transform.lossyScale.x;
        alliesDetectionRange *= alliesDetectionRange;   // using sqr magnitude for distance

        animator.SetFloat("attackSpeed", attackSpeed);

        for (int i = 0; i < attacks.Length; i++)
            attacks[i].Initialize(enemiesMask, transform);

        EnemyCreatedEventHandler(this);

        //player = PlayerStats.instance.transform;
    }
    
    public override void Activate()
    {
        base.Activate();
        Instantiate(damageViewer, transform);
    }
    protected void SeeEnemy()
    {
        EnemyLookAtPlayerEventHandler(this);
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
        if (state != UnitStates.TELEPORTATION)
            targetView.SetActive(!targetView.activeSelf);
    }

    protected virtual void FixedUpdate()
    {
        if (Died)
            return;

        switch (state)
        {
            case UnitStates.UNACTIVE:

                // avaiting for player;
                if (Vector3.SqrMagnitude(PlayerStats.instance.transform.position - transform.position) < alliesDetectionRange)
                {
                    target = PlayerStats.instance.transform;
                    state = UnitStates.WAIT;

                    Activate();
                    SeeEnemy();

                }
                break;
                
        }
    }
}
