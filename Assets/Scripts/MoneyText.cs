using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyText : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.instance != null)
        {
            int i = GameManager.instance.m_Money;
            Text t = GetComponent<Text>();
            t.text = "$" + i.ToString();
        }
        else
        {
            Text t = GetComponent<Text>();
            t.text = "$0";
        }
    }
}
