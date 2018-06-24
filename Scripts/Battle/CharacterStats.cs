
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour {

    public delegate void CharactersStatsEvents();
    public delegate void CharactersStatsDamageEvents(int damage);
    public event CharactersStatsDamageEvents CharacterDamagedEventHandler;
    public event CharactersStatsEvents CharacterHealedEventHandler;
    public event CharactersStatsEvents CharacterDieEventHandler;
    public event CharactersStatsEvents CharacterActivatedEventHandler;
    public event CharactersStatsEvents CharacterAttackEventHandler;
    public event CharactersStatsEvents CharacterSkillCastEventHandler;

    public float health = 100;
    public float currentHealth;

    [HideInInspector]
    public bool Died { get
        {
            return currentHealth <= 0;
        }
    }


    [HideInInspector] public Collider unitCollider;
    public LayerMask enemiesMask;
    
    protected bool Damaged = false;

    public GameObject blood;
        
    protected Animator animator;
    WaitForSeconds damagedTime = new WaitForSeconds(1);
   
    
    protected virtual void Start(){
        currentHealth = health;
        unitCollider = GetComponent<Collider>();

        if (!animator)
            animator = GetComponent<Animator>();
    }
    public virtual void Activate()
    {
        CharacterActivatedEventHandler();
    }
    public virtual void Attacking()
    {
        CharacterAttackEventHandler();
    }
    public virtual void SkillCast()
    {
        CharacterSkillCastEventHandler();
    }
    /*
	public virtual void GetDamage(float damage, bool interuption = false)
    {
        if (Died)
            return;

        currentHealth -= damage;
        Instantiate(blood, transform.position + Vector3.up, Quaternion.identity, transform);
        CharacterDamagedEventHandler();

        if (interuption)       
            StartCoroutine(DamagedCoroutine());     
        else       
            animator.SetTrigger("LightDamaged");        

        if (Died)     
            Die();      
    }*/


    public virtual void GetDamage(float damage, bool canBeBlocked, float stunChance = 0.1f)
    {
        if (Died)
            return;

        Debug.Log(name + " get " + damage + " damage");

        currentHealth -= damage;
        Instantiate(blood, transform.position + Vector3.up, Quaternion.identity, transform);
        CharacterDamagedEventHandler((int)damage);

        float r = Random.Range(0, 1f);

        if (r < stunChance)
            StartCoroutine(DamagedCoroutine());
        else
            animator.SetTrigger("LightDamaged");

        if (Died)
            Die();
    }
    public virtual void Heal(float value)
    {
        currentHealth += value;

        if (currentHealth > health)
            currentHealth = health;

       // CharacterHealedEventHandler();
    }
    protected virtual void Die()
    {
        animator.SetTrigger("Die");
        animator.SetBool("isDie", true);

        CharacterDieEventHandler();
    }

    IEnumerator DamagedCoroutine()
    {
        
        Damaged = true;
        animator.SetTrigger("Damaged");
        yield return damagedTime;
        Damaged = false;
    }
    
}
