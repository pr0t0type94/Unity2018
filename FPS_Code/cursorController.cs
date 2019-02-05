using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorController : MonoBehaviour {

	// Use this for initialization
        public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;
    private bool m_AngleLocked;
    private bool m_AimLocked;

    public Canvas infoCanvas;
    void Start () {
        Cursor.visible = false;

    }

    // Update is called once per frame
    void Update () {


        if(Input.GetKeyDown(m_DebugLockAngleKeyCode))
        m_AngleLocked=!m_AngleLocked;
        if(Input.GetKeyDown(m_DebugLockKeyCode))
        {
        if(Cursor.lockState==CursorLockMode.Locked)
	        Cursor.lockState=CursorLockMode.None;
        else
	        Cursor.lockState=CursorLockMode.Locked;
        m_AimLocked=Cursor.lockState==CursorLockMode.Locked;
        }

        if(infoCanvas.enabled == true)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                infoCanvas.enabled = false;
            }
        }

	}
}
