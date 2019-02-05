using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_HP : MonoBehaviour {
    Player_Health healthScript;
    public AudioSource source;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            healthScript = other.gameObject.GetComponent<Player_Health>();

            if(healthScript.CurrHp < healthScript.MaxHp)
            {
                source.Play();
                healthScript.HpPowerUpCollected();
                Destroy(gameObject);
            }
        }
    }
}
