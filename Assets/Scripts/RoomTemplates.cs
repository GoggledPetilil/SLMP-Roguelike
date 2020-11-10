using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTemplates : MonoBehaviour
{
    public GameObject[] m_TopRooms;
    public GameObject[] m_RightRooms;
    public GameObject[] m_BottomRooms;
    public GameObject[] m_LeftRooms;

    public GameObject m_Closed;

    public List<GameObject> m_RoomsList;

    public GameObject m_Stairs;
    public float m_Timer;
    public bool m_StairsAppeared;

    // Update is called once per frame
    void Update()
    {
        if (m_Timer < 0 && m_StairsAppeared == false)
        {
            Instantiate(m_Stairs, m_RoomsList[m_RoomsList.Count - 1].transform.position, Quaternion.identity);
            m_StairsAppeared = true;
        }
        else if(m_StairsAppeared == false)
        {
            m_Timer -= Time.deltaTime;
        }
    }
}
