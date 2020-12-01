using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private enum State
    {
        Normal,
        Attacking,
        Rolling
    }
    
    [Header("Physics")]
    public float m_MoveSpeed;
    public float m_RollSpeed;
    private float m_RollSpeedOrigin;
    public Vector2 m_Mov;
    public Vector2 m_ActionDir;
    public float m_ActionCooldown;
    private float m_ActionTimer;
    
    [Header("Components")]
    public Animator m_ani;
    public Rigidbody2D m_rb;
    private State m_state;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_state = State.Normal;
        m_RollSpeedOrigin = m_RollSpeed;
        m_ActionDir.y = -1f;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        { 
            case State.Normal:
                float m_MovX = 0f;
                float m_MovY = 0f;
        
                m_MovX = Input.GetAxisRaw("Horizontal");
                m_MovY = Input.GetAxisRaw("Vertical");
        
                m_Mov = new Vector2(m_MovX, m_MovY).normalized;
                if (m_MovX != 0 || m_MovY != 0)
                {
                    m_ActionDir = m_Mov;
                }
        
                MoveAndAni();

                if (Input.GetButtonDown("Roll") && m_ActionTimer <= 0)
                {
                    ActionDirToDirection();
                    m_state = State.Rolling;
                    m_ani.SetBool("isRolling", true);
                }

                if (Input.GetButtonDown("Attack") && m_ActionTimer <= 0)
                {
                    m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    m_Mov = new Vector2(0f, 0f);
                    ActionDirToDirection();
                    m_state = State.Attacking;
                    StartCoroutine("Attack");
                }

                m_ActionTimer -= 1 * Time.deltaTime;
                
                break;
            case State.Rolling:
                float m_RollMultiplier = 10f; // How fast the rolling speed decreases.

                m_RollSpeed -= m_RollSpeed * m_RollMultiplier * Time.deltaTime;

                float m_RollSpeedMinimum = 1;
                if (m_RollSpeed < m_RollSpeedMinimum)
                {
                    m_RollSpeed = m_RollSpeedOrigin;

                    m_state = State.Normal;
                    m_ani.SetBool("isRolling", false);
                    ReturnCooldown();
                }

                break;
        }
    }

    void FixedUpdate()
    {
        switch (m_state)
        {
            case State.Normal:
                m_rb.velocity = m_Mov * m_MoveSpeed;
                break;
            case State.Rolling:
                m_rb.velocity = m_ActionDir * m_MoveSpeed * m_RollSpeed;
                break;
        }
    }

    private void MoveAndAni()
    {
        if (m_Mov != Vector2.zero)
        {
            m_ani.SetFloat("Horizontal", m_Mov.x);
            m_ani.SetFloat("Vertical", m_Mov.y);
        }
        m_ani.SetFloat("Speed", m_Mov.sqrMagnitude);
    }
    
    private IEnumerator Attack()
    {
        m_ani.SetBool("Attacking", true);
        yield return null;
        m_ani.SetBool("Attacking", false);
        yield return new WaitForSeconds(0.3f);
        ReturnCooldown();
        m_state = State.Normal;
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void ReturnCooldown()
    {
        m_ActionTimer = m_ActionCooldown;
    }

    private void ActionDirToDirection()
    {
        if (m_ActionDir.y > 0f && m_ActionDir.y < 1f)
        {
            if (m_rb.velocity.magnitude > 0)
            {
                m_ActionDir.x = 0f;
                m_ActionDir.y = Mathf.RoundToInt(m_ActionDir.y);
            }
            else
            {
                m_ActionDir.x = Mathf.RoundToInt(m_ActionDir.x);
                m_ActionDir.y = 0f;
            }
        }
        else if (m_ActionDir.y < 0f && m_ActionDir.y > -1f)
        {
            m_ActionDir.x = 0f;
            m_ActionDir.y = Mathf.RoundToInt(m_ActionDir.y);
        }
        
        m_ani.SetFloat("Horizontal", m_ActionDir.x);
        m_ani.SetFloat("Vertical", m_ActionDir.y);
    }
}
