using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelController : MonoBehaviour {

    // Use this for initialization
    
    public Canvas DieCanvas;

    public PortalGun gun;

    public FPS_CharacterController FPSController;
    public GameObject iniPos;

    void Start () {
        Cursor.visible = false;


        DieCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}



    public void OpenDoor(GameObject door)
    {
        Animation anim = door.GetComponent<Animation>();
        anim.Play("open");
        AudioSource source = door.GetComponent<AudioSource>();
        source.Play();

    }
    public void CloseDoor(GameObject door)
    {
        Animation anim = door.GetComponent<Animation>();
        anim.Play("close");
        AudioSource source = door.GetComponent<AudioSource>();
        source.Play();

    }
    public void MovePlatform(Animation anim)
    {
        anim.Play("Platform");
    }
    public void StopPlatform(Animation anim)
    {
        anim.Play("Platform");
    }
    public void KillPlayer()
    {
        StartCoroutine(RestartGame());
    }

    IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(0.0f);
        DieCanvas.enabled = true;
        gun.canShoot = false;
        Time.timeScale = 0.0f;

        yield return waitForKeyPress(KeyCode.Return);

        Time.timeScale = 1.0f;

        SceneManager.LoadScene(0);    

    }

    private IEnumerator waitForKeyPress(KeyCode key)
    {
        bool done = false;
        while (!done)
        {
            if (Input.GetKeyDown(key))
            {
                done = true;
            }
            yield return null;
        }

    }
}
