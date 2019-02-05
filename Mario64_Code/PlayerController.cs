using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private CharacterController controller;

    public Animator m_Animator;
    public Camera m_CameraController;

    public KeyCode m_UpKeyCode;
    public KeyCode m_DownKeyCode;
    public KeyCode LeftKeyCode;
    public KeyCode RightKeyCode;
    public KeyCode JumpKeyCode;
    public KeyCode RunKeyCode;
    public KeyCode punchKeyCode;

    private bool l_HasMovement;
    public float l_Speed;
    public float m_RunSpeed;
    private float iniSpeed;

    public float JumpSpeed;

    private float verticalSpeed;

    public bool OnGround;

    private GameObject m_CurrentPlatform;

    public float m_BridgeForce;

    public ParticleSystem runParticles;
    public ParticleSystem jumpParticles;

    public float jumpTimer = 3f;
    private bool startjumpTimer;
    public float doubleJumpTimer =4f;
    private bool startDoubleJumpTimer;

    public float m_JumpOverEnemySpeed=2f;
    private bool canKillWithFeet;    

    public float punchTimer = 3f;
    private bool startPunchTimer;
    public float doublePunchTimer = 3f;
    private bool startDoublePunchTimer;

    public Collider punchRightCollider;
    public Collider punch2LeftCollider;
    public Collider kickCollider;
    public AudioSource coinsSource;
    public int currentCoins;

    public AudioSource stepSource;
    public AudioSource runSource;
    public AudioSource jumpSource;
    public AudioSource doubleJumpSource;
    public AudioSource tripleJumpSource;
    public AudioSource longJumpSource;
    public AudioSource punch1;
    public AudioSource punch2;
    public AudioSource punch3;
    public AudioSource hitsource;
    public AudioSource dieSource;
    public AudioSource landSource;

    private Vector3 iniPlatformRotation;

    public GameController gameControler;

    public AudioSource starSource;

    private bool startLongJumpTimer;
    public float longJumpTimer;
    public float longJumpImpulseForce;
    private bool onWall=false;
    private bool startWallTimer;
    public float wallTimer=1f;

    public bool canMove;
    public bool canAttack;

    CollisionFlags l_CollisionFlags;


    public Transform m_AttachingPosition;
    public bool m_AttachedObject;
    private Rigidbody m_ObjectAttached;
    public float m_AttachingObjectSpeed;
    private Quaternion m_AttachingObjectStartRotation;
    private bool m_AttachingObject;
    public float impulseForce = 50f;
    public bool hasShell = false;

    public RestartGame resetController;

    private bool onLava;

    public Vector3 respawnPosition;

    // Use this for initialization
    void Start () {
        controller = gameObject.GetComponent<CharacterController>();
        iniSpeed = l_Speed;
        punchRightCollider.enabled = false;
        punch2LeftCollider.enabled = false;
        kickCollider.enabled = false;
        canMove = true;
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //..
        Vector3 l_Movement = Vector3.zero;
        Vector3 l_Forward = m_CameraController.transform.forward;
        Vector3 l_Right = m_CameraController.transform.right;
        l_Forward.y = 0.0f;
        l_Forward.Normalize();
        l_Right.y = 0.0f;
        l_Right.Normalize();
        if (Input.GetKey(m_UpKeyCode))
            l_Movement = l_Forward;
        else if (Input.GetKey(m_DownKeyCode))
            l_Movement = -l_Forward;
        if (Input.GetKey(LeftKeyCode))
            l_Movement = -l_Right;
        else if (Input.GetKey(RightKeyCode))
            l_Movement = l_Right;

        /////////////
        ///
        if (m_AttachedObject)
        {
            canAttack = false;
            m_ObjectAttached.isKinematic = true;
            UpdateAttachedObject();
            m_ObjectAttached.GetComponent<SphereCollider>().enabled = false;
            l_Speed /= 2;
            hasShell = true;
        }
        if (hasShell)
        {
            if(Input.GetKeyDown(KeyCode.Mouse1))
            DetachObject(impulseForce);
        }



        //transform.LookAt(transform.forward);
        //PUNCH

        if (Input.GetKeyDown(KeyCode.Mouse0) && canAttack)
        {
            m_Animator.SetTrigger("Punch1");

            startPunchTimer = true;
        }
        if (startPunchTimer)
        {

            punchTimer -= Time.deltaTime;

            if (punchTimer <= 0)
            {
                startPunchTimer = false;
                punchTimer = 1f;
            }
            else if (punchTimer <= 0.9f && punchTimer >= 0)
            {
                if (OnGround && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Animator.ResetTrigger("Punch1");
                    m_Animator.SetTrigger("Punch2");

                    startDoublePunchTimer = true;
                }
            }
        }
        if (startDoublePunchTimer)
        {

            doublePunchTimer -= Time.deltaTime;

            if (doublePunchTimer <= 0)
            {
                startDoublePunchTimer = false;
                doublePunchTimer = 1f;
            }
            else if (doublePunchTimer <= 0.9f && doublePunchTimer >= 0)
            {
                if (OnGround && Input.GetKeyDown(KeyCode.Mouse0))
                {
                    m_Animator.ResetTrigger("Punch1");
                    m_Animator.ResetTrigger("Punch2");
                    m_Animator.SetTrigger("Punch3");

                }
            }
        }

        //JUMP
        if (OnGround && Input.GetKeyDown(JumpKeyCode) && !startDoubleJumpTimer)
        {
            //ParticleSystem sys = Instantiate(jumpParticles, gameObject.transform.position, Quaternion.identity);
            //sys.Play();
            m_Animator.SetTrigger("Jump");
            verticalSpeed = JumpSpeed;
            startjumpTimer = true;
        }

        if (startjumpTimer)
        {

            jumpTimer -= Time.deltaTime;

            if (jumpTimer <= 0)
            {
                startjumpTimer = false;
                jumpTimer = 1.5f;
            }
            else if (jumpTimer <= 0.9f && jumpTimer >= 0)
            {
                if (OnGround && Input.GetKeyDown(KeyCode.Space))
                {
                    verticalSpeed = JumpSpeed * 1.5f;
                    m_Animator.ResetTrigger("Jump");

                    m_Animator.SetTrigger("DoubleJump");

                    startDoubleJumpTimer = true;
                }
            }
        }


        if (startDoubleJumpTimer)
        {
            doubleJumpTimer -= Time.deltaTime;

            if (doubleJumpTimer <= 0)
            {
                startDoubleJumpTimer = false;
                doubleJumpTimer = 2f;
            }
            else if (doubleJumpTimer <= 0.9f && doubleJumpTimer >= 0)
            {
                if (OnGround && Input.GetKeyDown(KeyCode.Space))
                {
                    verticalSpeed = JumpSpeed * 1.7f;
                    m_Animator.SetTrigger("TripleJump");

                }
            }
        }

        //RUN
        if (OnGround && Input.GetKey(RunKeyCode) && !hasShell)
        {
            l_Speed = m_RunSpeed;
            if (Input.GetKeyDown(KeyCode.Mouse1) && OnGround)
            {
                m_Animator.SetTrigger("LongJump");
                startLongJumpTimer = true;
            }
        }
        else
        {
            l_Speed = iniSpeed;
        }
        //if (Input.GetKeyDown(RunKeyCode))
        //    runParticles.Play();

        ///LONGJUMP
        ///

        if (startLongJumpTimer)
        {
            longJumpTimer -= Time.deltaTime;

            if (longJumpTimer <= 0)
            {
                startLongJumpTimer = false;
                longJumpTimer = 1f;
                canMove = true;
            }
            else
            {
                canMove = false;
                verticalSpeed = JumpSpeed / 6;
                controller.Move(transform.forward * longJumpImpulseForce * Time.deltaTime);
                controller.Move(transform.up * verticalSpeed * Time.deltaTime);

            }


        }




        //movement normaliz
        l_Movement.Normalize();

        l_Movement *= Time.deltaTime * l_Speed;

        //hasMovement
        if (l_Movement != Vector3.zero)
        {
            l_HasMovement = true;

            Quaternion newRotation = Quaternion.LookRotation(new Vector3(l_Movement.x, 0, l_Movement.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, 4 * Time.deltaTime);
            //transform.forward = m_CameraController.transform.forward;
        }
        else
        {
            l_HasMovement = false;

        }
        //GRAVITY
        if (OnGround && verticalSpeed <= 0)
        {
            verticalSpeed = -controller.stepOffset / Time.deltaTime;
        }
        else
        {
      
                verticalSpeed += Physics.gravity.y * Time.deltaTime;       

        }


        l_Movement.y = verticalSpeed * Time.deltaTime;

        //CollisionFlags + controller Move
        if (canMove)
        {
            l_CollisionFlags = controller.Move(l_Movement);

        }

        if ((l_CollisionFlags & CollisionFlags.Below) != 0)
        {
            OnGround = true;
            verticalSpeed = 0.0f;
            m_Animator.SetBool("OnGround", true);
            m_Animator.ResetTrigger("Fall");
        }
        else
        {
            OnGround = false;
            m_Animator.SetBool("OnGround", false);

        }

        if ((l_CollisionFlags & CollisionFlags.Above) != 0 && verticalSpeed > 0.0f)
            verticalSpeed = 0.0f;

        if ((l_CollisionFlags & CollisionFlags.Sides) != 0 && !OnGround)
        {

            m_Animator.SetTrigger("Wall");
            onWall = true;
            startWallTimer = true;
        }
        if(startWallTimer)
        {
            wallTimer -= Time.deltaTime;
            if(wallTimer <=0)
            {
                startWallTimer = false;
                wallTimer = 1f;
            }
            else if(wallTimer<=0.99f && wallTimer > 0f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    m_Animator.SetTrigger("WallJump");
                    controller.Move(JumpSpeed * transform.forward * Time.deltaTime);
                    Debug.Log("wallJUMP");
                    
                }
            }
           
        }

        m_Animator.SetFloat("Speed", l_HasMovement ? (l_Speed == m_RunSpeed ? 1.0f : 0.2f) : 0.0f);

        UpdatePlatform();


        if (!OnGround && verticalSpeed < -3f && !startjumpTimer && !startDoubleJumpTimer && !onLava)
        {
            m_Animator.SetTrigger("Fall");
        }

        
    }

    //PLATFORMS
    void UpdatePlatform()
    {
        if (m_CurrentPlatform != null)
        {
            Debug.Log(Mathf.Abs(Vector3.Dot(m_CurrentPlatform.transform.forward, Vector3.up)));

            transform.parent = m_CurrentPlatform.transform;

            transform.eulerAngles = iniPlatformRotation;

            if (Mathf.Abs(Vector3.Dot(m_CurrentPlatform.transform.forward, Vector3.up)) < 0.95f)
                DetachPlatform();
            //Check with dot if current platform is looking up, if not detach platform
        }
    }
    void AttachPlatform(Transform platf)
    {
        iniPlatformRotation = transform.eulerAngles;
        m_CurrentPlatform = platf.gameObject;

    }
    void DetachPlatform()
    {
        m_CurrentPlatform = null;
        transform.parent = null;
    }

    //TRIGGERS
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Platform" && m_CurrentPlatform == null)
        {

            if (Mathf.Abs(Vector3.Dot(other.transform.forward, Vector3.up)) > 0.95f)
                AttachPlatform(other.transform); 
            //Set CurrentPlatform and set parent mario to platform

        }

        if (other.tag == "Goomba")
        {
            
            if(!OnGround)
            {
                other.GetComponent<GoombaEnemy>().Kill();
                JumpOverEnemy();
            }
            
            else
            {
                if(punchRightCollider.enabled ==true || punch2LeftCollider.enabled == true || kickCollider.enabled == true)
                {
                    other.GetComponent<GoombaEnemy>().Kill();
                }
            }
        }
        if (other.tag == "Koopa")
        {

            if (!OnGround)
            {
                other.GetComponent<KoopaEnemy>().Kill();
                JumpOverEnemy();
            }

            else
            {
                if (punchRightCollider.enabled == true || punch2LeftCollider.enabled == true || kickCollider.enabled == true)
                {
                    other.GetComponent<KoopaEnemy>().Kill();
                }
            }
        }

        if (other.tag == "Coin")
        {
            currentCoins++;
            coinsSource.Play();
            Destroy(other.gameObject);
        }

        if(other.tag =="PowerStar")
        {
            if(gameControler.counter != 8)
            {
                starSource.Play();
                gameControler.starRecolected();
                gameControler.ShowHUD();
                Destroy(other.gameObject);
            }
        }

        if (other.gameObject.tag == "DeadZone")
        {
            Debug.Log("enter dead zone");
            onLava = true;
            gameControler.KillPlayer();
        }

        if (other.gameObject.tag == "Checkpoint")
        {
            Debug.Log("chekpoint enter");
            other.gameObject.GetComponent<ChekpointPosition>().source.Play();
            respawnPosition = other.gameObject.GetComponent<ChekpointPosition>().respawnPosition.transform.position;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.tag == "Platform" && m_CurrentPlatform != null)
            DetachPlatform();
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Shell")
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                m_AttachedObject = true;
                m_ObjectAttached = other.GetComponent<Rigidbody>();
                m_Animator.SetBool("hasShell", true);
            }
        }
    }
    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.tag == "Bridge")
        {
            Rigidbody l_Bridge = hit.collider.attachedRigidbody;
            l_Bridge.AddForceAtPosition(-hit.normal * m_BridgeForce, hit.point);
        }   
        
        if(hit.collider.tag =="Shell")
        {
            //Rigidbody rb = hit.collider.gameObject.GetComponent<Rigidbody>();
            //rb.AddForce(transform.forward * impulseForce*50);

            if (hit.gameObject.GetComponent<Shell>().startMovement == true && !hasShell && hit.gameObject.GetComponent<SphereCollider>().enabled==true)
            {
                m_Animator.SetTrigger("Hit");
                gameControler.counter -= 1f;
                Destroy(hit.gameObject);
            }
            else
            {
          
                hit.collider.gameObject.GetComponent<Shell>().startMoving(transform.forward);
              
            }
        }
    }
    ////////////////UPDATE ATTACHED OBJECT SHELL
    ///

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

    void DetachObject(float Force)
    {
        m_ObjectAttached.isKinematic = false;
        m_ObjectAttached.GetComponent<Shell>().startMoving(transform.forward);
        m_AttachedObject = false;
        m_AttachingObject = false;

        hasShell = false;
        //m_ObjectAttached.AddForce(m_AttachingPosition.forward * Force);
        m_Animator.SetBool("hasShell", false);
        punch3.Play();


        m_ObjectAttached = null;
    }


    /// <summary>
    /// ////////////EXTRAS
    /// </summary>
    public void JumpOverEnemy()
    {
        verticalSpeed = m_JumpOverEnemySpeed;
    }

    public void takeDamage()
    {
        m_Animator.SetTrigger("Hit");
        gameControler.UpdateLives();
    }


    public void playerDie()
    {

    }
    public int GetCoins()
    {
        return currentCoins;
    }

    //PUNCH CONTROLLERS
    public void EnableLeftHandPunch(bool enable)
    {
        if (enable == true)       
            punchRightCollider.enabled = true;       
        else        
            punchRightCollider.enabled = false;
        
    }
    public void EnableRightHandPunch(bool enable)
    {
        if (enable == true)       
            punch2LeftCollider.enabled = true;
        else
            punch2LeftCollider.enabled = false;
    }
    public void EnableKickAttack(bool enable)
    {
        if (enable == true)
            kickCollider.enabled = true;
        else
            kickCollider.enabled = false;
    }

    

    //ANIMATION EVENTS
    /// <summary>
    /// //
    /// </summary>
    /// <param name="stringParameter"></param>
    public void Step(string stringParameter)
    {
        //Code
        if(stringParameter == "Step")
        {
            stepSource.Play();
        }
    }

    public void Run()
    {
        runSource.Play();
    }

    public void Jump1(string stringParameter)
    {
        if (stringParameter == "Jump")
        {
            jumpSource.Play();
        }
    }
    public void Jump2(string stringParameter)
    {
        if (stringParameter == "Jump")
        {
            doubleJumpSource.Play();
        }
    }
    public void Jump3(string stringParameter)
    {
        if (stringParameter == "Jump")
        {
            tripleJumpSource.Play();
        }
    }
    public void Punch1(string stringParameter)
    {
        if (stringParameter == "Punch")
        {
            punch2.Play();
        }
    }
    public void Punch2(string stringParameter)
    {
        if (stringParameter == "Punch")
        {
            punch2.Play();
        }
    }
    public void Punch3(string stringParameter)
    {
        if (stringParameter == "Punch")
        {
            punch3.Play();
        }
    }
    public void Hit(string stringParameter)
    {
        if (stringParameter == "Hit")
        {
            hitsource.Play();
        }
    }
    public void Die(string stringParameter)
    {
        if (stringParameter == "Die")
        {
            dieSource.Play();
        }
    }
    public void Land(string stringParameter)
    {
        if (stringParameter == "Land")
        {
            landSource.Play();
        }
    }
    public void Longjump(string stgParam)
    {
        if(stgParam=="Jump")
        {
            longJumpSource.Play();
        }
    }

   

}//end of cls

