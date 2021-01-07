using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerMove m_Player;
    public int m_Floor;
    public int m_Money;
    public AudioSource m_AudioSource;
    
    public int m_EnemiesKilled;
    public bool m_HiddenStairsSpawned;

    public Animator m_Screen;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            m_Floor = 1;
            m_Money = 0;
            
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void IncreaseFloor()
    {
        m_Floor++;
        GameObject go = GameObject.Find("Floor Text");
        Text t = go.GetComponent<Text>();
        t.text = m_Floor.ToString() + "F";
    }

    public void ChangeMoney(int amount)
    {
        m_Money += amount;
        GameObject go = GameObject.Find("Money Text");
        Text t = go.GetComponent<Text>();
        t.text = "$" + m_Money.ToString();
    }

    public float GetRandomRange(float min, float max)
    {
        float i = Random.Range(min, max);
        return i;
    }

    public void ScreenFadeIn()
    {
        m_Screen.Play("Fade_In");
    }
    
    public void ScreenFadeOut()
    {
        m_Screen.Play("Fade_Out");
    }

    public void PlayAudio(AudioClip audioClip)
    {
        m_AudioSource.clip = audioClip;
        m_AudioSource.Play();
    }
}
