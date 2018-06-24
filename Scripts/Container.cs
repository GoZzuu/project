using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Container : MonoBehaviour {

    Text text;
    Transform player;

    Color textColor;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
        player = FindObjectOfType<PlayerStats>().transform;
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if(Vector3.Distance(transform.position, player.position) < 10)
        {
            text.color = new Color(255, 255,255, 1.2f / Vector3.Distance(transform.position, player.position));
        }
        else
        {
            text.color = Color.clear;
        }

		
	}
}
