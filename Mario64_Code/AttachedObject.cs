using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachedObject : MonoBehaviour {


    public Transform m_AttachingPosition;
    public bool m_AttachedObject;
    private Rigidbody m_ObjectAttached;
    public float m_AttachingObjectSpeed;
    private Quaternion m_AttachingObjectStartRotation;
    private bool m_AttachingObject;
    public float impulseForce = 50f;
    // Use this for initialization
    void Start()
    {
        m_AttachingObjectStartRotation = GetComponent<Transform>().rotation;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAttachedObject();


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            DetachObject(impulseForce);
        }

        //Debug.DrawLine(transform.position, transform.forward,Color.red);
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


    void DetachObject(float Force)
    {
        m_AttachedObject = false;
        m_AttachingObject = false;
        m_ObjectAttached.isKinematic = false;
        m_ObjectAttached.AddForce(m_AttachingPosition.forward * Force);
    }

}
