using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diana : MonoBehaviour {

    public GameController gc;
    public float dianaPoints = 250f;
    //public float DianaPoints;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		

	}
    public void dianaHited()
    {
        gc.SumPoints(dianaPoints);
        gameObject.SetActive(false);
        //sum points
    }

}
