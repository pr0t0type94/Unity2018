using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionCube : MonoBehaviour
{

    public LineRenderer m_LineRenderer;
    private bool m_CreateRefraction;
    public float m_MaxDistance;
    public LayerMask m_CollisionLayerMask;

    void Update()
    {
        m_LineRenderer.gameObject.SetActive(m_CreateRefraction);
        m_CreateRefraction = false;
    }
    public void CreateRefraction()
    {
        if (m_CreateRefraction == true)
            return;

        m_CreateRefraction = true;
        Vector3 l_EndRaycastPosition = Vector3.forward * m_MaxDistance;
        RaycastHit l_RaycastHit;
        if (Physics.Raycast(new Ray(m_LineRenderer.transform.position, m_LineRenderer.transform.forward), out l_RaycastHit, m_MaxDistance, m_CollisionLayerMask.value))
        {
            l_EndRaycastPosition = Vector3.forward * l_RaycastHit.distance;
            if (l_RaycastHit.collider.tag == "RefractionCube")
            {
                //Reflect ray
                l_RaycastHit.collider.GetComponent<RefractionCube>().CreateRefraction();
            }
            //Other collisions
        }
        m_LineRenderer.SetPosition(1, l_EndRaycastPosition);
    }
}

