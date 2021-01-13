using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private RoomTemplates m_RT;

    public GameObject[] m_Enemies;
    public List<GameObject> m_ActiveEnemyList;
    public int m_EnemiesAllowed;
    public float m_SpawnTimer = 3f;

    // Start is called before the first frame update
    void Start()
    {
        m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_RT.m_StairsAppeared == true)
        {
            if (m_EnemiesAllowed < 1)
            {
                m_EnemiesAllowed = 1 + Mathf.RoundToInt(m_RT.m_RoomsList.Count / 3);
            }

            if (m_SpawnTimer < 0.1 && m_ActiveEnemyList.Count < m_EnemiesAllowed)
            {
                SpawnEnemy();
                m_SpawnTimer = 3f;
            }
            else if(m_SpawnTimer > 0)
            {
                m_SpawnTimer -= Time.deltaTime;
            }

            if (GameManager.instance.m_EnemiesKilled > m_EnemiesAllowed && GameManager.instance.m_HiddenStairsSpawned == false)
            {
                m_RT.StairsToSpawn();
                GameManager.instance.m_HiddenStairsSpawned = true;
            }
        }
        else if (m_RT.m_StairsAppeared == false)
        {
            m_EnemiesAllowed = 0;
            m_ActiveEnemyList.Clear();
        }
    }

    void SpawnEnemy()
    {
        int r = Random.Range(0, m_RT.m_RoomsList.Count - 1);
        int g = Random.Range(0, m_Enemies.Length);

        while (m_RT.m_RoomsList[r].GetComponentInChildren<RoomCam>().m_Focused == true)
        {
            r = Random.Range(0, m_RT.m_RoomsList.Count - 1);
        }

        GameObject prefab = m_Enemies[g];
        float roomX = m_RT.m_RoomsList[r].transform.position.x;
        float roomY = m_RT.m_RoomsList[r].transform.position.y;
        Vector2 roomTransform = new Vector2(roomX, roomY);

        Instantiate(prefab, roomTransform, Quaternion.identity);
        
        m_ActiveEnemyList.Add(prefab);
    }

    public void RemoveDeadEnemies()
    {
        m_ActiveEnemyList.Clear();
        m_ActiveEnemyList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));

    }
}
