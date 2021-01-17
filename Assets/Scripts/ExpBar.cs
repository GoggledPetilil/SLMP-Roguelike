using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpBar : MonoBehaviour
{
    public RectTransform m_WhiteBar;
    
    public void SetExpBar(float sizeNormalized)
    {
        m_WhiteBar.localScale = new Vector3(sizeNormalized, 1f);
    }
}
