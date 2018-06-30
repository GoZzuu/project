using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationCaster : AbilityCaster {

    float teleportationTime = 1;

    List<Renderer> _renderers = new List<Renderer>();

    public override void CastSequince(Vector3 onPoint)
    {
      
    }

    public override void CastSequince()
    {
        Debug.Log("Teleportating " + caster.name);
        StartCoroutine(CastingSkill());
    }

    private void FixedUpdate()
    {
        Debug.Log("Teleportating " + caster.name + "...");
    }


    public void Initial(CharacterStats caster, TeleportationAbility ability)
    {
        base.Init(caster, ability);
        teleportationTime = ability.teleportationSpeed;
        
        caster.GetComponentsInChildren(_renderers);
        


        this.enabled = false;
    }

    public override IEnumerator CastingSkill()
    {
        //caster.state = UnitStates.TELEPORTATION;
        //SkillCast();
        BaseSkillFX.Play();

        if (caster.GetType() == typeof(EnemyAIbase))
        {
            var enemy = (EnemyAIbase)caster;
            enemy.targetView.SetActive(false);
        }
        //Sound Play

        yield return new WaitForSeconds(0.8f);

        caster.unitCollider.enabled = false;
        foreach (var renderer in _renderers)
            renderer.enabled = false;

        float t = 2.8f + Random.Range(-0.5f, 0.5f);
        yield return new WaitForSeconds(teleportationTime);

        /*
        Vector3 offset = Random.onUnitSphere * UnitSize;
        offset.y = 0;

        agent.Warp(target.position + offset);
        */
        transform.position += new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * 3;

        BaseSkillFX.Play();
        //Sound Play

        yield return new WaitForSeconds(1f);

        //transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        caster.unitCollider.enabled = true;

        foreach (var renderer in _renderers)
            renderer.enabled = true;

        //state = UnitStates.ATTACKING;
    }
}
