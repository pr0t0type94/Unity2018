using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartGame : MonoBehaviour {

    //private GameObject[] enemiesToRespawnList;
    private List<GameObject> enemiesToRespawnList = new List<GameObject>();
    public PlayerController playerController;

    // Use this for initialization
    void Start () {
        

    }
	
	// Update is called once per frame
	void Update () {
		
	}
    public void resetGame()
    {
        StartCoroutine(restartGame());
    }
    public void newLive()
    {
        StartCoroutine(tryAgain());

    }
    IEnumerator tryAgain()
    {
        foreach (GameObject enemy in enemiesToRespawnList)
        {
            if (enemy.GetComponent<KoopaEnemy>() != null)
            {
                enemy.transform.position = enemy.GetComponent<KoopaEnemy>().startingPosition;
                enemy.GetComponent<KoopaEnemy>().state = KoopaEnemy.TState.WALK;

            }
            else if (enemy.GetComponent<GoombaEnemy>() != null)
            {
                enemy.transform.position = enemy.GetComponent<GoombaEnemy>().startingPosition;
                enemy.GetComponent<GoombaEnemy>().state = GoombaEnemy.TState.WALK;

            }
        }
        playerController.gameObject.SetActive(false);
        playerController.transform.position = playerController.respawnPosition;
        //chekcpoint position
        yield return new WaitForSeconds(.1f);
        playerController.gameObject.SetActive(true);
    }

        IEnumerator restartGame()
    {
        playerController.gameObject.SetActive(false);
        playerController.transform.position = playerController.respawnPosition;
        yield return new WaitForSeconds(.1f);
        playerController.gameObject.SetActive(true);


        foreach (GameObject enemy in enemiesToRespawnList)
        {
            if(enemy.GetComponent<KoopaEnemy>() !=null)
            {
                enemy.transform.position = enemy.GetComponent<KoopaEnemy>().startingPosition;
                enemy.GetComponent<KoopaEnemy>().state = KoopaEnemy.TState.WALK;

            }
            else if(enemy.GetComponent<GoombaEnemy>() != null)
            {
                enemy.transform.position = enemy.GetComponent<GoombaEnemy>().startingPosition;
                enemy.GetComponent<GoombaEnemy>().state = GoombaEnemy.TState.WALK;


            }
            enemy.SetActive(true);
        }

    }

    public void addEnemyToList(GameObject obj)
    {
        enemiesToRespawnList.Add(obj);
    }
    
}
