using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public enum Type
    {
        Money,
        Health,
        StatBoost
    }
    
    public Type m_type;

    [Header("Money Parameters")] 
    public int m_Worth;
    
    [Header("Health Parameters")] 
    public int m_HPRestore;
    
    [Header("Stat Parameters")] 
    public int m_AtkBoost;
    public int m_DefBoost;
    public float m_SpdBoost;

    [Header("Audio Components")] 
    public AudioClip m_Collected;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            switch (m_type)
            {
                case Type.Money:
                    if (m_Worth == 0)
                    {
                        int max = 100 * GameManager.instance.m_Floor / 8;
                        m_Worth = Mathf.RoundToInt(GameManager.instance.GetRandomRange(2, max));
                    }

                    GameManager.instance.ChangeMoney(m_Worth);
                break;
                case Type.Health:
                    GameManager.instance.m_Player.ChangeHealth(m_HPRestore);
                    break;
                case Type.StatBoost:
                    GameManager.instance.m_Player.ChangeStatBoost(m_AtkBoost, m_DefBoost, m_SpdBoost);
                break;
            }

            GameManager.instance.PlayAudio(m_Collected);
            Destroy(gameObject);
        }
    }
}
