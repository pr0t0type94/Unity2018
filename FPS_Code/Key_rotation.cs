using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key_rotation : MonoBehaviour {

    public float rotationSpeed;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0f, 1f, 0f) * Time.deltaTime * rotationSpeed);
    }
}
