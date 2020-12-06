using System;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class Entity : MonoBehaviour
{
    [Header("Entity Parameters")] 
    public int m_Level;
    public int m_MaxHP;
    public int m_HP;
    public int m_Attack;
    public int m_Defence;
    public float m_Speed;
    public float m_KnockbackSpeed;
    public float m_InvincibilityTime;
    public bool m_IsInvincible;

    [Header("Entity Multipliers")] 
    public int m_AtkBonus;  
    public int m_DefBonus;
    public int m_SpdBonus; 
    
    [Header("Entity Physics")]
    public Vector2 m_MovDir;
    public Vector2 m_ActionDir;
    public bool m_Damaged;
    public Vector2 m_KnockbackDir;
    
    [Header("Entity Components")] 
    public Animator m_ani;
    public Rigidbody2D m_rb;
    public GameObject m_CharacterSprite;
    public SpriteRenderer m_Sprite;
    public ParticleSystem m_Dust;
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.root != transform.root && other.gameObject.tag == "Attack" && m_Damaged == false && m_IsInvincible == false)
        {
            Entity otherEntity = other.transform.root.GetComponent<Entity>();
            int m_EnemyAtk = otherEntity.m_Attack + otherEntity.m_AtkBonus;
            
            m_KnockbackSpeed = 1.8f + m_EnemyAtk * 0.1f;
            m_KnockbackDir.x = Mathf.Round(otherEntity.m_ActionDir.x);
            m_KnockbackDir.y = Mathf.Round(otherEntity.m_ActionDir.y);
            
            m_Sprite.color = new Color(0f, 0f, 0f, 0.5f);
            TakeDamage(m_EnemyAtk);
            KnockbackStartUp();
        }
    }

    IEnumerator Squeeze(float m_SqueezeX, float m_SqueezeY, float m_sec)
    {
        Vector3 m_OriginSize = Vector3.one;
        Vector3 m_NewSize = new Vector3(m_SqueezeX, m_SqueezeY, m_OriginSize.z);
        float t = 0f;
        while (t <= 1)
        {
            t += Time.deltaTime / m_sec;
            m_CharacterSprite.transform.localScale = Vector3.Lerp(m_OriginSize, m_NewSize, t);
            yield return null;
        }
        t = 0f;
        while (t <= 1)
        {
            t += Time.deltaTime / m_sec;
            m_CharacterSprite.transform.localScale = Vector3.Lerp(m_NewSize, m_OriginSize, t);
            yield return null;
        }
    }

    // Because the Start method doesn't work.
    public void InitializeCharacter()
    {
        m_HP = m_MaxHP;

        m_MovDir = new Vector2(0, -1f);
        m_ActionDir = m_MovDir;
        
        m_rb = GetComponent<Rigidbody2D>();
        m_Sprite = m_CharacterSprite.GetComponent<SpriteRenderer>();
    }
    
    public void MoveAni()
    {
        if (m_MovDir != Vector2.zero)
        {
            m_ani.SetFloat("Horizontal", m_MovDir.x);
            m_ani.SetFloat("Vertical", m_MovDir.y);
        }
        m_ani.SetFloat("Speed", m_MovDir.sqrMagnitude);
    }
    
    public void ActionDirToDirection()
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

    public void TakeDamage(int m_EnemyAttack)
    {
        int damage = m_EnemyAttack - (m_Defence + m_DefBonus);
        if (damage < 1)
        {
            damage = 1;
        }
        else if (damage >= m_HP)
        {
            damage = damage * 3 + m_EnemyAttack;
            // To make the lethal hit extra flashy.
        }
        
        Debug.Log(this.gameObject.name + " took " + damage + " damage.");

        m_HP -= damage;

        if (m_HP < 1)
        {
            m_HP = 0;
            Death();
        }
    }

    public void CreateDust()
    {
        m_Dust.Play();
    }
    
    public void StopDust()
    {
        m_Dust.Stop();
    }

    public void KnockbackStartUp()
    {
        CreateDust();
        m_Damaged = true;
        m_InvincibilityTime = 1.5f + (m_Defence + m_DefBonus) / 100;
        StartCoroutine(Squeeze(0.5f, 1.2f, 0.01f));
    }

    public void ReduceInvincibility()
    {
        if (m_InvincibilityTime > 0)
        {
            m_InvincibilityTime -= 1 * Time.deltaTime;
        }
        else
        {
            m_Damaged = false;
            m_Sprite.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    public void Death()
    {
        if (this.gameObject.tag == "Entity")
        {
            Destroy(this.gameObject);
        }
        else if (this.gameObject.tag == "Player")
        {
            Debug.Log("Player has died!");
        }
        
    }

    public void LevelUp()
    {
        m_Level++;

        int hpUp = 10;
        m_MaxHP += hpUp;
        m_HP += hpUp;

        int paraUp = 1;
        m_Attack += paraUp;
        m_Defence += paraUp;

        float spdUp = 0.01f;
        m_Speed += spdUp;
    }
}
