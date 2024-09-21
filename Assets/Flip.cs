using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Flip : MonoBehaviour
{
    private bool m_FacingRight = true;
    float horizontalInput = 0f;
    
    void Start()
    {
        
    }

    void Update()
    {
        if(horizontalInput != 0f)
        {
            Flipo();
        }
    }

    private void Flipo()
    {
        if(m_FacingRight == true && horizontalInput < 0f)
        {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        }
        
        else if(m_FacingRight == false && horizontalInput > 0f)
        {
        m_FacingRight = !m_FacingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        }
        else
        {

        }

    }
    
}
