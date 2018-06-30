using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//public delegate void InventoryEvents(Item item);

public class Container : MonoBehaviour {

    

    Text text;
    Transform player;
    Color textColor;

    public Item[] Items;

    bool Opened = false;

	// Use this for initialization
	void Start () {
        text = GetComponentInChildren<Text>();
        player = FindObjectOfType<PlayerStats>().transform;
        
    }
	
	// Update is called once per frame
	void FixedUpdate () {

        if (Opened)
            return;

            float dist = (transform.position - player.position).sqrMagnitude;
        if (dist < 100)
        {
            text.color = new Color(255, 255, 255, 1.2f / Vector3.Distance(transform.position, player.position));

            if (dist < 6)
                Open();
            }
        else
        {
            text.color = Color.clear;
        }
        
		
	}


    public void Open()
    {
        Opened = true;
        GetComponent<Animator>().SetTrigger("Opening");

        foreach (var item in Items)       
            item.Collect(player.GetComponent<PlayerStats>());     

        text.color = Color.clear;
    }
}
