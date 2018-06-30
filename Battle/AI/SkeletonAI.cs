﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAI : EnemyAIbase
{
    public GameObject deadParticles;   

    bool Revived = false;

    protected override void Start()
    {
        base.Start();
        animator.enabled = false;
        unitCollider.enabled = false;
    }

    protected override void FixedUpdate()
    {
        if (Died)
            return;

        base.FixedUpdate();
        int targetsCount;

        switch (state)
        {        
            case UnitStates.WAIT:

                if (!animator.enabled)
                {
                    unitCollider.enabled = true;
                    animator.enabled = true;
                    Invoke("Revive", 2f);
                }

                if (Revived == false)
                    return;

                    state = UnitStates.MOVING;

                break;
            case UnitStates.MOVING:

                if (target == null || Vector3.SqrMagnitude(target.position - transform.position) > alliesDetectionRange * alliesDetectionRange * 3)
                {
                    state = UnitStates.WAIT;
                    animator.SetBool("isMoving", false);
                }

                if (Vector3.SqrMagnitude(target.position - transform.position) > 1 +  UnitSize && !isAttackStarted)
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

                transform.rotation = Quaternion.LookRotation(target.position - transform.position);


                if (Vector3.SqrMagnitude(target.position - transform.position) > 1 + UnitSize)
                {
                    state = UnitStates.MOVING;

                }else if (!isAttackStarted && !Damaged)
                {
                    Attacking();
                    isAttackStarted = true;
                    animator.SetTrigger(attacks[0].triggerName);
                }
                break;
        }

    }
   

    void Revive()
    {
        Revived = true;
    }

    protected override void Die()
    {
        base.Die();
        deadParticles.SetActive(true);
    }
}
