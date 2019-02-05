using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour {

    private bool isDoorOpen;
    public GameController gc;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (gc.KeysCollected>=1)
            {
                OpenDoor();
            }
        }
    }

    
    void OpenDoor()
    {
        //play animation 
    }

}
