using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOverlap : MonoBehaviour
{
    public bool m_Spawned;

    // Start is called before the first frame update
    void Start()
    {
        m_Spawned = true;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Decor")
        {
            if (other.gameObject.GetComponent<EntityOverlap>().m_Spawned == false && m_Spawned == true)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
