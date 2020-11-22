using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stairs : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            // Erase all the walls, items and enemies.
            // Set Player back to 0, 0.
            // Spawn new Start Room
            // Generate new dungeon
        }
    }
}
