using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Shield : MonoBehaviour {

    private GameObject player;
    public float MaxShield=100;
    public float CurrShield;
    public Text currentShield_Text;
    public Slider Slider;
    public bool ShieldAvailable;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (CurrShield > 0)
        {
            ShieldAvailable = true;
        }
        else
        {
            ShieldAvailable = false;
        }


        if (CurrShield >= MaxShield)
        {
            CurrShield = MaxShield;
        }
        if(CurrShield<0)
        {
            CurrShield = 0;
        }
    }

    private void FixedUpdate()
    {       
        UpdateShield();       
    }

    public void ShieldPowerUpCollected()
    {
        CurrShield += 25;
    }

    public void UpdateShield()
    {
        Slider.value = CurrShield / 100;
        UpdateShieldText();
    }

    void UpdateShieldText()
    {
        currentShield_Text.text = CurrShield.ToString();

       
    }
}
