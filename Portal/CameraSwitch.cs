using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraSwitch : MonoBehaviour
{

    public Camera FPSCamera;
    public Camera TPSCamera;
    public Mode CameraMode;

    public KeyCode FirstPersonKey = KeyCode.F;
    public KeyCode ThirdPersonKey = KeyCode.T;

    private bool fpsactive = true;
    public enum Mode
    {
        FPS,
        TPS,

    }
    // Use this for initialization
    void Start()
    {
        FPSCamera.enabled = fpsactive;
        TPSCamera.enabled = !fpsactive;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(FirstPersonKey))
            CameraMode = Mode.FPS;

        if (Input.GetKeyDown(ThirdPersonKey))
            CameraMode = Mode.TPS;


        if (CameraMode == Mode.FPS)
        {
            fpsactive = true;
            FPSCamera.enabled = fpsactive;
            TPSCamera.enabled = !fpsactive;
        }
        else if (CameraMode == Mode.TPS)
        {
            fpsactive = false;
            FPSCamera.enabled = fpsactive;
            TPSCamera.enabled = !fpsactive;
        }
    }

}
    