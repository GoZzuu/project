using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LichAI : EnemyAIbase
{
    public GameObject deadParticles;
    public GameObject TeleportationFXPrefab;
    ParticleSystem teleportationEffect;

    List <Renderer> _renderers = new List<Renderer>();

    public ObjectPool missulesPool;

    [SerializeField]
    public Skill[] skills;

    protected override void Start()
    {
        base.Start();
        
        foreach(var skill in skills)
        {
            skill.Initialize(transform, enemiesMask);
        }
    }

    void FixedUpdate()
    {

        int targetsCount;

        switch (state)
        {
            case UnitStates.UNACTIVE:
                targetsCount = Physics.OverlapSphereNonAlloc(transform.position, alliesDetectionRange, allies, enemiesMask);

                if (targetsCount > 0)
                {
                    target = allies[0].transform;
                    state = UnitStates.WAIT;

                    Activate();

                }
                break;
            case UnitStates.WAIT:

                if (target == null)
                {
                    targetsCount = Physics.OverlapSphereNonAlloc(transform.position, alliesDetectionRange, allies, enemiesMask);
                    if (targetsCount > 0)
                        target = allies[0].transform;

                }
                else
                {


                    state = UnitStates.ATTACKING;

                }


                break;
            case UnitStates.TELEPORTATION:

                break;
            case UnitStates.MOVING:

                if (Vector3.SqrMagnitude(target.position - transform.position) > 1 + UnitSize && !isAttackStarted)
                {

                    agent.SetDestination(target.position);
                    animator.SetBool("isMoving", true);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                    state = UnitStates.ATTACKING;
                }
                break;
            case UnitStates.ATTACKING:

                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), 0.3f * attackSpeed);


                if (Vector3.SqrMagnitude(target.position - transform.position) > 4 && !isAttackStarted)
                {
                    Attacking();
                    isAttackStarted = true;

                    animator.SetTrigger(attacks[0].triggerName);

                    missulesPool.PoolObject();
                }
                else if (!isAttackStarted && !Damaged)
                {
                    Attacking();
                    isAttackStarted = true;

                    animator.SetTrigger("Attack2");
                    skills[0].Execute();
                }
                break;
        }

    }

    protected override void Die()
    {
        base.Die();
        deadParticles.SetActive(true);
    }

    IEnumerator Teleporate()
    {
        state = UnitStates.TELEPORTATION;

        teleportationEffect.Play();
        targetView.SetActive(false);
        SkillCast();

        yield return new WaitForSeconds(0.8f);

        unitCollider.enabled = false;      
        foreach (var renderer in _renderers)
            renderer.enabled = false;

        float t = 2.8f + Random.Range(-0.5f, 0.5f);
        yield return new WaitForSeconds(t);

        Vector3 offset = Random.onUnitSphere * UnitSize;
        offset.y = 0;

        agent.Warp(target.position + offset);

        teleportationEffect.Play();
        SkillCast();

        yield return new WaitForSeconds(1f);

        transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        unitCollider.enabled = true;
        foreach (var renderer in _renderers)
            renderer.enabled = true;

        state = UnitStates.ATTACKING;
    }
}
