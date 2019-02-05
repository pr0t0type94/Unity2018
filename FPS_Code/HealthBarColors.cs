using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBarColors : MonoBehaviour {

    [SerializeField]

    public Slider HealthBar;
    public Color Health_low;
    public Color Health_medium;
    public Color Health_high;
    public Image Health_image;

    Vector3 health_startshape;
    Color currentHealth_Color;
    // Use this for initialization
     float currentHealth;

	void Awake () {
	}
	
	// Update is called once per frame
	void Update () {
        if (HealthBar.value <= 0.3f)
            currentHealth_Color = Color.Lerp(Health_image.color,Health_low,Time.deltaTime);
        else if (HealthBar.value <= 0.6f)
            currentHealth_Color = Color.Lerp(Health_image.color, Health_medium,Time.deltaTime); 
        else 
            currentHealth_Color = Color.Lerp(Health_image.color, Health_high,Time.deltaTime); 

        Health_image.color = currentHealth_Color;


    }

    public void Return_currentHealth()
    {

    }
}
