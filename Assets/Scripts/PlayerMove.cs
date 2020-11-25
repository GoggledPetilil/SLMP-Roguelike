using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Physics")]
    public float m_MoveSpeed;
    public Vector2 m_Mov;
    
    [Header("Components")]
    public Animator m_ani;
    public Rigidbody2D m_rb;
    
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        m_Mov.x = Input.GetAxisRaw("Horizontal");
        m_Mov.y = Input.GetAxisRaw("Vertical");
        
        m_ani.SetFloat("Horizontal", m_Mov.x);
        m_ani.SetFloat("Vertical", m_Mov.y);
        m_ani.SetFloat("Speed", m_Mov.sqrMagnitude);
    }

    void FixedUpdate()
    {
        m_rb.MovePosition(m_rb.position + m_Mov * m_MoveSpeed * Time.fixedDeltaTime);
    }
}
