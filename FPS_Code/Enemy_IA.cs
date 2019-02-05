using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Enemy_IA : MonoBehaviour {
    NavMeshAgent m_NavMeshAgent;
    public ParticleSystem particles;
    public Color NormalColor;
    public Color AttackColor;
    public Color ChaseColor;
    public Color AlertColor;
    private FPSController target;
    public float EnemyDamge = 150f;
    public float EnemyFireRate = 5f;
    
    public enum TState
    {
        IDLE = 0,
        PATROL,
        ALERT,
        CHASE,
        ATTACK,
        HIT,
        DIE
    }
    public TState m_State;
    public List<Transform> m_PatrolPositions;
    float m_CurrentTime = 0.0f;
    int m_CurrentPatrolPositionId = -1;
    float m_StartAlertRotation = 0.0f;
    float m_CurrentAlertRotation = 0.0f;
    public GameController m_GameController;
    public float m_MinDistanceToAlert = 5.0f;
    public LayerMask m_CollisionLayerMask;
    public float m_MinDistanceToAttack = 3.0f;
    public float m_MaxDistanceToAttack = 7.0f;
    public float m_MaxDistanceToChase = 100.0f;
    public float m_ConeAngle = 45.0f;
    public float m_LerpAttackRotation = 0.6f;
    const float m_MaxLife = 100.0f;
    float m_Life = m_MaxLife;
    [Range(0.0f, 1.0f)]
    public float m_ShootAccuracy = 0.8f;
    NavMeshObstacle nMeshObstacle;
    private ParticleSystem[] particlesArray;

    private float nextTimeToFire = 0.0f;
    public float fireRate;
    public float damage;

    /// <summary>
    /// //
    /// </summary>
    public ParticleSystem auraParticles;
    private ParticleSystem.MainModule main;
    private ParticleSystem.EmissionModule emModule;

    public float Idletimer = 5f;
    private bool startTimer;
    private float iniIdletimer;

    private bool soundplayed;

    public AudioSource alertSound;
    public AudioSource patrolSound;
    public AudioSource attackSound;
    public AudioSource dieSound;
    // Use this for initialization
    private void Awake()
    {
        main = auraParticles.main;
        emModule = auraParticles.emission;
        soundplayed = false;
    }
    private void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        particlesArray = particles.GetComponentsInChildren<ParticleSystem>();

        //for(int i = 0; i < particlesArray.Length;i++ )
        //{
        //    mainModuleArray[i] = particlesArray[i].main;
        //}
        startTimer = false;
        iniIdletimer = Idletimer;
    }

    void Update()
    {
        m_CurrentTime += Time.deltaTime;
        switch (m_State)
        {
            case TState.IDLE:
                UpdateIdleState();
                break;
            case TState.PATROL:
                UpdatePatrolState();
                break;
            case TState.ALERT:
                UpdateAlertState();
                break;
            case TState.CHASE:
                UpdateChaseState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.HIT:
                UpdateHitState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }


        UpdateParticlesSize();
    }
    public void SetIdleState()
    {
        m_State = TState.IDLE;
        startTimer = true;
    }

    void UpdateIdleState()
    {
        if(startTimer == true)
        {
            Idletimer -= Time.deltaTime;
            if (Idletimer <= 0)
            {
                SetPatrolState();
                startTimer = false;
                Idletimer = iniIdletimer;
            }
        }



    }
    void SetPatrolState()
    {
        m_State = TState.PATROL;
        m_CurrentTime = 0.0f;
        m_CurrentPatrolPositionId = GetClosestPatrolPositionId();
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);


    }
    int GetClosestPatrolPositionId()
    {
        int patrolpos = 0;

        for (int i = 0; i< m_PatrolPositions.Count; i++)
        {
            var DistanceToPatrolPos = Vector3.Distance(transform.position, m_PatrolPositions[i].position);
            float MinDistanceSoFar = 0;
            if(DistanceToPatrolPos < MinDistanceSoFar)
            {
                MinDistanceSoFar = DistanceToPatrolPos;
                patrolpos = m_PatrolPositions[i].GetInstanceID();
            }         
        }
        return patrolpos;

    }
    void MoveToNextPatrolPosition()
    {
        ++m_CurrentPatrolPositionId;
        if (m_CurrentPatrolPositionId >= m_PatrolPositions.Count)
            m_CurrentPatrolPositionId = 0;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);
    }
    bool SeesPlayer()
    {
        Vector3 l_Direction = (m_GameController.m_PlayerController.transform.position + Vector3.up * 0.9f) - transform.position;
        Ray l_Ray = new Ray(transform.position, l_Direction);
        float l_Distance = l_Direction.magnitude;
        l_Direction /= l_Distance;

        bool l_Collides = Physics.Raycast(l_Ray, l_Distance, m_CollisionLayerMask.value);
        Vector3 normalizedVect = new Vector3(transform.forward.x, 0, transform.forward.z);

        float l_DotAngle = Vector3.Dot(l_Direction, normalizedVect.normalized);
        //float l_DotAngle = Vector3.Dot(l_Direction, transform.forward);

        Debug.DrawRay(transform.position, l_Direction * l_Distance, l_Collides ? Color.red : Color.yellow);
        //Debug.Log(l_Collides);
        return !l_Collides && l_DotAngle > Mathf.Cos(m_ConeAngle * 0.5f * Mathf.Deg2Rad);
    }

    private float GetSqrDistanceXZToPosition(Vector3 pos)
    {
        Vector3 dirVector = pos - transform.position;
        dirVector.y = 0f;
        //Debug.Log(dirVector.magnitude);
        return dirVector.sqrMagnitude;
    }
    bool HearsPlayer()
    {
           
        return GetSqrDistanceXZToPosition(m_GameController.m_PlayerController.transform.position) < (m_MinDistanceToAlert * m_MinDistanceToAlert);

    }
    void UpdatePatrolState()
    {
        if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            MoveToNextPatrolPosition();
        //else if (m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
        //    MoveToNextPatrolPosition();
        
        if (HearsPlayer())
            SetAlertState();

        UpdateParticlesColor(NormalColor);

        //else if (HearsPlayer() && !SeesPlayer())
        //    SetPatrolState();
    }

    void SetNextChasePosition()
    {
        m_NavMeshAgent.isStopped = false;
        Vector3 l_Destination = m_GameController.m_PlayerController.transform.position - transform.position;
        float l_Distance = l_Destination.magnitude;
        l_Destination /= l_Distance;
        l_Destination = transform.position + l_Destination * (l_Distance - m_MinDistanceToAttack);
        m_NavMeshAgent.SetDestination(l_Destination);
    }

    void SetAlertState()
    {
        m_State = TState.ALERT;
        m_NavMeshAgent.isStopped = true;
        //Debug.Log("ALERT STATE");
    }
    void UpdateAlertState()
    {
    
        PlaySound(alertSound);
        UpdateParticlesColor(AlertColor);


        m_StartAlertRotation = transform.rotation.y;
        m_CurrentAlertRotation = 0;
        if (m_CurrentAlertRotation < 360)
        {
            Quaternion rotation = Quaternion.LookRotation(m_GameController.m_PlayerController.transform.position - transform.position).normalized;
            //Quaternion accumRotat = new Quaternion(0, m_StartAlertRotation- m_CurrentAlertRotation, 0,0);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * m_LerpAttackRotation);
            m_CurrentAlertRotation += rotation.eulerAngles.y;

            if(SeesPlayer())
            {
                m_State= TState.CHASE;
            }

        }   
        if(m_CurrentAlertRotation >= 360 && !SeesPlayer())
        {

                m_State = TState.PATROL;
        }
        
            
            

        //else
        //{
        //    m_State = TState.CHASE;

        //}
        
    }


   
    void UpdateChaseState()
    {
        Vector3 dirVector = m_GameController.m_PlayerController.transform.position - transform.position;
        dirVector.y = 0f;
        Debug.Log("CHASE STATE: "+dirVector.magnitude);

        //Debug.Log("CHASE SEES PLAYER: "+SeesPlayer());

        
        if (dirVector.magnitude > m_MaxDistanceToChase)
        {
            m_State = TState.PATROL;
        }
        
        else 
        {
            SetNextChasePosition();
            if(dirVector.magnitude<=m_MaxDistanceToAttack)
                m_State = TState.ATTACK;
        }

        UpdateParticlesColor(ChaseColor);

    
    }

    void UpdateAttackState()
    {
        m_NavMeshAgent.isStopped = true;
        Vector3 dirVector = m_GameController.m_PlayerController.transform.position - transform.position;
        dirVector.y = 0f;

        Debug.Log("Attack STATE: "+dirVector.magnitude);

        attackSound.Play();
        UpdateParticlesColor(AttackColor);

        if (dirVector.magnitude <= m_MaxDistanceToAttack)
        {
            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                EnemyShoot();
            }
        }
  
        else
        {
            m_State = TState.CHASE;
        }

        

    }
    void UpdateHitState()
    {

    }
    void UpdateDieState()
    {

    }

    //rayo entre el player y el enemigo
    //si estoy de espalda al player, no lo veo -> dot
    //
    
    void EnemyShoot()
    {
        m_GameController.TakeDamage(damage);
    }

    void UpdateParticlesColor(Color color)
    {
        foreach (ParticleSystem p in particlesArray)
        {
            ParticleSystem.MainModule newMain = p.main;
            newMain.startColor = color;
        }
    }

    void UpdateParticlesSize()
    {
        if (m_State== TState.ATTACK)
        {
            emModule.rateOverTime = 4f;
            main.startLifetime = 6f;
        }
        else
        {
            emModule.rateOverTime = 2f;
            main.startLifetime = 2f;
        }


    }

    void PlaySound(AudioSource source)
    {      
        if (!soundplayed)
        {
            source.PlayOneShot(source.clip);
            soundplayed = true;
        }

    }

}
