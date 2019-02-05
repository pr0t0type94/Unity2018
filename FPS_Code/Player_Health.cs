using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Health : MonoBehaviour {

    // Use this for initialization


    [SerializeField]

    public float MaxHp=1000;
    public float CurrHp;
    public Text currentHealth_Text;
    public Slider Slider;

    public Player_Shield player_shield;

    public GameController gc;
	// Update is called once per frame
	void Update () {
        UpdateHealthText();

        if (Input.GetKeyDown(KeyCode.L))
            RecieveDamage(100);

        UpdateHp();

    }
    public void UpdateHealthText()
    {
        currentHealth_Text.text = CurrHp.ToString();
    }

    public void RecieveDamage(float amount)
    {
        //if active shield >> reduce damage

        if (player_shield.ShieldAvailable)
        {
            CurrHp -= amount * 0.25f;
            player_shield.CurrShield -= amount * 0.075f;
        }
        else
        {
            CurrHp -= amount;
        }


        if (CurrHp <= 0)
        {
            gc.PlayerDie();
        }
    }
    public void Die()
    {
        //RestartGame
        //MenuScreen
        //HP=0
        //CANT move,shoot..
    }

    public void UpdateHp()
    {
        Slider.value = CurrHp / 1000;

    }
    public void HpPowerUpCollected()
    {
        CurrHp += 150;
        if (CurrHp >= MaxHp)
        {
            CurrHp = MaxHp;
        }
    }


}
