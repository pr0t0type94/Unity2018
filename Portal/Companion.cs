using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companion : MonoBehaviour {

    // Use this for initialization
    private bool canTeleport;
    float countDownToTeleportAgain;
    float iniTimer = .1f;
    bool startTimer;
    bool teleportDone;
    private Vector3 iniScale;
    public AudioSource source;
	void Start () {
        countDownToTeleportAgain = iniTimer;
        canTeleport = true;
        iniScale = transform.localScale;
    }
	
	// Update is called once per frame
	void Update () {
		if (canTeleport == false)
        {
            countDownToTeleportAgain -= Time.deltaTime;
            if(countDownToTeleportAgain <= 0)
            {
                canTeleport = true;
                countDownToTeleportAgain = iniTimer;
            }
        }
	}

    public void SetTeleport(bool state)
    {
        if (state == true)
            canTeleport = true;
        else
            canTeleport = false;
    }


    public void Teleport(Portal _Portal)
    {
        if (canTeleport)
        {
            Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            Vector3 l_Position = _Portal.transform.InverseTransformPoint(transform.position);
            transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_Position);
            Vector3 l_Direction = _Portal.transform.InverseTransformDirection(-transform.forward);
            transform.forward = _Portal.m_MirrorPortal.transform.TransformDirection(l_Direction);

            Vector3 l_Velocity = _Portal.transform.InverseTransformDirection(-l_Rigidbody.velocity);
            l_Rigidbody.velocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_Velocity);

           

            transform.localScale *= (_Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x);

            source.Play();

            canTeleport = false;
        }
        
    }





    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "BluePortal")
        {

            Portal blue_portal = other.GetComponent<Portal>();
            Teleport(blue_portal);
        }
        if (other.gameObject.tag == "OrangePortal")
        {

            Portal orange_portal = other.GetComponent<Portal>();
            Teleport(orange_portal);
        }

    }

    //end of cls
}
