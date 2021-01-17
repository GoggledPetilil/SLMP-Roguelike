using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random=UnityEngine.Random;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Essentials")]
    public static GameManager instance;
    public AudioSource m_Music;
    public AudioSource m_AudioSource;
    public PlayerMove m_Player;
    public Canvas m_Canvas;
    public int m_Money;
    
    [Header("Effects")]
    public Animator m_Screen;
    public GameObject m_PopUpText;
    public GameObject m_Explosion;
    public GameObject m_LevelUpFX;
    public float m_ScreenFadeTime = 0.5f;

    [Header("Dungeon")] 
    public GameObject m_DungeonGenerator;
    public int m_Floor;
    public int m_EnemiesKilled;
    public bool m_HiddenStairsSpawned;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            m_Floor = 0;
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

    public IEnumerator ScreenFadeIn()
    {
        CanvasToCam();
        m_Screen.Play("Fade_In");
        yield return new WaitForSeconds(m_ScreenFadeTime);
    }
    
    public IEnumerator ScreenFadeOut()
    {
        CanvasToCam();
        m_Screen.Play("Fade_Out");
        yield return new WaitForSeconds(m_ScreenFadeTime);
    }

    private void CanvasToCam()
    {
        if (m_Canvas == null)
        {
            m_Canvas = GameObject.Find("GM Canvas").GetComponent<Canvas>();
        }
        if (m_Canvas.worldCamera == null)
        {
            m_Canvas.worldCamera = Camera.main;
        }
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

    public void SpawnLevelFX(Vector3 pos, Transform parent)
    {
        Instantiate(m_LevelUpFX, pos, Quaternion.identity, parent);
    }

    public void FreezeAllEntities(bool isFrozen)
    {
        List<GameObject> allEntities = new List<GameObject>(GameObject.FindGameObjectsWithTag("Entity"));
        allEntities.Add(m_Player.gameObject);
        m_Player.m_CanMove = !isFrozen;
        foreach (var go in allEntities)
        {
            go.GetComponent<Entity>().m_CanMove = !isFrozen;
            if (isFrozen)
            {
                go.GetComponent<Entity>().m_MovDir = Vector2.zero;
                go.GetComponent<Entity>().m_ActionDir = Vector2.zero;
                go.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            }
            else
            {
                go.GetComponent<Entity>().m_MovDir = new Vector2(0, -1f);
                go.GetComponent<Entity>().m_ActionDir = go.GetComponent<Entity>().m_MovDir;
            }
        }
    }

    public void EnableDungeon(bool state)
    {
        m_DungeonGenerator.SetActive(state);
    }

    public void HidePlayer(bool state)
    {
        m_Player.gameObject.SetActive(state);
    }

    public void SwitchScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
