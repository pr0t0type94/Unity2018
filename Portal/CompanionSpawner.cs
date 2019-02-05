using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    public Transform m_SpawnPosition;
    public GameObject m_CompanionPrefab;
    public int companionNmbers = 0;
    private bool Spawned;
    private AudioSource source;

    private void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(Spawned)
        {
            Spawned = false;
        }
    }

    public void Spawn()
    {     
        if(!Spawned)
        {
            GameObject.Instantiate(m_CompanionPrefab, m_SpawnPosition.position, m_SpawnPosition.rotation, null);
            source.Play();
            Spawned = true;
        }
              
    }
}

