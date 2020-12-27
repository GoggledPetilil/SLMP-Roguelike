using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public RectTransform m_WhiteBar;
    public Text m_HPText;

    public void SetHealthBar(float sizeNormalized)
    {
        m_WhiteBar.localScale = new Vector3(sizeNormalized, 1f);
    }

    public void SetHealthText(int currentHP, int maxHP)
    {
        m_HPText.text = currentHP.ToString() + "/" + maxHP.ToString();
    }
}
