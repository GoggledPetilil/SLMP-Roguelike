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
        Damaged,
        Frozen
    }

    [Header("Player Parameters")] 
    public int m_Exp;
    
    [Header("Player Physics")]
    public float m_RollSpeed;
    public float m_ActionCooldown;
    public float m_ActionTimer;
    
    [Header("Player Components")]
    public State m_state;
    public HealthBar m_HealthBar;
    public SpriteRenderer m_AtkBoostIcon;
    public SpriteRenderer m_DefBoostIcon;
    public SpriteRenderer m_SpdBoostIcon;

    [Header("Player Audio")] 
    public AudioClip m_SwordSwing;
    public AudioClip m_RollSFX;
    public AudioClip m_LevelUpJingle;
    
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance.m_Player == null)
        {
            GameManager.instance.m_Player = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
        InitializeCharacter();
        SetRollSpeed();
        m_state = State.Normal;
        
        m_HealthBar.SetHealthText(m_HP, m_MaxHP);
        m_HealthBar.SetHealthBar(m_HP / m_MaxHP);
        ChangeStatBoost(0, 0, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_state)
        { 
            case State.Normal:
                if (m_Invincible == true && m_KnockbackSpeed > 1)
                {
                    m_state = State.Damaged;
                }
                
                float m_MovX = 0f;
                float m_MovY = 0f;

                if (m_CanMove)
                {
                    m_MovX = Input.GetAxisRaw("Horizontal");
                    m_MovY = Input.GetAxisRaw("Vertical");
                }
                
                m_MovDir = new Vector2(m_MovX, m_MovY).normalized;
                if (m_MovX != 0 || m_MovY != 0)
                {
                    m_ActionDir = m_MovDir;
                }
        
                MoveAni();

                if (Input.GetButtonDown("Roll") && m_ActionTimer <= 0)
                {
                    PlayAudio(m_RollSFX);
                    CreateDust();
                    m_Invincible = true;
                    ActionDirToDirection();
                    m_state = State.Rolling;
                    m_ani.SetBool("isRolling", true);
                    StartCoroutine(InvincibleFrames(1f + (m_Defence + m_DefBonus) / 100));
                }

                if (Input.GetButtonDown("Attack") && m_ActionTimer <= 0)
                {
                    PlayAudio(m_SwordSwing);
                    FreezeMovement(true);
                    m_MovDir = new Vector2(0f, 0f);
                    ActionDirToDirection();
                    m_state = State.Attacking;
                    StartCoroutine("Attack");
                }

                if (m_ActionTimer > 0)
                {
                    m_ActionTimer -= 1 * Time.deltaTime;
                }
                
                break;
            case State.Rolling:
                float m_RollMultiplier = 10f; // How fast the rolling speed decreases.

                m_RollSpeed -= m_RollSpeed * m_RollMultiplier * Time.deltaTime;

                float m_RollSpeedMinimum = 1;
                if (m_RollSpeed < m_RollSpeedMinimum)
                {
                    SetRollSpeed();
                    StopDust();
                    m_ani.SetBool("isRolling", false);
                    ReturnCooldown();
                    m_Invincible = true;
                    m_state = State.Normal;
                }
                break;
            case State.Damaged:
                float m_KnockBackMulti = m_Defence + m_DefBonus; // How fast the knockback speed decreases.
                
                m_KnockbackSpeed -= m_KnockbackSpeed * m_KnockBackMulti * Time.deltaTime;

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
        FreezeMovement(false);
    }

    private void ReturnCooldown()
    {
        float speed = (m_Speed + m_SpdBonus) * 0.1f;
        m_ActionTimer = (m_ActionCooldown - speed);
        Debug.Log(m_ActionTimer);
    }

    public void HealthUpdate()
    {
        if (m_HP > m_MaxHP)
        {
            m_HP = m_MaxHP;
        }
        else if(m_HP < 0)
        {
            m_HP = 0;
        }
        
        m_HealthBar.SetHealthBar((float)m_HP / (float)m_MaxHP);
        m_HealthBar.SetHealthText(m_HP, m_MaxHP);
    }

    public void ChangeStatBoost(int AtkBoost, int DefBoost, float SpdBoost)
    {
        m_AtkBonus += AtkBoost;
        m_DefBonus += DefBoost;
        m_SpdBonus += SpdBoost;

        if (m_AtkBonus > 0)
        {
            m_AtkBoostIcon.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            m_AtkBoostIcon.color = new Color(0f, 0f, 0f, 1f);
            m_AtkBonus = 0;
        }
        
        if (m_DefBonus > 0)
        {
            m_DefBoostIcon.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            m_DefBoostIcon.color = new Color(0f, 0f, 0f, 1f);
            m_DefBonus = 0;
        }
        
        if (m_SpdBonus > 0)
        {
            m_SpdBoostIcon.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            m_SpdBoostIcon.color = new Color(0f, 0f, 0f, 1f);
            m_SpdBonus = 0f;
        }
    }

    public void IncreaseEXP(int amount)
    {
        m_Exp += amount;
        CheckEXP();
    }

    private void CheckEXP()
    {
        if (m_Exp >= 100 && m_Level < 99)
        {
            int amount = Mathf.FloorToInt(m_Exp / 100);
            LevelUp(amount);
        }
    }
    
    private void LevelUp(int amount)
    {
        GameManager.instance.PlayAudio(m_LevelUpJingle);
        GameManager.instance.SpawnLevelFX(transform.position, transform);
        GameManager.instance.SpawnPopUp("LEVEL UP!", transform.position);
        
        m_Level += amount;

        int hpUp = 3 * amount;
        m_MaxHP += hpUp;
        m_HP += hpUp;

        int paraUp = 1 * amount;
        m_Attack += paraUp;
        m_Defence += paraUp;

        float spdUp = 0.01f * amount;
        m_Speed += spdUp;

        m_Exp = 0;
        
        HealthUpdate();
    }

    private void SetRollSpeed()
    {
        m_RollSpeed = 2 + m_Speed + m_SpdBonus;
    }
}
