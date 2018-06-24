using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactible : MonoBehaviour {

    public delegate void Interact(Interactible objectToInteract, bool Ready);
    //public static event Interact ReadyToInteract;

    protected PlayerMovement player;


    void Start()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    public abstract void PerformInteract();

}
