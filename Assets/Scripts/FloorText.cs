using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            int i = GameManager.instance.m_Floor;
            Text t = GetComponent<Text>();
            t.text = i.ToString() + "F";
        }
        else
        {
            Text t = GetComponent<Text>();
            t.text = "1F";
        }
        
    }
}
