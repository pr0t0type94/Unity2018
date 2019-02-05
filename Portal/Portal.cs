using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Transform m_PlayerCamera;
    public Portal m_MirrorPortal;
    public Camera m_PortalCamera;
    public float m_NearClipOffset = 0.5f;

    public List<Transform> m_ValidPoints;

    private float m_Yaw;
    public float m_SetPortalOffset;
    public Transform m_MirrorTransform;

    public LayerMask Layer;
    // Use this for initialization
    void Start () {
		
	}



    // Update is called once per frame
    void Update()
    {
        //Vector3 l_ReflectedVector = m_PlayerCamera.position - transform.position;
        //l_ReflectedVector = transform.position - l_ReflectedVector;
        //Debug.DrawLine(transform.position, l_ReflectedVector, Color.red);

        //l_ReflectedVector = transform.InverseTransformPoint(l_ReflectedVector);
        //m_MirrorPortal.m_PortalCamera.transform.position = m_MirrorPortal.transform.TransformPoint(l_ReflectedVector);
        //m_PortalCamera.transform.forward = (transform.position - m_PortalCamera.transform.position).normalized;
        //m_PortalCamera.nearClipPlane = Vector3.Distance(m_PortalCamera.transform.position, this.transform.position) + m_NearClipOffset;


        ////FOV
        //Vector3 l_PlayerToPortal = transform.position - m_PlayerCamera.position;
        //float l_Distance = l_PlayerToPortal.magnitude;
        //float l_Pct = 1.0f - Mathf.Min(l_Distance / m_MaxFOVDistance, 1.0f);
        //m_MirrorPortal.m_PortalCamera.fieldOfView = Mathf.Lerp(m_MinFOV, 60.0f, l_Pct);

        //CORRECTION
        Vector3 l_ReflectedPosition = m_MirrorTransform.InverseTransformPoint(m_PlayerCamera.position);
        Vector3 l_ReflectedDirection = m_MirrorTransform.InverseTransformDirection(m_PlayerCamera.forward);
        m_MirrorPortal.m_PortalCamera.transform.position = m_MirrorPortal.transform.TransformPoint(l_ReflectedPosition);
        m_MirrorPortal.m_PortalCamera.transform.forward = m_MirrorPortal.transform.TransformDirection(l_ReflectedDirection);

        m_PortalCamera.nearClipPlane = Vector3.Distance(m_PortalCamera.transform.position, this.transform.position) + m_NearClipOffset;

    }

    public bool IsValidPosition()
    {


        Vector3 l_Normal = Vector3.zero;
        bool valid = false;
        for (int i = 0; i < m_ValidPoints.Count; ++i)
        {
            Transform l_ValidPoint = m_ValidPoints[i];
            Ray l_Ray = new Ray(m_PlayerCamera.position, l_ValidPoint.position - m_PlayerCamera.position);

            RaycastHit l_RaycastHit;
            if (Physics.Raycast(l_Ray, out l_RaycastHit, Layer))
            {
                //Por tag, normal y distancia coincide en todos los puntos
                valid =(m_ValidPoints[i].tag != l_RaycastHit.collider.tag && l_Normal != l_RaycastHit.normal
                    && (m_ValidPoints[i].transform.position-m_PlayerCamera.position) != (l_RaycastHit.point - m_PlayerCamera.position));
                return false;
            }
        }

        return true;
    }




    //end of class
}
