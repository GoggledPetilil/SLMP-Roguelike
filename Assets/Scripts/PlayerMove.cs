using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float m_Speed;
    public Rigidbody2D m_RB;
    private Vector2 m_Mov;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_Mov.x = Input.GetAxisRaw("Horizontal");
        m_Mov.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        m_RB.MovePosition(m_RB.position + m_Mov * m_Speed * Time.fixedDeltaTime);
    }
    
}
