using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomSpawner : MonoBehaviour
{
    public int m_OpenDir; // 1 = NEEDS Top, 2 = NEEDS Right, 3 = NEEDS Bottom, 4 = NEEDS Left
    private RoomTemplates m_RT;
    public bool spawned = false;

    private float m_DestroyTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_DestroyTime);
        m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("RoomSpawn", 0.1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void RoomSpawn()
    {
        if (spawned == false)
        {
            if(m_OpenDir == 1)
            {
                // NEEDS Top opening
                int r = Random.Range(0, m_RT.m_TopRooms.Length);
                Instantiate(m_RT.m_TopRooms[r], transform.position, Quaternion.identity);
            }
            else if(m_OpenDir == 2)
            {
                // NEEDS Right opening
                int r = Random.Range(0, m_RT.m_RightRooms.Length);
                Instantiate(m_RT.m_RightRooms[r], transform.position, Quaternion.identity);
            }
            else if(m_OpenDir == 3)
            {
                // NEEDS Bottom opening
                int r = Random.Range(0, m_RT.m_BottomRooms.Length);
                Instantiate(m_RT.m_BottomRooms[r], transform.position, Quaternion.identity);
            }
            else if(m_OpenDir == 4)
            {
                // NEEDS Left opening
                int r = Random.Range(0, m_RT.m_LeftRooms.Length);
                Instantiate(m_RT.m_LeftRooms[r], transform.position, Quaternion.identity);
            }
            spawned = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "SpawnPoint")
        {
            if (other.gameObject.GetComponent<RoomSpawner>().spawned == false && spawned == false)
            {
                Instantiate(m_RT.m_Closed, this.gameObject.transform.position, Quaternion.identity);
                spawned = true;
                other.GetComponent<RoomSpawner>().spawned = true;
                Destroy(this.transform.parent.gameObject);
            }

            Destroy(this);
        }
    }
}
