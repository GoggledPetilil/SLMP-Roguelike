using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWalls : MonoBehaviour
{
    private float m_DestroyTime = 4f;
    
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_DestroyTime);
    }
    
    // To Prevent the Start Room from being clogged
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SpawnPoint" || other.tag == "Wall")
        {
            Destroy(other.gameObject);
        }
    }
}
