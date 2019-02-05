using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChekpointPosition : MonoBehaviour {

    // Use this for initialization
    public Transform respawnPosition;

    public AudioSource source;
	void Start () {
        source = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
