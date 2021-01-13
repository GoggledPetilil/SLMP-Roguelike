using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehaviour : Entity
{
    private enum State
    {
        Normal,
        Chasing,
        Damaged
    }
    
    [Header("Enemy Parameters")] 
    public float m_Sight;
    
    [Header("Enemy Physics")]
    public GameObject m_TargetObject;
    public Vector3 m_TargetPos;
    private float m_Interest; // How long the enemy is interested in a location.
    public float m_UnseenTimer; // How long the enemy is unseen by the player.
    
    [Header("Enemy Components")]
    private State m_state;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacter();
        Changeinterest();
        
        m_state = State.Normal;
        
        m_TargetObject = GameObject.Find("Player");

        LevelUp(1 + Mathf.FloorToInt(GameManager.instance.m_Floor / 4));
    }

    // Update is called once per frame
    void Update()
    {
        CheckSight();
        if (m_Invincible == true && m_KnockbackSpeed > 1)
        {
            m_state = State.Damaged;
        }
        
        switch (m_state)
        {
            case State.Normal:
                m_MovDir = ((m_TargetPos) - transform.position).normalized;
                if (m_MovDir.x != 0 || m_MovDir.y != 0)
                {
                    m_ActionDir = m_MovDir;
                }
                
                CheckSight();
                if (Vector2.Distance(transform.position, m_TargetPos) <= m_Sight || m_Interest < 0)
                {
                    // In Sight or bored, change interest.
                    Changeinterest();
                }
                else
                {
                    m_Interest -= Time.deltaTime;
                }
                WhileUnSeen();
                break;
            case State.Chasing:
                m_MovDir = ((m_TargetObject.transform.position) - transform.position).normalized;
                if (m_MovDir.x != 0 || m_MovDir.y != 0)
                {
                    m_ActionDir = m_MovDir;
                }
                break;
            case State.Damaged:
                m_KnockbackSpeed -= m_KnockbackSpeed * (m_Defence + m_DefBonus) * Time.deltaTime;

                float m_KnockSpeedMinimum = 1;
                if (m_KnockbackSpeed < m_KnockSpeedMinimum)
                {
                    StopDust();
                    m_rb.velocity = Vector2.zero;
                    m_KnockbackDir = Vector2.zero;
                    m_state = State.Normal;
                }
                break;
        }
    }

    void FixedUpdate()
    {
        if (m_CanMove == true)
        {
            switch (m_state)
            {
                case State.Normal:
                    m_rb.velocity = m_MovDir * (m_Speed + m_SpdBonus);
                    break;
                case State.Chasing:
                    m_rb.velocity = m_MovDir * (m_Speed + m_SpdBonus);
                    break;
                case State.Damaged:
                    m_rb.velocity = m_KnockbackDir * ((m_Speed + m_SpdBonus) * m_KnockbackSpeed);
                    break;
            }
        }
    }

    private void CheckSight()
    {
        if (Vector2.Distance(transform.position, m_TargetObject.transform.position) <= m_Sight)
        {
            // In Sight, Chase.
            m_state = State.Chasing;
        }
        else
        {
            // No player in sight. Roam about.
            m_state = State.Normal;
        }
    }

    private void Changeinterest()
    {
        RoomTemplates m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        int r = Random.Range(0, m_RT.m_RoomsList.Count - 1);

        m_TargetPos = m_RT.m_RoomsList[r].transform.position;
        m_Interest = 10f;
    }

    private void WhileUnSeen()
    {
        if (m_UnseenTimer > 30)
        {
            Debug.Log(gameObject.name + " got destroyed, due to being inactive.");
            Destroy(gameObject);
        }
        else
        {
            m_UnseenTimer += Time.deltaTime;
        }
    }

    private void LevelUp(int level)
    {
        m_Level = level;

        int hpUp = 2 * (level - 1);
        m_MaxHP += hpUp;
        m_HP += hpUp;

        int paraUp = 1 * (level - 1);
        m_Attack += paraUp;
        m_Defence += paraUp;

        float spdUp = 0.01f * (level - 1);
        m_Speed += spdUp;
    }
}
