using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiSpawnClog : MonoBehaviour
{
    public bool m_Spawned;

    // Start is called before the first frame update
    void Start()
    {
        Vector3 t = new Vector3(0, 0, 0);
        if (transform.position == t && m_Spawned == false)
        {
            Destroy(this.transform.parent.gameObject);
        }
        else
        {
            m_Spawned = true;
        }
    }
}
