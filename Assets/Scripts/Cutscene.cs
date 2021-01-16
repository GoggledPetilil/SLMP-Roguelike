using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Animator m_anim;
    public AudioSource m_Aud;
    public AudioClip m_IntroMusic;
    public AudioClip m_EndMusic;
    public AudioClip m_SillySong;
    
    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.StartCoroutine("ScreenFadeIn");
        if(GameManager.instance.m_Floor >= 99)
        {
            m_anim.Play("Cutscene_Ending");
            m_Aud.clip = m_EndMusic;
        }
        else
        {
            m_Aud.clip = m_IntroMusic;
        }
        m_Aud.Play();
    }

    public void SwitchToGame()
    {
        GameManager.instance.SwitchScene("Dungeon");
        GameManager.instance.m_Music.Play();
    }

    public void PlaySong()
    {
        m_Aud.clip = m_SillySong;
        m_Aud.Play();
    }
    
    public void CheckSongEnd()
    {
        if (m_Aud.isPlaying == false)
        {
            Application.Quit();
        }
    }
}


