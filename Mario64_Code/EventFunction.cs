using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventFunction : MonoBehaviour {

    // Use this for initialization
    public AudioSource stepSource;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    //ANIMATION EVENTS
    public void Step(string stringParameter)
    {
        //Code
        if (stringParameter == "Step")
        {
            stepSource.Play();
        }
    }
}
