using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEffects : MonoBehaviour
{
    public float m_DestroyTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, m_DestroyTime);
    }
}
