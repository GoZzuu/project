using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSearchingTarget : MonoBehaviour {

    public bool Targeted
    {
        get
        {
            return Target != null;   // && !targetHealth.Died && Vector3.SqrMagnitude(Target.position - transform.position) < 16;
        }
    } 
    
    public Transform Target;
    EnemyAIbase targetHealth;
    private readonly Collider[] targets = new Collider[5];
    const int maxTargetingAngle = 80;

    public float meleeTargetingRange = 5f;
    public float rangedTargetingRange = 7f;

    int mask = 256;

    PlayerWeaponAndAttacks weapon;

    void Start () {
        weapon = GetComponent<PlayerWeaponAndAttacks>();

    }
	
	// Update is called once per frame
	void FixedUpdate () {
        //Debug.Log((int)LayerMask);
        SearchingForTargets();

    }

    void SearchingForTargets()
    {
        if (!weapon.CurrentWeaponRanged())
        {

            int targetsCount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * meleeTargetingRange / 3 * 2, meleeTargetingRange, targets, mask);
            Transform temp = null;

            for (int i = 0; i < targetsCount; i++)
            {
                if (temp == null)
                {
                    temp = targets[i].transform;
                }
                else if (temp != null && targets[i] != null)
                {
                    bool distanceCheck = Vector3.SqrMagnitude(temp.transform.position - transform.position)
                        > Vector3.SqrMagnitude(targets[i].transform.position - transform.position);

                    bool angleCheck = Vector3.Angle(transform.forward, temp.transform.position - transform.position)
                        > Vector3.Angle(transform.forward, targets[i].transform.position - transform.position);

                    if (distanceCheck || angleCheck)
                        temp = targets[i].transform;
                }
            }

            if (!Targeted && temp != null)
            {
                targetHealth = temp.GetComponent<EnemyAIbase>();
                if (!targetHealth.Died || targetHealth.state != EnemyAIbase.UnitStates.UNACTIVE)
                {
                    //Debug.Log("Targeted");
                    Target = temp.transform;
                    targetHealth.Targeted();
                }
                else
                {
                    targetHealth = null;
                    Target = null;
                }
            }
            else if (Targeted && temp == null)
                Target = null;
            /*
            }
            else
            {
                float _angleBtw = Vector3.Angle(transform.forward, Target.position - transform.position);
                if (targetHealth.Died)// || Vector3.SqrMagnitude(Target.position - transform.position) > meleeTargetingRange * meleeTargetingRange)
                   // || !InRange(_angleBtw, maxTargetingAngle + 5))
                {
                    //Debug.Log("Untargeted, angle " + _angleBtw);
                    targetHealth.Targeted();
                    Target = null;
                }
            }*/
        }
        else
        {
            if (!Targeted)
            {
                int targetsCount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * rangedTargetingRange / 3 * 2, rangedTargetingRange, targets, mask);
                Transform temp = null;

                for (int i = 0; i < targetsCount; i++)
                {
                    bool lokingAtTarget = InRange(Vector3.Angle(transform.forward, targets[i].transform.position - transform.position),
                        maxTargetingAngle);

                    // Is target visible?

                    if (temp == null)
                        temp = targets[i].transform;
                    else
                    if (temp != null && targets[i] != null && Vector3.SqrMagnitude(temp.transform.position - transform.position)
                        > Vector3.SqrMagnitude(targets[i].transform.position - transform.position) && lokingAtTarget)
                    {
                        temp = targets[i].transform;
                    }
                }

                if (temp != null)
                {
                    targetHealth = temp.GetComponent<EnemyAIbase>();
                    if (!targetHealth.Died && targetHealth.state != EnemyAIbase.UnitStates.UNACTIVE)
                    {
                        //Debug.Log("Targeted");
                        Target = temp.transform;
                        targetHealth.Targeted();
                    }
                    else
                        targetHealth = null;
                }
                else
                    Target = null;
            }
            else
            {
                float _angleBtw = Vector3.Angle(transform.forward, Target.position - transform.position);
                if (targetHealth.Died || Vector3.SqrMagnitude(Target.position - transform.position) > 169
                    || !InRange(_angleBtw, maxTargetingAngle - 10))
                {
                    // Debug.Log("Untargeted, angle " + _angleBtw + "   " + Vector3.SqrMagnitude(Target.position - transform.position));
                    targetHealth.Targeted();
                    Target = null;
                }
            }
        }
    }





    public bool TargetsNear()
    {
        if (!Targeted)
        {
            int targetsCount = Physics.OverlapSphereNonAlloc(transform.position + transform.forward * meleeTargetingRange / 3 * 2, meleeTargetingRange, targets, mask);
            Transform temp = null;

            for (int i = 0; i < targetsCount; i++)
            {
                bool lokingAtTarget = InRange(Vector3.Angle(transform.forward, targets[i].transform.position - transform.position),
                    maxTargetingAngle);

                //float angleBtw = Vector3.Angle(transform.forward, targets[i].transform.position - transform.position);

                if (temp == null)
                    temp = targets[i].transform;

                if (temp != null && targets[i] != null && Vector3.SqrMagnitude(temp.transform.position - transform.position)
                    > Vector3.SqrMagnitude(targets[i].transform.position - transform.position) && lokingAtTarget)
                {
                    temp = targets[i].transform;
                }
            }

            if (temp != null)
            {
                targetHealth = temp.GetComponent<EnemyAIbase>();
                if (!targetHealth.Died && targetHealth.state != EnemyAIbase.UnitStates.UNACTIVE)
                {
                    //Debug.Log("Targeted");
                    Target = temp.transform;
                    targetHealth.Targeted();
                }
                else
                    targetHealth = null;
            }
            else
                Target = null;
        }
        else
        {
            float _angleBtw = Vector3.Angle(transform.forward, Target.position - transform.position);
            if (targetHealth.Died || Vector3.SqrMagnitude(Target.position - transform.position) > 16
                || !InRange(_angleBtw, maxTargetingAngle + 5))
            {
                //Debug.Log("Untargeted, angle " + _angleBtw);
                targetHealth.Targeted();
                Target = null;
            }
        }
        return true;
    }

    bool InRange(float value, float maxRange, float minRange)
    {
        return value < maxRange && value > minRange;
    }
    bool InRange(float value, float maxRange)
    {
        return value < maxRange && value > -maxRange;
    }
}
