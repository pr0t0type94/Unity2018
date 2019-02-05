using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour {

    // Use this for initialization
    Player_Health player_hp;


void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PickedUpItem(string ItemType)
    {
        if (ItemType == "health")
        { }
            //player_hp.updateHP();
    }
}
