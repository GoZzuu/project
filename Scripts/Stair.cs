using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stair : Interactible {

    public static event Interact ReadyToInteract;
    public static event Interact _Interact;


    public Transform downPoint;
    public Transform upPoint;

    private void Start()
    {
        
    }

    public override void PerformInteract()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReadyToInteract(this, true);
        }
    }


    IEnumerator PerformingAction()
    {
        player.MoveTo(downPoint.position);
        while (true)
            yield return null;

    }
}
