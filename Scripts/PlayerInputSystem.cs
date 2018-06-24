using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSystem : MonoBehaviour {

    public delegate void InputEvents();
    public delegate void InputFloatEvents(float x);
    public event InputEvents SwitchWeaponEvent;
    public event InputEvents AttackEvent;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
