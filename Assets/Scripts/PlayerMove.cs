using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : Entity
{
    public enum State
    {
        Normal,
        Attacking,
        Rolling,
        Damaged
    }
    
    [Header("Player Physics")]
    public float m_RollSpeed;
    private float m_RollSpeedOrigin;
    public float m_ActionCooldown;
    private float m_ActionTimer;
    
    [Header("Player Components")]
    private State m_state;
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeCharacter();
        
        m_state = State.Normal;

        m_RollSpeedOrigin = m_RollSpeed;
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
        
                m_MovDir = new Vector2(m_MovX, m_MovY).normalized;
                if (m_MovX != 0 || m_MovY != 0)
                {
                    m_ActionDir = m_MovDir;
                }
        
                MoveAni();

                if (Input.GetButtonDown("Roll") && m_ActionTimer <= 0)
                {
                    CreateDust();
                    m_IsInvincible = true;
                    ActionDirToDirection();
                    m_state = State.Rolling;
                    m_ani.SetBool("isRolling", true);
                }

                if (Input.GetButtonDown("Attack") && m_ActionTimer <= 0)
                {
                    m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
                    m_MovDir = new Vector2(0f, 0f);
                    ActionDirToDirection();
                    m_state = State.Attacking;
                    StartCoroutine("Attack");
                }

                m_ActionTimer -= 1 * Time.deltaTime;
                ReduceInvincibility();

                if (m_Damaged == true)
                {
                    m_state = State.Damaged;
                }
                break;
            case State.Rolling:
                float m_RollMultiplier = 10f; // How fast the rolling speed decreases.

                m_RollSpeed -= m_RollSpeed * m_RollMultiplier * Time.deltaTime;

                float m_RollSpeedMinimum = 1;
                if (m_RollSpeed < m_RollSpeedMinimum)
                {
                    m_RollSpeed = m_RollSpeedOrigin;

                    StopDust();
                    m_ani.SetBool("isRolling", false);
                    ReturnCooldown();
                    m_IsInvincible = false;
                    m_state = State.Normal;
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
                m_rb.velocity = m_MovDir * (m_Speed + m_SpdBonus);
                break;
            case State.Rolling:
                m_rb.velocity = m_ActionDir * ((m_Speed + m_SpdBonus) * m_RollSpeed);
                break;
            case State.Damaged:
                m_rb.velocity = m_KnockbackDir * ((m_Speed + m_SpdBonus) * m_KnockbackSpeed);
                break;
        }
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
}
