using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Enemy_Canvas : MonoBehaviour {

    // Use this for initialization
    Enemy player_target;
    public Gun GunScript;
    public Text EnemyHP;
    public Text EnemyName;
    public Slider EnemySlider;
    public Canvas EnemyCanvas;

    void Start () {
        EnemyCanvas.enabled = false;

    }
	
	// Update is called once per frame
	void Update () {
        player_target = GunScript.GetTarget();


        if (player_target != null && player_target.isActiveAndEnabled)
        {
            if (player_target.currHealth <=0)
            {
                 player_target = null;
            }

            EnemyCanvas.enabled = true;

            EnemyName.text = player_target.gameObject.name;
            UpdateSliderHp();
        }
        else
        {
            EnemyCanvas.enabled = false;

        }
    }

    void displayTargetHp()
    {
        EnemyHP.text = player_target.currHealth.ToString();
    }

    void UpdateSliderHp()
    {
        EnemySlider.value = player_target.currHealth / 100;
    }
}
