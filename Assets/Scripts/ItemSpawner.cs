using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemSpawner : MonoBehaviour
{
    private RoomTemplates m_RT;
    public GameObject[] m_Items;
    public int m_ItemsAllowed;
    
    // Start is called before the first frame update
    void Start()
    {
        m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RT.m_StairsAppeared == true && m_ItemsAllowed > 0)
        {
            SpawnItems();
            m_ItemsAllowed--;
        }
        else if (m_RT.m_StairsAppeared == false && m_ItemsAllowed < 1)
        {
            m_ItemsAllowed = Mathf.RoundToInt(GameManager.instance.GetRandomRange(2, 8));
        }
    }
    
    void SpawnItems()
    {
        int r = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_RT.m_RoomsList.Count));
        int g = Mathf.FloorToInt(GameManager.instance.GetRandomRange(0, m_Items.Length));

        GameObject prefab = m_Items[g];
        float roomX = m_RT.m_RoomsList[r].transform.position.x;
        float roomY = m_RT.m_RoomsList[r].transform.position.y;
        float offsetX = GameManager.instance.GetRandomRange(-3f, 3f);
        float offsetY = GameManager.instance.GetRandomRange(-3f, 3f);
        
        Vector2 roomTransform = new Vector2(roomX + offsetX, roomY + offsetY);

        Instantiate(prefab, roomTransform, Quaternion.identity);
    }
}
