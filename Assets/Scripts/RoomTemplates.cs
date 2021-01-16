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
    public GameObject m_StartRoom;

    public List<GameObject> m_RoomsList;

    public GameObject[] m_Decorations;
    public GameObject m_Stairs;
    public float m_Timer;
    public bool m_StairsAppeared;
    public AudioClip m_HiddenStairsSFX;

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
    
    public void ResetDungeon()
    {
        GameManager.instance.StartCoroutine("ScreenFadeOut");
        ClearAll();
    }

    private void ClearAll()
    {
        List<GameObject> allEntities = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));
        allEntities.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Attack")));
        allEntities.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Item")));
        allEntities.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Decor")));
        allEntities.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Wall")));
        allEntities.AddRange(new List<GameObject>(GameObject.FindGameObjectsWithTag("Finish"))); // Stairs
        
        foreach (var go in allEntities)
        {
            GameObject.Destroy(go);
        }

        for (int i = 0; i < m_RoomsList.Count; i++)
        {
            Destroy(m_RoomsList[i].gameObject);
        }
        m_RoomsList.Clear();
        
        Vector3 t = new Vector3(0, 0);
        GameManager.instance.m_Player.transform.position = t;
        Instantiate(m_StartRoom, t, Quaternion.identity);

        GameManager.instance.m_EnemiesKilled = 0;
        GameManager.instance.m_HiddenStairsSpawned = false;
        m_StairsAppeared = false;
        m_Timer = 2f;
    }

    public void StairsToSpawn()
    {
        GameManager.instance.PlayAudio(m_HiddenStairsSFX);
        Instantiate(m_Stairs, m_RoomsList[0].transform.position, Quaternion.identity);
    }
}
