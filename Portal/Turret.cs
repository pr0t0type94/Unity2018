using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour {

    // Use this for initialization
    public LineRenderer m_LineRenderer;
    public LayerMask m_CollisionLayerMask;
    public float m_MaxDistance = 250.0f;
    public float m_AngleLaserActive = 60.0f;

    public LevelController GameController;
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 l_EndRaycastPosition = Vector3.forward * m_MaxDistance;
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward), out l_RaycastHit, m_MaxDistance, m_CollisionLayerMask.value))
        {
            l_EndRaycastPosition = Vector3.forward * l_RaycastHit.distance;
            //m_LineRenderer.SetPosition(1, l_EndRaycastPosition);


            if(l_RaycastHit.collider.tag == "Turret" && l_RaycastHit.collider.gameObject != this.gameObject && m_LineRenderer.enabled == true)
            {
                Turret turret = l_RaycastHit.collider.gameObject.GetComponent<Turret>();
                turret.DestroyTurret();
            }

            if(l_RaycastHit.collider.tag == "Player" && m_LineRenderer.enabled == true)
            {
                //GC kill player and restart game;
                GameController.KillPlayer();
            }


        }
        m_LineRenderer.SetPosition(1, l_EndRaycastPosition);
       


        float l_DotAngleLaserActive = Mathf.Cos(m_AngleLaserActive * Mathf.Deg2Rad * 0.5f);
        bool l_RayActive = Vector3.Dot(transform.up, Vector3.up) > l_DotAngleLaserActive;

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Companion" || collision.gameObject.tag == "Turret")
        {
            m_LineRenderer.enabled = false;
            AudioSource[] source = GetComponents<AudioSource>();
            source[0].Stop();
            source[1].Play();
        }
    }

    public void DestroyTurret()
    {

        AudioSource[] source = GetComponents<AudioSource>();
        source[0].Stop();
        source[1].Play();
        Destroy(gameObject, 0.5f);
    }


    //class end
}
