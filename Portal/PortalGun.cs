using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour {

    // Use this for initialization
    public Camera mainCam;
    public Transform m_AttachingPosition;
    public bool m_AttachedObject;
    private Rigidbody m_ObjectAttached;
    public float m_AttachingObjectSpeed;
    private Quaternion m_AttachingObjectStartRotation;
    private bool m_AttachingObject;
    public float impulseForce = 50f;
    public float WeaponRange;
    public LayerMask weaponLayer;

    public float m_SetPortalOffset;
    private Companion CompanionTarget;
    private Transform collisionWall;
    public Portal bluePortal;
    public Portal orangePortal;

    private Turret turret;

    public bool canShoot;
    private float shootCounter=.2f;

    public Transform m_PlayerCamera;

    public Renderer previewQuadBlue;
    public Material[] quadMaterialsBlue = new Material[2];

    public Renderer previewQuadOrange;
    public Material[] quadMaterialsOrange = new Material[2];

    public GameObject BluePortalOBJ;
    public GameObject OrangePortalOBJ;
    Vector3 scale = new Vector3(1f, 1f, 1f);

    public AudioSource expulseObject;
    public AudioSource shootPortal;
    public AudioSource grabingSource;
    public AudioSource releaseSource;
    private void Start()
    {
        canShoot = true;
    }
    
	
	// Update is called once per frame
	void Update () {

        if(/*bluePortal.IsValidPosition() && */ canShoot && Input.GetKey(KeyCode.Mouse0) && (!CompanionTarget && !turret))
        {
            PreviewPortal("blue");
            ResizingPortal(BluePortalOBJ);                    
        }
        else if (canShoot && Input.GetKeyUp(KeyCode.Mouse0) && (!CompanionTarget || !turret))
        {
            Shoot();
            ShootPortal("blue");
            shootPortal.Play();

        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && (CompanionTarget || turret))
        {
            ExpulseObject(impulseForce);
            expulseObject.Play();
        }

        //orange portal

        if (canShoot && Input.GetKey(KeyCode.Mouse1) && (!CompanionTarget && !turret))
        {
            PreviewPortal("orange");
            ResizingPortal(OrangePortalOBJ);
            
        }
        else if(Input.GetKeyDown(KeyCode.Mouse1) && (CompanionTarget || turret))
        {
            DetachObject();
            releaseSource.Play();
        }
        else if(canShoot && Input.GetKeyUp(KeyCode.Mouse1) && (!CompanionTarget || !turret))
        {
            ShootPortal("orange");
            shootPortal.Play();

        }


        //update attach
        if (CompanionTarget || turret)
        {
            UpdateAttachedObject();
        }
           
        //can shoot timer
        if (canShoot == false)
        {
            shootCounter -= Time.deltaTime;
            if(shootCounter <= 0)
            {
                canShoot = true;
                shootCounter = .2f;
            }
        }
    }

    void SetPortal(Portal _Portal, Vector3 Position, Vector3 Normal, float Scale)
    {
        _Portal.transform.position = Position + Normal * m_SetPortalOffset;
        _Portal.transform.forward = Normal;
        //_Portal.transform.localScale = Vector3.one * Scale;
    }

    //attraction gun
    void Shoot()
    {
        RaycastHit HitInfo;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out HitInfo, WeaponRange))
        {
            //Debug.Log(HitInfo.transform.name);
            CompanionTarget = HitInfo.transform.GetComponent<Companion>();

            if (CompanionTarget != null)
            {
                m_AttachedObject = true;
                m_ObjectAttached = CompanionTarget.GetComponent<Rigidbody>();
                m_ObjectAttached.isKinematic = true;
                
                m_AttachingObjectStartRotation = CompanionTarget.GetComponent<Quaternion>();

            }

            turret = HitInfo.transform.GetComponent<Turret>();

            if (turret != null)
            {
                m_AttachedObject = true;
                m_ObjectAttached = turret.GetComponent<Rigidbody>();
                m_ObjectAttached.isKinematic = true;

                m_AttachingObjectStartRotation = turret.GetComponent<Quaternion>();

            }

        }

    }

    //portal gun
    void ShootPortal(string PortalType)
    {
        RaycastHit HitInfo;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out HitInfo, WeaponRange))
        {
            //Debug.Log(HitInfo.transform.name);
            collisionWall = HitInfo.transform.GetComponent<Transform>();

           
            if (HitInfo.transform.gameObject.tag == "WallYes" && PortalType == "blue")
            {

                SetPortal(bluePortal, HitInfo.point, HitInfo.normal, 1);

            }

            else if (HitInfo.transform.gameObject.tag == "WallYes" && PortalType == "orange")
            {

                SetPortal(orangePortal, HitInfo.point, HitInfo.normal, 1);

            }

        }

        previewQuadBlue.material = quadMaterialsBlue[0];
        previewQuadOrange.material = quadMaterialsOrange[0];

    }

    //portal previewing
    void PreviewPortal(string PortalType)
    {
        RaycastHit HitInfo;
        if (Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out HitInfo, WeaponRange))
        {
            //Debug.Log(HitInfo.transform.name);
            collisionWall = HitInfo.transform.GetComponent<Transform>();


            if (HitInfo.transform.gameObject.tag == "WallYes" && PortalType == "blue")
            {
                previewQuadBlue.material = quadMaterialsBlue[1];
                SetPortal(bluePortal, HitInfo.point, HitInfo.normal, 1);

            }

            else if (HitInfo.transform.gameObject.tag == "WallYes" && PortalType == "orange")
            {
                previewQuadOrange.material = quadMaterialsOrange[1];
                SetPortal(orangePortal, HitInfo.point, HitInfo.normal, 1);
            }

        }
    }


    void UpdateAttachedObject()
    {
        Vector3 l_EulerAngles = m_AttachingPosition.rotation.eulerAngles;
        if (!m_AttachedObject)
        {
            Vector3 l_Direction = m_AttachingPosition.transform.position - m_ObjectAttached.transform.position;
            float l_Distance = l_Direction.magnitude;
            float l_Movement = m_AttachingObjectSpeed * Time.deltaTime;
            if (l_Movement >= l_Distance)
            {
                m_AttachedObject = true;
                m_ObjectAttached.MovePosition(m_AttachingPosition.position);
                m_ObjectAttached.MoveRotation(Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z));
            }
            else
            {
                l_Direction /= l_Distance;
                m_ObjectAttached.MovePosition(m_ObjectAttached.transform.position + l_Direction * l_Movement);
                m_ObjectAttached.MoveRotation(Quaternion.Lerp(m_AttachingObjectStartRotation, Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z), 1.0f - Mathf.Min(l_Distance / 1.5f, 1.0f)));
            }
        }
        else
        {
            m_ObjectAttached.MoveRotation(Quaternion.Euler(0.0f, l_EulerAngles.y, l_EulerAngles.z));
            m_ObjectAttached.MovePosition(m_AttachingPosition.position);
        }
    }


    void ExpulseObject(float Force)
    {
        m_AttachedObject = false;
        m_AttachingObject = false;
        m_ObjectAttached.isKinematic = false;

        if (CompanionTarget)
            m_ObjectAttached.GetComponent<Companion>().SetTeleport(true);
        //else if (turret)
        //    m_ObjectAttached.GetComponent<Turret>().m_LineRenderer.enabled = false;


        m_ObjectAttached.AddForce(m_AttachingPosition.forward * Force);

        CompanionTarget = null;
        turret = null;
        canShoot = false;

    }

    void DetachObject()
    {
        m_AttachedObject = false;
        m_AttachingObject = false;
        m_ObjectAttached.isKinematic = false;
        CompanionTarget = null;
        turret = null;
        canShoot = false;

    }

    //resizing portal

    void ResizingPortal(GameObject portal)
    {
        if (portal.transform.localScale == scale * 0.5f && Input.GetAxis("Mouse ScrollWheel") == 0.1f)
        {
            portal.transform.localScale = scale;
        }
        else if (portal.transform.localScale == scale * 1.5f && Input.GetAxis("Mouse ScrollWheel") == -0.1f)
        {
            portal.transform.localScale = scale;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") == 0.1f)
        {
            portal.transform.localScale = scale * 0.5f;

        }
        else if (Input.GetAxis("Mouse ScrollWheel") == -0.1f)
        {
            portal.transform.localScale = scale * 1.5f;

        }


    }
}
