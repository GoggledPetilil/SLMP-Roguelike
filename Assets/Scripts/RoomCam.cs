using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class RoomCam : MonoBehaviour
{
    private Vector3 m_ThisPos;
    private bool m_Scroll = false; // Tells the engine to scroll to this or not.
    public bool m_Focused; // Bool to see if the screen is currently focused on this room.
    private int m_Speed = 11;

    // Update is called once per frame
    void Update()
    {
        if (m_Scroll == true && Camera.main.transform.position != m_ThisPos)
        {
            Camera.main.transform.position = Vector3.LerpUnclamped(Camera.main.transform.position, m_ThisPos, m_Speed * Time.deltaTime);
            m_Focused = true;
        }
        else
        {
            m_Scroll = false;
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_ThisPos = new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z);
            m_Scroll = true;
            m_Focused = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            m_Scroll = false;
            m_Focused = false;
        }
    }
}
