using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackTypesToLayer
{
    ALLIES,
    ENEMIES,
    PROJECTILES
}


public class Bullet : IPooledObject
{
    public float speed = 10f;

    public float detectRange = 0.2f;
    Collider[] attackTo = new Collider[1];
    public WeaponBase weapon;

    public Attack attack;

    public LayerMask attacksTo;
    float t = -0.2f;

    public bool useGravity = true;

    public GameObject particles;
    Vector3 direction;
    bool collide = false;

    public override void PoolObject()
    {
        base.PoolObject();
        t = -0.1f;
        direction = (transform.forward * speed);
    }
    

    void FixedUpdate()
    {
        if (useGravity)
        {
            t += Time.fixedDeltaTime * 0.3f;
            direction -= transform.up * 0.5f * t;
            transform.rotation = Quaternion.LookRotation(direction);
        }
        if (!collide)
        {
            transform.position += direction * Time.fixedDeltaTime;
            DetectUnits();
        }
    }
    void DetectUnits()
    {
        int colCount = Physics.OverlapSphereNonAlloc(transform.position, detectRange, attackTo, attacksTo);

        if (colCount > 0)
        {
            collide = true;

            for (int i = 0; i < colCount; i++)
            {
                Debug.Log("Hit " + attackTo[i].name);
                var h = attackTo[i].GetComponent<CharacterStats>();
                if (h)
                    h.GetDamage(attack.DamageMultiplier * Random.Range(weapon.MinDamage,weapon.MaxDamage), attack.CanBeBlocked, attack.stunChance);
            }
            if(!particles)
               DeactivateObject();
            else
            {
                particles.SetActive(true);
                Invoke("DeactivateObject", 1f);
            }
        }
    }

    public override void DeactivateObject()
    {
        base.DeactivateObject();
        collide = false;
        if (particles)
            particles.SetActive(false);
    }
}
