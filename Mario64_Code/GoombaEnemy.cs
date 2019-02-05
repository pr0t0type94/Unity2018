using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoombaEnemy : MonoBehaviour {

    // Use this for initialization

    Animator anim;

    public float minDistanceToAlert;
    public PlayerController playerController;
    public float minDistanceToAttack;

    public float damageRate;
    private float nextTimeToFire = 0.0f;

    public float impulseSpeed;

    public bool startHitMarioTimer;
    public float moveHitTimer = 1f;

    public AudioSource alertSource;
    public AudioSource runSource;
    public AudioSource walkSource;
    public AudioSource dieSource;

    NavMeshAgent m_NavMeshAgent;
    public List<Transform> m_PatrolPositions;

    float m_CurrentTime = 0.0f;
    int m_CurrentPatrolPositionId = -1;

    public GameController gameController;
    public RestartGame restartController;
    public Vector3 startingPosition;

    public enum TState
    {
        WALK = 0,     
        ALERT,
        RUN,
        ATTACK,
        DIE
    }

    public TState state;

    void Start () {
        anim = GetComponent<Animator>();
        state = TState.WALK;
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
        startingPosition = transform.position;
        restartController.addEnemyToList(this.gameObject);

    }

    // Update is called once per frame
    void Update () {

        m_CurrentTime += Time.deltaTime;

        switch (state)
        {
            case TState.WALK:
                UpdateWalkState();
                break;
            case TState.ALERT:
                UpdateAlertState();
                break;
            case TState.RUN:
                UpdateRunState();
                break;
            case TState.ATTACK:
                UpdateAttackState();
                break;
            case TState.DIE:
                UpdateDieState();
                break;
        }

        if(startHitMarioTimer)
        {
            playerController.canAttack = false;
            moveHitTimer -= Time.deltaTime;
            if (moveHitTimer > 0)
            {

                hitMarioMove();
            }
            else
            {
                startHitMarioTimer = false;
                moveHitTimer = 1f;
            }
        }
        else
        {
            playerController.canAttack = true;
        }
	}

    private float GetSqrDistanceXZToPosition(Vector3 pos)
    {
        Vector3 dirVector = pos - transform.position;
        dirVector.y = 0f;
        //Debug.Log(dirVector.sqrMagnitude);
        return dirVector.sqrMagnitude;
    }

    bool HearsPlayer()
    {

        return GetSqrDistanceXZToPosition(playerController.transform.position) < (minDistanceToAlert * minDistanceToAlert);

    }

    int GetClosestPatrolPositionId()
    {
        int patrolpos = 0;

        for (int i = 0; i < m_PatrolPositions.Count; i++)
        {
            var DistanceToPatrolPos = Vector3.Distance(transform.position, m_PatrolPositions[i].position);
            float MinDistanceSoFar = 0;
            if (DistanceToPatrolPos < MinDistanceSoFar)
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


    void UpdateWalkState()
    {
        if (!m_NavMeshAgent.hasPath && m_NavMeshAgent.pathStatus == NavMeshPathStatus.PathComplete)
            MoveToNextPatrolPosition();

        if (HearsPlayer())
        {
            setAlertState();
        }
    }
    void UpdateAlertState()
    {

        setRunState();
    }
    void UpdateRunState()
    {
        ChasePlayer();

        
        //do damage to player if on distance
    }
    void UpdateAttackState()
    {
        if (Time.time >= nextTimeToFire && playerController.OnGround)
        {
            nextTimeToFire = Time.time + 1f / damageRate;
            EnemyShoot();
        }
        //Debug.Log(GetSqrDistanceXZToPosition(controller.transform.position));
        Vector3 l_Destination = playerController.transform.position - transform.position;
        float l_Distance = l_Destination.magnitude;


        if (/*GetSqrDistanceXZToPosition(playerController.transform.position)*/l_Distance >= minDistanceToAttack)
        {
            setRunState();
        }
    }
    void UpdateDieState()
    {
        //play die animation
        dieSource.Play();

    }
    public void setWalkState()
    {
        state = TState.WALK;
        m_CurrentTime = 0.0f;
        m_CurrentPatrolPositionId = GetClosestPatrolPositionId();
        m_NavMeshAgent.isStopped = false;
        m_NavMeshAgent.SetDestination(m_PatrolPositions[m_CurrentPatrolPositionId].position);
    }
    void setAlertState()
    {
        anim.SetTrigger("Alert");
        state = TState.ALERT;
    }
    void setRunState()
    {
        state = TState.RUN;
    }
    void setAttackState()
    {
        state = TState.ATTACK;
    }
    void setDieState()
    {
        state = TState.DIE;
    }
    
    void ChasePlayer()
    {
        
        m_NavMeshAgent.isStopped = false;
        Vector3 l_Destination = playerController.transform.position - transform.position;
        float l_Distance = l_Destination.magnitude;
        l_Destination /= l_Distance;
        l_Destination = transform.position + l_Destination * (l_Distance - minDistanceToAttack);
        m_NavMeshAgent.SetDestination(l_Destination);

        //Debug.Log(l_Distance);
        if (l_Distance <= minDistanceToAttack+0.000001f)
        {
            setAttackState();
        }       
       
    }

    void EnemyShoot()
    {
        playerController.takeDamage();
        startHitMarioTimer = true;        
        
    }
    public void hitMarioMove()
    {
        
        CharacterController cc = playerController.gameObject.GetComponent<CharacterController>();
   
        cc.Move(transform.forward * impulseSpeed * Time.deltaTime);

        gameController.ShowHUD();


    }
    public void Kill()
    {
        anim.SetTrigger("Killed");
        dieSource.Play();
        StartCoroutine(killEnemy());

    }
    IEnumerator killEnemy()
    {
        yield return new WaitForSeconds(.5f);
        gameObject.SetActive(false);
    }
    /// <summary>
    /// ANIMATION EVENTS
    /// </summary>
    /// <param name="param"></param>
    public void Alert(string param)
    {
        if (param == "Alert")
        {
            alertSource.Play();
        }
    }
    public void Die(string param)
    {
        if(param =="Die")
        {
            dieSource.Play();
        }
    }
    public void Run(string param)
    {
        if (param == "Run")
        {
            runSource.Play();
        }
    }
    public void Walk(string param)
    {
        if (param == "Walk")
        {
            walkSource.Play();
        }
    }
}
