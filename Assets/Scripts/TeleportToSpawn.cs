using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportToSpawn : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Entity"))
        {
            float roomX = 0;
            float roomY = 0;
            float offsetX = GameManager.instance.GetRandomRange(-3f, 3f);
            float offsetY = GameManager.instance.GetRandomRange(-3f, 3f);
            
            other.transform.position = new Vector3(roomX + offsetX, roomY + offsetY);
        }
        else if (other.CompareTag("Decor") || other.CompareTag("Item"))
        {
            Destroy(other.gameObject);
        }
    }
}
