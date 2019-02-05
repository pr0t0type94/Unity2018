using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PortalButton : MonoBehaviour
{
    public UnityEvent m_Event;
    public UnityEvent m_Event2;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Companion")
            m_Event.Invoke();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Companion")
            m_Event2.Invoke();
    }
}

