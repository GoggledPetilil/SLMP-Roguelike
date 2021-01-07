using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOverlap : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Decor")
        {
            Destroy(gameObject);
        }
    }
}
