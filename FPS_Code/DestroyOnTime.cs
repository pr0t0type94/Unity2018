using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour {

    public float m_DestroyOnTime = 3.0f;

    private void Start()
    {
        StartCoroutine(DestroyOnTimeFunct());

    }

    IEnumerator DestroyOnTimeFunct()
    {
        yield return new WaitForSeconds(m_DestroyOnTime);
        GameObject.Destroy(gameObject);
    }

}
