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

    [Header("Entity Multipliers")] 
    public int m_AtkBonus;  
    public int m_DefBonus;
    public float m_SpdBonus;

    [Header("Entity Physics")] 
    public bool m_CanMove;
    public Vector2 m_MovDir;
    public Vector2 m_ActionDir;
    public Vector2 m_KnockbackDir;
    public float m_KnockbackSpeed;
    public bool m_Invincible;
    
    [Header("Entity Components")] 
    public Animator m_ani;
    public Rigidbody2D m_rb;
    public GameObject m_CharacterSprite;
    public SpriteRenderer m_Sprite;
    public ParticleSystem m_Dust;
    public AudioSource m_AudioSource;

    [Header("Entity Audio")] 
    public AudioClip m_Hurt;
    public AudioClip m_HurtBadly;
    public AudioClip m_Death;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.transform.root.tag != transform.root.tag && other.gameObject.tag == "Attack" && m_Invincible == false)
        {
            Debug.Log(gameObject.transform.root.name + " got hit by " + other.transform.root.name);
            Entity otherEntity = other.transform.root.GetComponent<Entity>();
            int m_EnemyAtk = otherEntity.m_Attack + otherEntity.m_AtkBonus;
            
            m_KnockbackSpeed = 1.8f + m_EnemyAtk * 0.1f;
            m_KnockbackDir.x = Mathf.Round(otherEntity.m_ActionDir.x);
            m_KnockbackDir.y = Mathf.Round(otherEntity.m_ActionDir.y);

            TakeDamage(m_EnemyAtk);
            if (gameObject.tag == "Player")
            {
                GameManager.instance.m_Player.HealthUpdate();
                GameManager.instance.m_Player.ChangeStatBoost(Mathf.FloorToInt(-m_AtkBonus / 2), -1, 0f);
            }
            KnockbackStartUp();
            float entityMultiplier = 1f;
            if (transform.root.CompareTag("Entity"))
            {
                entityMultiplier = 0.5f;
            }
            StartCoroutine(InvincibleFrames((1.5f + (m_Defence + m_DefBonus) / 100) * entityMultiplier));
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

    public IEnumerator InvincibleFrames(float time)
    {
        m_Invincible = true;
        if (transform.root.CompareTag("Player"))
        {
            Physics2D.IgnoreLayerCollision(8, 8, true);
        }
        m_Sprite.color = new Color(1f, 1f, 1f, 0.5f);
        yield return new WaitForSeconds(time);
        if (transform.root.CompareTag("Player"))
        {
            Physics2D.IgnoreLayerCollision(8, 8, false);
        }
        m_Sprite.color = new Color(1f, 1f, 1f, 1f);
        m_Invincible = false;
        yield return null;
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
        m_AudioSource.clip = m_Hurt;
        
        int damage = m_EnemyAttack - (m_Defence + m_DefBonus);
        if (damage < 1)
        {
            damage = 1;
        }
        else if (damage >= m_HP)
        {
            m_AudioSource.clip = m_HurtBadly;
            damage = damage * 3 + m_EnemyAttack;
            // To make the lethal hit extra flashy.
        }
        
        m_AudioSource.Play();
        m_HP -= damage;
        string t = "-" + damage.ToString();
        GameManager.instance.SpawnPopUp(t, transform.position);

        if (m_HP < 1)
        {
            m_HP = 0;
            GameManager.instance.PlayAudio(m_Death);
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
        StartCoroutine(Squeeze(0.5f, 1.2f, 0.01f));
    }

    public void Death()
    {
        GameManager.instance.SpawnExplosion(transform.position);
        FreezeMovement(true);
        m_Sprite.color = new Color(1f, 1f, 1f, 0f);
        float destroyDelay = 0.3f;

        if (this.gameObject.tag == "Entity")
        {
            GameManager.instance.m_EnemiesKilled++;
            
            int bonusEXP = (m_Level - GameManager.instance.m_Player.m_Level) * 10;
            GameManager.instance.m_Player.IncreaseEXP(9 + bonusEXP);
            
            EnemySpawner m_Generator = GameObject.FindGameObjectWithTag("Rooms").GetComponent<EnemySpawner>();
            m_Generator.RemoveDeadEnemies();
            
            Destroy(gameObject, destroyDelay);
        }
        else if (this.gameObject.tag == "Player")
        {
            Debug.Log("Player Died");
            RoomTemplates m_RT;
            m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
            m_RT.ResetDungeon();
            
            GameManager.instance.m_Floor = 1;
            GameManager.instance.UpdateFloorText();

            GameManager.instance.m_Player.m_HP = GameManager.instance.m_Player.m_MaxHP;
            GameManager.instance.m_Player.HealthUpdate();
        }
    }

    public void FreezeMovement(bool state)
    {
        if (state)
        {
            m_rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }
        else
        {
            m_rb.constraints = RigidbodyConstraints2D.None;
        }
        m_rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        m_MovDir = Vector2.zero;
    }

    public void PlayAudio(AudioClip audioClip)
    {
        m_AudioSource.clip = audioClip;
        m_AudioSource.Play();
    }
}
