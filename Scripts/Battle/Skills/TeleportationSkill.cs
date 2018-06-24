using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportationSkill : Skill {

    public ParticleSystem teleportationEffect;
    public AudioClip skillAudio;

    Renderer[] _renderers;
    Collider unitCollider;

    public override void Initialize(Transform transform, LayerMask attackTo)
    {
        throw new System.NotImplementedException();
    }

    public override void Execute()
    {
        throw new System.NotImplementedException();
    }

    IEnumerator Teleporate()
    {
        //state = UnitStates.TELEPORTATION;

        teleportationEffect.Play();
        //targetView.SetActive(false);
        //SkillCast();

        yield return new WaitForSeconds(0.8f);

        unitCollider.enabled = false;
        foreach (var renderer in _renderers)
            renderer.enabled = false;

        float t = 2.8f + Random.Range(-0.5f, 0.5f);
        yield return new WaitForSeconds(t);

        //Vector3 offset = Random.onUnitSphere * UnitSize;
        //offset.y = 0;

        //agent.Warp(target.position + offset);

        teleportationEffect.Play();
        //SkillCast();

        yield return new WaitForSeconds(1f);

        //transform.rotation = Quaternion.LookRotation(target.position - transform.position);

        unitCollider.enabled = true;
        foreach (var renderer in _renderers)
            renderer.enabled = true;

        //state = UnitStates.ATTACKING;
    }
}
