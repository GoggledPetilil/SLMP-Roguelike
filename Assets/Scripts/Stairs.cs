using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Stairs : MonoBehaviour
{
    public AudioClip m_Descend;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.PlayAudio(m_Descend);
            StartCoroutine(Fade());
        }

        if (other.gameObject.tag == "Item" || other.gameObject.tag == "Decor")
        {
            Destroy(other.gameObject);
        }
    }

    private IEnumerator Fade()
    {
        GameManager.instance.ScreenFadeOut();
        yield return new WaitForSeconds(0.5f);
        RoomTemplates m_RT;
        m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        m_RT.ClearAll();

        PlayerMove p = GameManager.instance.m_Player;
        p.ChangeStatBoost(0, 0, -p.m_SpdBonus);
        GameManager.instance.IncreaseFloor();
            
        Destroy(this.gameObject);
    }
}
