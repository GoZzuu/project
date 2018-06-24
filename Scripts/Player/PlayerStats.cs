using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : CharacterStats
{
    public event CharactersStatsEvents PlayerDamaged;


    public float CurrentSouls = 0;
    public GameObject sparks;

    PlayerMovement movementController;
    PlayerWeaponAndAttacks battleController;

    public Ability[] abilities;
    AbilityCaster[] abilitiesSystem;

    protected override void Start()
    {
        base.Start();
        movementController = GetComponent<PlayerMovement>();
        battleController = GetComponent<PlayerWeaponAndAttacks>();

        foreach (var ability in abilities)
            ability.Initialize(this);
    }
    private void FixedUpdate()
    {
        /*
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("SkillCast");
            abilities[0].Cast();
        }*/
    }

    public override void Heal(float value)
    {
        base.Heal(value);
        
    }
    public override void GetDamage(float damage, bool canBeBlocked, float stunChance = 0.1F)
    {
        if (canBeBlocked && battleController.IsBlocking)
        {
            animator.SetTrigger("blocked_impact");
            Instantiate(sparks, transform);
        }
        else
        {
            base.GetDamage(damage, canBeBlocked, stunChance);
            PlayerDamaged();
        }
    }

    protected override void Die()
    {
        base.Die();
        movementController.enabled = false;
        battleController.enabled = false;
    }
}
