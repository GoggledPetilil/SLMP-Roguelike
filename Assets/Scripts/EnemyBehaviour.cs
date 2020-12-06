using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : Entity
{
    private enum State
    {
        Normal,
        Chasing,
        Attacking,
        Damaged
    }
    
    [Header("Enemy Parameters")] 
    public float m_Sight;
    
    [Header("Enemy Physics")]
    public GameObject m_TargetObject;
    public Vector2 m_TargetPos;
    
    [Header("Enemy Components")]
    private State m_state;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacter();
        
        m_state = State.Normal;
        
        m_TargetObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        CheckSight();

        switch (m_state)
        {
            case State.Normal:
                // Move to a random room. Preferably with pathfinding.
                
                CheckSight();
                ReduceInvincibility();
                
                if (m_Damaged == true)
                {
                    m_state = State.Damaged;
                }
                break;
            case State.Chasing:
                m_MovDir = ((m_TargetObject.transform.position) - transform.position).normalized;
                if (m_MovDir.x != 0 || m_MovDir.y != 0)
                {
                    m_ActionDir = m_MovDir;
                }
                
                CheckSight();
                ReduceInvincibility();
                
                if (m_Damaged == true)
                {
                    m_state = State.Damaged;
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
        switch (m_state)
        {
            case State.Normal:
                m_rb.velocity = m_MovDir * 0f;
                break;
            case State.Chasing:
                m_rb.velocity = m_MovDir * (m_Speed + m_SpdBonus);
                break;
            
            case State.Damaged:
                m_rb.velocity = m_KnockbackDir * ((m_Speed + m_SpdBonus) * m_KnockbackSpeed);
                break;
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
}
