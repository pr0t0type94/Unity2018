using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    public int KeysCollected;
    public Text KeysCollected_text;
    public FPSController m_PlayerController;
    public Text points_Text;
    private float currentPoints;

    public Player_Health player_Hp;
    public Player_Shield player_shield;
     
    private GameObject[] enemiesToRespawnList;

    /// <summary>
    /// /Shooting Galery
    /// </summary>
    private GameObject[] dianasToRespawnList;
    private Animator[] dianasListAnimator = new Animator[5];
    private float iniDianas;
    private float currentDianas;
    public int phaseMode = -1;
    private bool StartGallery;
    public Canvas GalleryCanvas;
    private int gamesPlayed = 0;
    public float GalleryTimer = 30f;
    private float iniGalleryTimer;
    public Text infoText;

    public Gun guns;

    private bool startTimer;

    public Text galleryTimeText;

    private float maxPoints;

    public GameObject portal;

    public Text congratzText;

    public Canvas restartCanvas;

    public AudioSource DieSound;

    public GameObject respanwPoint;
    private Vector3 initialpos;

    // Use this for initialization
    private void Awake()
    {
        enemiesToRespawnList = GameObject.FindGameObjectsWithTag("Enemy");
        restartCanvas.enabled = false;
        GalleryCanvas.enabled = false;
        initialpos = respanwPoint.transform.position;
        currentPoints = 0;
        points_Text.text = currentPoints.ToString();

    }
    void Start () {
        iniGalleryTimer = GalleryTimer;
        
        KeysCollected = 0;
        KeysCollected_text.text = "x" + KeysCollected.ToString();


        dianasToRespawnList = GameObject.FindGameObjectsWithTag("Diana");

        iniDianas = 5;
        currentDianas = iniDianas;

        StartGallery = false;


        for (int i = 0; i < dianasToRespawnList.Length; i++)
        {
            dianasListAnimator[i] = dianasToRespawnList[i].GetComponentInParent<Animator>();
        }

        setDianasState(false);

        infoText.text = "Press ENTER to start";
        galleryTimeText.text = GalleryTimer.ToString();


    }

    // Update is called once per frame
    void Update () {
		//if(Input.GetKeyDown(KeyCode.F1))
  //      {
  //          SceneManager.LoadScene(0);
  //      }
  //      if (Input.GetKeyDown(KeyCode.F2))
  //      {
  //          SceneManager.LoadScene(1);
  //      }



        if(GalleryCanvas.enabled)
        {
            guns.canAttack = false;

            if (Input.GetKeyDown(KeyCode.Return))
                StartCoroutine(startCountDown());
        }


        if(phaseMode == 1 && currentDianas ==0)
        {
            //wait some time
            //reset all dianas //start phase 2
            GoToNextPhase();

        }
        if(phaseMode==2 && currentDianas == 0)
        {

            GoToNextPhase();

        }

        if(phaseMode == 3 && currentDianas ==0)
        {            
            EndGalleryGame();
        }


        if(startTimer)
        {           
            GalleryTimer -= Time.deltaTime;
            float roundTimer = Mathf.Abs(GalleryTimer);
            galleryTimeText.text = roundTimer.ToString();

            if (GalleryTimer <= 0)
            {
                startTimer = false;               
                EndGalleryGame();
                
            }
        }

        //if(tryAgainCanvas.enabled==true)
        //{
        //    GalleryCanvas.enabled = false;
        //    if (Input.GetKeyDown(KeyCode.Return))
        //    {

        //        StartCoroutine(startCountDown());

        //    }
        //}
    }

    public void KeyCollected()
    {
        KeysCollected++;
        updateKeysText();
    }
    private void updateKeysText()
    {
        KeysCollected_text.text = KeysCollected.ToString();
    }


    public void PlayerDie()
    {
        player_Hp.CurrHp = 0;
        DieSound.Play();

        StartCoroutine(RestartGame(initialpos));
    }
    public void TakeDamage(float damage)
    {
        player_Hp.RecieveDamage(damage);
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

    IEnumerator RestartGame(Vector3 inipos)
    {
        //show canvas //wait for input
        yield return new WaitForSeconds(0.0f);
        restartCanvas.enabled = true;
        guns.canAttack = false;
        Time.timeScale = 0.0f;

        yield return waitForKeyPress(KeyCode.Return);

        m_PlayerController.transform.position = inipos;
        Time.timeScale = 1.0f;
        restartCanvas.enabled = false;
        guns.canAttack = true;
        //last checkpoint
        player_Hp.CurrHp = player_Hp.MaxHp;
        player_shield.CurrShield = 0;
        m_PlayerController.currentStamina = m_PlayerController.maxStamina;


        foreach (GameObject enemy in enemiesToRespawnList)
        {
            enemy.SetActive(true);
            enemy.transform.position = enemy.GetComponent<Enemy>().iniPos;
            enemy.GetComponent<Enemy>().currHealth = enemy.GetComponent<Enemy>().maxHealth;
            enemy.GetComponent<Enemy_IA>().SetIdleState();
        }
    }




    /// <summary>
    /// //gallery
    /// </summary>
    public void SumPoints(float points)
    {
        currentDianas--;
        currentPoints += points;
        points_Text.text = currentPoints.ToString();

    }
   
    IEnumerator startCountDown()
    {
        GalleryCanvas.enabled = true;
        currentDianas = 5;

        portal.SetActive(false);
        currentPoints = 0;
        guns.canAttack = false;
        galleryTimeText.enabled = true;

        infoText.text = "3";
        yield return new WaitForSeconds(1);
        infoText.text = "2";
        yield return new WaitForSeconds(1);
        infoText.text = "1";
        yield return new WaitForSeconds(1);

        guns.canAttack = true;
        GalleryCanvas.enabled = false;
        phaseMode = 1;

        GalleryTimer = iniGalleryTimer;
        startTimer = true;
        setDianasState(true);
        setDianasAnimMode(phaseMode);
        gamesPlayed++;

    }

    void EndGalleryGame()
    {

        GalleryTimer = 0;
        guns.canAttack = false;
        phaseMode = 0;
        setDianasAnimMode(phaseMode);
        galleryTimeText.enabled = false;
        GalleryCanvas.enabled = true;
        infoText.text = "Press ENTER to try again";
        //tryAgainCanvas.enabled = true;


        if (currentPoints > 3500)
        {
            //spawn portal load new scene
            portal.SetActive(true);
            congratzText.text = "Congratulations, proceed to the portal";

            //open door
        }

    }

    void GoToNextPhase()
    {
        setDianasState(true);

        phaseMode++;
        setDianasAnimMode(phaseMode);

        currentDianas = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gallery")
        {
            GalleryCanvas.enabled = true;
            infoText.text = "Press ENTER to start";

        }
        else if (other.gameObject.tag == "Portal")
        {
            
            SceneManager.LoadSceneAsync(1);
        }
    }

   
    void setDianasState(bool state)
    {
        foreach (GameObject diana in dianasToRespawnList)
        {
            diana.SetActive(state);
        }
    }

    void setDianasAnimMode(int phase)
    {
        foreach (Animator anim in dianasListAnimator)
        {
            anim.SetInteger("Phase", phase);
        }
    }
}
