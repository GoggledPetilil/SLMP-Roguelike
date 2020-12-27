using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public enum Opening
    {
        Top,
        Right,
        Bottom,
        Left
    }
    
    public Opening m_Opening;
    private int m_DecorAmount;
    private int m_MaxDecorAllowed = 8;
    private RoomTemplates m_RT;
    public bool m_Spawned;

    private float m_DestroyTime = 3f;

    // Start is called before the first frame update
    void Start()
    {
        //Destroy(gameObject, m_DestroyTime);
        m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        Invoke("RoomSpawn", 0.1f);
    }

    void RoomSpawn()
    {
        if (m_Spawned == false)
        {
            switch (m_Opening)
            {
                case Opening.Top:
                    int tr = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_TopRooms.Length));
                    Instantiate(m_RT.m_TopRooms[tr], transform.position, Quaternion.identity);
                    break;
                case Opening.Right:
                    int rr = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_RightRooms.Length));
                    Instantiate(m_RT.m_RightRooms[rr], transform.position, Quaternion.identity);
                    break;
                case Opening.Bottom:
                    int br = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_BottomRooms.Length));
                    Instantiate(m_RT.m_BottomRooms[br], transform.position, Quaternion.identity);
                    break;
                case Opening.Left:
                    int lr = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_LeftRooms.Length));
                    Instantiate(m_RT.m_LeftRooms[lr], transform.position, Quaternion.identity);
                    break;
            }
            
            m_Spawned = true;
            
            if (m_DecorAmount < 1)
            {
                m_DecorAmount = Mathf.RoundToInt(GameManager.instance.GetRandomRange(0, m_MaxDecorAllowed));
            }
            DecorateRoom();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
        {
            /*if (other.gameObject.tag == "SpawnPoint")
            {
                if (m_Spawned == false && other.gameObject.GetComponent<RoomSpawner>().m_Spawned == false)
                {
                    Instantiate(m_RT.m_Closed, gameObject.transform.position, Quaternion.identity);
                    
                    m_Spawned = true;
                    other.GetComponent<RoomSpawner>().m_Spawned = true;
                    
                    Destroy(transform.parent.gameObject);
                    Destroy(other.transform.parent.gameObject);
                }
                else if (m_Spawned == true && other.gameObject.GetComponent<RoomSpawner>().m_Spawned == false)
                {
                    Destroy(other.transform.parent.gameObject);
                }
            }*/

            if (other.CompareTag("SpawnPoint"))
            {
                if (other.GetComponent<RoomSpawner>().m_Spawned == false && m_Spawned == false)
                {
                    Instantiate(m_RT.m_Closed, gameObject.transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }

                m_Spawned = true;
            }
        }

        private void DecorateRoom()
        {
            for (int i = 0; i < m_DecorAmount; i++)
            {
                int r = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_Decorations.Length));
                GameObject go = m_RT.m_Decorations[r];

                float posX = transform.position.x;
                float posY = transform.position.y;
                float offsetX = GameManager.instance.GetRandomRange(-3f, 3f);
                float offsetY = GameManager.instance.GetRandomRange(-3f, 3f);
                Vector2 roomTransform = new Vector2(posX + offsetX, posY + offsetY);

                Instantiate(go, roomTransform, Quaternion.identity);
            }
        }
    }
