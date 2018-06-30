using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButtonsController : MonoBehaviour {

    public GameObject attackButton;
    public GameObject defenceButton;
    public GameObject switchWeaponButton;

    public PlayerWeaponAndAttacks playerWeapon;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (playerWeapon.CurrentWeaponRanged() == defenceButton.activeSelf)
        {
            defenceButton.SetActive(!playerWeapon.CurrentWeaponRanged());
        }

        if(playerWeapon.GetWeaponsCount()>1 && !switchWeaponButton.activeSelf)
        {
            switchWeaponButton.SetActive(true);
        }
	}
}
