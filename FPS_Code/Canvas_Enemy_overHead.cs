using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Canvas_Enemy_overHead : MonoBehaviour {

    public float m_LifeBarOffsetY;
    public Image m_LifeBar;
    public RectTransform m_LifeBarCanvasRectTransform;
    public Transform m_EnemyTransform;
    public Enemy enemy;
    public FPSController player;
    public float minDotProduct;
    private bool playerSeesEnemy;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        PlayerCanSeeEnemy();
        if (playerSeesEnemy)
        {
            m_LifeBar.enabled = true;

            Vector3 l_ViewportPoint = Camera.main.WorldToViewportPoint(m_EnemyTransform.position + Vector3.up * m_LifeBarOffsetY);
            m_LifeBar.rectTransform.anchoredPosition = new Vector3(l_ViewportPoint.x * m_LifeBarCanvasRectTransform.sizeDelta.x, -(1.0f - l_ViewportPoint.y) * m_LifeBarCanvasRectTransform.sizeDelta.y, 0.0f);
        }
        else
        {
            m_LifeBar.enabled = false;
        }
    }

    void PlayerCanSeeEnemy()
    {
        Vector3 playerFrwd = player.transform.forward;
        Vector3 playerFrwdXZ = new Vector3(playerFrwd.x, 0, playerFrwd.z);
        //Vector3 directionXZ = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 Direction = playerFrwdXZ - transform.position;//directionXZ;
        float distance = Direction.magnitude;
        Direction /= distance;

        float dotProduct = Vector3.Dot(Direction, playerFrwdXZ.normalized);
        //Debug.Log(dotProduct);
        if (dotProduct > minDotProduct)
        {
            playerSeesEnemy = true;
        }
        else
        {
            playerSeesEnemy = false;

        }
    }

}
