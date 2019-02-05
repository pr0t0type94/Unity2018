using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Ammo : MonoBehaviour {

    Gun player_ammo;
    public AudioSource source;
    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("ENTER");

        if (other.gameObject.tag == "Player")
        {
            source.Play();
            player_ammo = other.gameObject.GetComponentInChildren<Gun>();
          
            player_ammo.AmmoItemCollected();
            Destroy(gameObject);
            

        }
    }
}
