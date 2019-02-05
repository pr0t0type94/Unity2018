using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPS_CharacterController : MonoBehaviour
{



    public float m_YawRotationalSpeed = 360.0f;
    public float m_PitchRotationalSpeed = 180.0f;
    public float m_MinPitch = -80.0f;
    public float m_MaxPitch = 50.0f;
    public Transform m_PitchControllerTransform;
    public bool m_InvertedYaw = false;
    public bool m_InvertedPitch = true;
    float m_Yaw;
    float m_Pitch;
    /// ////
    private CharacterController m_CharacterController;
    public float m_Speed = 10.0f;
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode FlashLight_key = KeyCode.F;
    Vector3 l_Movement;

    ///
    /// 
    /// 

    float m_VerticalSpeed = 0.0f;
    bool m_OnGround = false;
    /// 
    /// 
    /// 
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public float m_FastSpeedMultiplier;
    public float m_JumpSpeed;

    public float l_SpeedMultiplier = 1f;

    private bool walking;

    public GameObject BluePortalGc;
    public GameObject OrangePortalGc;
    public LevelController gc;
    /// <summary>
    /// ///
    /// </summary>
    /// 

    private bool canTeleport;
    float countDownToTeleportAgain;
    float iniTimer = .1f;
    bool startTimer;
    bool teleportDone;

    public AudioSource teleportSource;

    void Start()
    {
        countDownToTeleportAgain = iniTimer;
        canTeleport = true;
    }

    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        m_CharacterController = GetComponent<CharacterController>();


    }
    void Update()
    {
        /////TELEPORT 
        ///
        if (canTeleport == false)
        {
            countDownToTeleportAgain -= Time.deltaTime;
            if (countDownToTeleportAgain <= 0)
            {
                canTeleport = true;
                countDownToTeleportAgain = iniTimer;
            }
        }

        ///////////////ROTATION

        float l_MouseAxisX = Input.GetAxis("Mouse X");

        if (m_InvertedYaw)
            m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;
        else
            m_Yaw += l_MouseAxisX * m_YawRotationalSpeed * Time.deltaTime;


        float l_MouseAxisY = Input.GetAxis("Mouse Y");

        if (m_InvertedPitch)
            m_Pitch += l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;
        else
            m_Pitch -= l_MouseAxisY * m_PitchRotationalSpeed * Time.deltaTime;

        m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
        //…
        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchControllerTransform.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);
        //…


        ///////////MOVEMENT

        float l_YawInRadians = m_Yaw * Mathf.Deg2Rad;
        float l_Yaw90InRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;
        Vector3 l_Forward = new Vector3(Mathf.Sin(l_YawInRadians), 0.0f, Mathf.Cos(l_YawInRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_Yaw90InRadians), 0.0f, Mathf.Cos(l_Yaw90InRadians));
        Vector3 l_Movement = Vector3.zero;
        if (Input.GetKey(m_UpKeyCode))
        {
            l_Movement = l_Forward;
        }
        else if (Input.GetKey(m_DownKeyCode))

            l_Movement = -l_Forward;

        if (Input.GetKey(m_RightKeyCode))
            l_Movement += l_Right;
        else if (Input.GetKey(m_LeftKeyCode))
            l_Movement -= l_Right;


        l_Movement.Normalize();
        l_Movement *= Time.deltaTime * m_Speed;

        //…



        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_FastSpeedMultiplier;
        else
            l_SpeedMultiplier = 1f;
        //…
        l_Movement *= Time.deltaTime * m_Speed * l_SpeedMultiplier;


        //////////GRAVITY
        ///
        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;
        l_Movement.y = m_VerticalSpeed * Time.deltaTime;



        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_Movement);
        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            m_OnGround = true;
            m_VerticalSpeed = 0.0f;
        }
        else
            m_OnGround = false;

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f)
            m_VerticalSpeed = 0.0f;

        //////////JUMP
        ///
        //…

        //…
        if (m_OnGround && Input.GetKeyDown(m_JumpKeyCode))
        {
            m_VerticalSpeed = m_JumpSpeed;
        }





        //end of update
    }
    public void Teleport(Portal _Portal)
    {
        if (canTeleport)
        {

            Vector3 l_Position = _Portal.transform.InverseTransformPoint(transform.position);
            transform.position = _Portal.m_MirrorPortal.transform.TransformPoint(l_Position);
            Vector3 l_Direction = _Portal.transform.InverseTransformDirection(-transform.forward);
            transform.forward = _Portal.m_MirrorPortal.transform.TransformDirection(l_Direction);

            //Rigidbody l_Rigidbody = GetComponent<Rigidbody>();
            //Vector3 l_Velocity = _Portal.transform.InverseTransformDirection(-l_Rigidbody.velocity);
            //l_Rigidbody.velocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_Velocity);
            teleportSource.Play();

            m_Yaw = transform.rotation.eulerAngles.y;
            canTeleport = false;

        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BluePortal")
        {
            Portal blue_portal = other.GetComponent<Portal>();
            Teleport(blue_portal);
            Debug.Log("blue portal passed");
        }
        if (other.gameObject.tag == "OrangePortal")
        {
            Portal org_portal = other.GetComponent<Portal>();
            Teleport(org_portal);
        }

        if(other.gameObject.tag == "KillingZone")
        {
            gc.KillPlayer();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "CSpawner")
        {
            CompanionSpawner spwner = other.gameObject.GetComponent<CompanionSpawner>();
            if (Input.GetKeyDown(KeyCode.F))
            {
                spwner.Spawn();
            }
        }

        if (other.tag == "Platform")
        {
            gameObject.transform.parent = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform")
        {
            transform.parent = null;
        }


    }

        //end of class
}