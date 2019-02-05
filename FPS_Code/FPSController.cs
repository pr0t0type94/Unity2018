using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FPSController : MonoBehaviour {

	public float m_YawRotationalSpeed=360.0f;
	public float m_PitchRotationalSpeed=180.0f;
	public float m_MinPitch=-80.0f;
	public float m_MaxPitch=50.0f;
	public Transform m_PitchControllerTransform;
	public bool m_InvertedYaw=false;
	public bool m_InvertedPitch=true;
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
    public KeyCode Dash_key = KeyCode.LeftAlt;
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

    // Use this for initialization

    public GameObject flashLight;
    public AudioSource flashLightSound;

    public float l_SpeedMultiplier = 1f;

    public AudioSource jumpSound;
     
    public Animator GunWalkAnimation;
    private bool walking;


    public float maxDashTime = 1.0f;
    public float dashSpeed = 1.0f;
    public float dashStoppingSpeed = 0.1f;
    private float currentDashTime;
    public float maxStamina;
    public float currentStamina;
    public Image staminaImage;
    public float staminaRecoveryMultiplier;
    public AudioSource DashSound;

    public GameController gc;
    
    void Start()
    {
        currentDashTime = maxDashTime;
        currentStamina = maxStamina;
        staminaImage.fillAmount = currentStamina / 100;

    }


    void Awake()
    {
        m_Yaw = transform.rotation.eulerAngles.y;
        m_Pitch = m_PitchControllerTransform.localRotation.eulerAngles.x;

        m_CharacterController = GetComponent<CharacterController>();

        
    }
    void Update()
    {
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

        ////DASHH 
        Vector3 moveDirection;
        if (Input.GetKeyDown(Dash_key) && currentStamina >=25)
        {
            currentDashTime = 0.0f;
            DashDone();
        }
        if (currentDashTime < maxDashTime)
        {
            moveDirection = new Vector3(l_Movement.x * dashSpeed, l_Movement.y * dashSpeed, l_Movement.z * dashSpeed);
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
            GunWalkAnimation.SetBool("Dash", false);

        }

        m_CharacterController.Move(moveDirection * Time.deltaTime);


        l_Movement.Normalize();
        l_Movement *= Time.deltaTime * m_Speed;

        
        //…




        if (walking)
        {
            GunWalkAnimation.SetBool("Walking", true);
        }
        else

            GunWalkAnimation.SetBool("Walking", false);

        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_FastSpeedMultiplier;
        else
            l_SpeedMultiplier = 1f;
        //…
        l_Movement *= Time.deltaTime * m_Speed * l_SpeedMultiplier;


        //////////GRAVEDAD
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

        //////////SALTO Y CORRER
        ///
        //…

       

        //…
        if (m_OnGround && Input.GetKeyDown(m_JumpKeyCode))
        {
            m_VerticalSpeed = m_JumpSpeed;
            jumpSound.PlayOneShot(jumpSound.clip);
        }


        //////////FLASHLIGHT

        if (Input.GetKeyDown(FlashLight_key))
        {
            flashLightSound.Play();
            if (flashLight.activeInHierarchy)
                flashLight.SetActive(false);
            else
                flashLight.SetActive(true);
            //playSound
        }


        UpdateStamina();



    }

    private void DashDone()
    {
        //play sound
        DashSound.Play();
        GunWalkAnimation.SetBool("Dash", true);
        currentStamina -= 25f;
        staminaImage.fillAmount = currentStamina / 100;

    }
    private void UpdateStamina()
    {

        currentStamina += Time.deltaTime * staminaRecoveryMultiplier;
        if (currentStamina >= maxStamina)
            currentStamina = maxStamina;
        staminaImage.fillAmount = currentStamina / 100;


    }

    public void CollectedKey()
    {
        
    }

    void KillPlayer()
    {
        gc.PlayerDie();
        //Podemo reiniciar el juego utilizando una corutina o una función lambda cuando terminemos de mostrar al jugador que ha muerto
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "DeadZone")
        {
            KillPlayer();
        }

        else if (other.gameObject.tag == "Teleporter")
            Application.Quit();

        else if(other.gameObject.tag == "Elevator")
        {
            Animator elevatorAnim = other.gameObject.GetComponent<Animator>();
            elevatorAnim.SetBool("moveUp",true);
        }

    }

    


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Platform")
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
        else if (other.tag == "Elevator")
        {
            Animator elevatorAnim = other.gameObject.GetComponent<Animator>();
            elevatorAnim.SetBool("moveUp", false);
        }
    }

    
}
