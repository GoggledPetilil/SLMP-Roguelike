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
    public bool m_CanBuffHP; // If true, the item will increase max HP if player is already full.
    public int m_BuffAmount;
    
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
            string t = null;
            switch (m_type)
            {
                case Type.Money:
                    if (m_Worth == 0)
                    {
                        int max = 100 * GameManager.instance.m_Floor / 8;
                        m_Worth = Mathf.RoundToInt(GameManager.instance.GetRandomRange(2, max));
                    }
                    GameManager.instance.ChangeMoney(m_Worth);
                    t = "$" + m_Worth.ToString();
                    GameManager.instance.SpawnPopUp(t, GameManager.instance.m_Player.transform.position);
                break;
                case Type.Health:
                    PlayerMove player = GameManager.instance.m_Player;

                    
                    if (player.m_HP == player.m_MaxHP && m_CanBuffHP == true)
                    {
                        player.m_MaxHP += m_BuffAmount;
                        player.m_HP += m_HPRestore;
                        t = "+" + m_BuffAmount.ToString();
                    }
                    else
                    {
                        player.m_HP += m_HPRestore;
                        t = "+" + m_HPRestore.ToString();
                    }
                    
                    GameManager.instance.m_Player.HealthUpdate();
                    GameManager.instance.SpawnPopUp(t, GameManager.instance.m_Player.transform.position);
                    break;
                case Type.StatBoost:
                    GameManager.instance.m_Player.ChangeStatBoost(m_AtkBoost, m_DefBoost, m_SpdBoost);
                break;
            }

            GameManager.instance.m_Player.IncreaseEXP(1);
            GameManager.instance.PlayAudio(m_Collected);
            Destroy(gameObject);
        }
    }
}
