using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public float maxHealth;
    public float currHealth;
    public GameObject Item_HP;
    public GameObject Item_Ammo;
    public GameObject Item_Shield;

    private Random rnd;

    public Vector3 powerCorrectionPos = new Vector3 (0,0.0f,0);
    public GameController gc;
    public float points;
    public ParticleSystem explosionParticles;
    public Vector3 iniPos;
    // Use this for initialization
    private void Awake()
    {
        gameObject.SetActive(true);
        iniPos = transform.position;
        currHealth = maxHealth;
    }
    public void TakeDamage(float amount, float multi)
    {

        currHealth -= amount * multi;

        if (currHealth <= 0)
        {
            StartCoroutine(waitForDestroy());
        }
    }
    void Die()
    {
        gameObject.SetActive(false);
        gc.SumPoints(points);
    }
    IEnumerator waitForDestroy()
    {
        SpawnRandomItem();
        CreateExplosionParticles(transform.position);
        explosionParticles.Play();

        yield return new WaitForSeconds(.25f);

        Die();

    }

    void SpawnRandomItem()
    {
        var rnd = Random.Range(0,2);

        if (rnd == 0)
            Instantiate(Item_HP, transform.position, Quaternion.identity);
        else if (rnd == 1)
            Instantiate(Item_Ammo, transform.position, Quaternion.identity);
        else if (rnd == 2)
            Instantiate(Item_Shield, transform.position, Quaternion.identity);
        else
            return;
        
    

    }

    void CreateExplosionParticles(Vector3 Position)
    {
        GameObject.Instantiate(explosionParticles, Position, Quaternion.identity);
    }

    //get current target from gun,, get hp from current target,,
}
