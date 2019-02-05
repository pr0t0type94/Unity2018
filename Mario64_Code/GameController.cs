using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IRestartGameElement
    {
        void RestartGame();
    }

    public class GameController : MonoBehaviour
    {

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_ShowHUDAnimationClip;
    public AnimationClip m_IdleHUDAnimationClip;
    List<IRestartGameElement> m_RestartGameElements = new List<IRestartGameElement>();
    public Text m_CoinsText;
    public PlayerController player;
    public Image currHpImage;
    public float counter;

    public Canvas DieCanvas;
    public RestartGame resetController;
    public int marioLives=3;
    public Text marioLivesText;
    private void Start()
    {
        counter = 8;
        UpdateHP();
    }
    void RestartGame()
    {
        foreach (IRestartGameElement l_RestartGameElement in m_RestartGameElements)
        l_RestartGameElement.RestartGame();


    }


    public void AddRestartGameElement(IRestartGameElement RestartGameElement)
    {
        m_RestartGameElements.Add(RestartGameElement);
    }

    private void Update()
    {
        UpdateCoins();
        UpdateHP();

        if(counter<=0)
        {
            KillPlayer();
            //RESTART GAME
        }
    }
    public void UpdateCoins(bool SetAnimation = true)
    {
        m_CoinsText.text = player.GetCoins().ToString();
        //if (SetAnimation)
        //    ShowHUD();
    }
    public void ShowHUD()
    {
        AnimationState l_AnimationState = m_Animation[m_ShowHUDAnimationClip.name];
        if (!l_AnimationState.enabled || l_AnimationState.normalizedTime >= 1.0f)
        {
            m_Animation.Stop();
            m_Animation.Play(m_ShowHUDAnimationClip.name);
        }
    }
    public void updateMarioLives()
    {
        marioLivesText.text = marioLives.ToString();
    }

    public void UpdateLives()
    {
        counter -= 1f;

    }

    public void UpdateHP()
    {
        
        currHpImage.fillAmount = (counter / 8) ;

    }

    public void starRecolected()
    {
        counter += 1f;
        Debug.Log(counter / 8);
    }

    public void KillPlayer()
    {
        if(marioLives==0)
        {

            resetGame();
        }
        else
        {
        StartCoroutine(tryAgainRutine());
        resetController.newLive();

        }

    }

    IEnumerator tryAgainRutine()
    {
        yield return new WaitForSeconds(0.0f);
        DieCanvas.gameObject.SetActive(true);
        Time.timeScale = 0.0f;


        yield return waitForKeyPress(KeyCode.Return);

        counter = 8;
        marioLives--;
        DieCanvas.gameObject.SetActive(false);
        updateMarioLives();

        Time.timeScale = 1.0f;



        //SceneManager.LoadScene(0);

    }
    public void resetGame()
    {
        StartCoroutine(restartGame());
        resetController.resetGame();

    }

    IEnumerator restartGame()
    {
        yield return new WaitForSeconds(0.0f);
        DieCanvas.GetComponentInChildren<Text>().text = "Game Over press Enter to restart";
        DieCanvas.gameObject.SetActive(true);
        Time.timeScale = 0.0f;


        yield return waitForKeyPress(KeyCode.Return);

        counter = 8;
        marioLives=3;
        DieCanvas.gameObject.SetActive(false);
        updateMarioLives();

        Time.timeScale = 1.0f;



        //SceneManager.LoadScene(0);

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

