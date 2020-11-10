using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRoom : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        RoomTemplates m_RT = GameObject.FindGameObjectWithTag("Rooms").GetComponent<RoomTemplates>();
        m_RT.m_RoomsList.Add(this.gameObject);
    }
}
