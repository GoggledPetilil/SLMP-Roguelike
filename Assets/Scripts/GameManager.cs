using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [Header("Essentials")]
    public static GameManager instance;
    public AudioSource m_AudioSource;
    public PlayerMove m_Player;
    public int m_Money;
    
    [Header("Effects")]
    public Animator m_Screen;
    public GameObject m_Explosion;
    public GameObject m_PopUpText;
    
    [Header("Dungeon")]
    public int m_Floor;
    public int m_EnemiesKilled;
    public bool m_HiddenStairsSpawned;
    
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

    public void IncreaseFloor(int amount)
    {
        m_Floor += amount;
        UpdateFloorText();
    }
    
    public void UpdateFloorText()
    {
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

    public void SpawnExplosion(Vector3 pos)
    {
        Instantiate(m_Explosion, pos, Quaternion.identity);
    }
    
    public void SpawnPopUp(string popText, Vector3 pos)
    {
        GameObject popUp = Instantiate(m_PopUpText, pos, Quaternion.identity) as GameObject;
        popUp.transform.GetChild(0).GetComponent<TextMesh>().text = popText;
    }

    public void FreezeAllEntities(bool isFrozen)
    {
        List<GameObject> allEntities = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));
        allEntities.Add(m_Player.gameObject);
        foreach (var go in allEntities)
        {
            go.GetComponent<Entity>().m_CanMove = !isFrozen;
        }
    }
}
