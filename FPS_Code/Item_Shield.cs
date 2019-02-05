using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Shield : MonoBehaviour {

    Player_Shield player_shield;
    public AudioSource source;

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("ENTER");

        if (other.gameObject.tag == "Player")
        {
            player_shield = other.gameObject.GetComponent<Player_Shield>();

            if (player_shield.CurrShield < player_shield.MaxShield)
            {
                source.Play();
                player_shield.ShieldPowerUpCollected();

                Destroy(gameObject);
            }
        }
    }
}
