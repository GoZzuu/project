using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboAttacks : StateMachineBehaviour {

    public delegate void AttacksCallback(int attackID);
    public event AttacksCallback AttackStartedHandler;
    public event AttacksCallback AttackEndedHandler;

    public int ID = 0;
    [Range(0,1)]
    public float attackTime = 0.5f;
    bool activated = false;

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if(stateInfo.normalizedTime >= attackTime && !activated)
        {
            AttackStartedHandler(ID);
            activated = true;
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        AttackEndedHandler(ID);
        activated = false;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
