using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{    
    List<GameObject> m_Elements;
    int m_CurrentElementId = 0;


    public ObjectPool(int ElementsCount, GameObject Prefab, Transform Parent)
     {
        m_Elements = new List<GameObject>(ElementsCount);

        for (int i=0;i< ElementsCount; i++)
        {
            GameObject obj = GameObject.Instantiate(Prefab,Parent);
            obj.SetActive(false);
            m_Elements.Add(obj);
        }
            
    }
    public GameObject GetNextElement()
    {        

            m_CurrentElementId += 1;

            if (m_CurrentElementId == m_Elements.Count)
            {
                m_CurrentElementId = 0;
            }


        return m_Elements[m_CurrentElementId];
       
    }       
}
