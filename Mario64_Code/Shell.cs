using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell : MonoBehaviour
{

    // Use this for initialization
    public bool startMovement = false;
    public float speed = 5f;
    private float iniSpeed;
    public Vector3 direction;
    private Vector3 directionToMove;
    Vector3 velocity;
    Rigidbody rb;
    public PlayerController controller;


    private bool startColliderTimer = false;
    private float colliderTimer = 0.2f;
    private SphereCollider collider;
    void Start()
    {
        iniSpeed = speed;
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<SphereCollider>();
    }


    // Update is called once per frame
    void Update()
    {
        if(direction==Vector3.zero)
        {

        }

        if (startMovement)
        {
            //transform.position += directionToMoveMethod() * speed * Time.deltaTime;

            rb.MovePosition(transform.position + directionToMoveMethod() * speed * Time.deltaTime);
            speed -= Time.deltaTime/4;

            startColliderTimer = true;
            
        }

        if(startColliderTimer)
        {
            colliderTimer -= Time.deltaTime;

            if(colliderTimer <=0)
            {
                collider.enabled = true;
                startColliderTimer = false;
                colliderTimer = 0.2f;
            }
        }
        
    }

    public void startMoving(Vector3 direction)
    {
        directionToMove = direction;
        startMovement = true;
        speed = iniSpeed;
        transform.forward = direction;
    }

    Vector3 directionToMoveMethod()
    {
        return directionToMove;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Goomba")
        {
            controller.m_AttachedObject = false;
            controller.hasShell = false;
            controller.m_Animator.SetBool("hasShell", false);
            other.gameObject.GetComponent<GoombaEnemy>().Kill();
            Destroy(gameObject);

        }
        if (other.gameObject.tag == "Koopa")
        {
            controller.m_AttachedObject = false;
            controller.m_Animator.SetBool("hasShell", false);
            controller.hasShell = false;

            other.gameObject.GetComponent<KoopaEnemy>().Kill();
            Destroy(gameObject);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="MAP")
        {

            Vector3 normal = collision.contacts[0].normal;
            Vector3 vel = rb.velocity;
            // measure angle
            Debug.Log(Vector3.Angle(vel, -normal));
            if (Vector3.Angle(vel, -normal) > 40)
            {
                // bullet bounces off the surface
                rb.velocity = Vector3.Reflect(vel, normal);
            }


            //if (Vector3.Angle(velocity, -collision.contacts[0].normal) <= 40)
            //{
            //    directionToMove = Vector3.Reflect(velocity, collision.contacts[0].normal);

            //    //directionToMove = -directionToMove;

            //}


        }
    }
}
