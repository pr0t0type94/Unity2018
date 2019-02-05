using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	

void OnTriggerEnter ( Collider collider  )
    {
        
	gameObject.GetComponent<Animation>().Play("open");
    }

void OnTriggerExit ( Collider collider  )
    {
	gameObject.GetComponent<Animation>().Play("close");
    }
}