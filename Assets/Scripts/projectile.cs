using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    public float speed = 10f;
    public Rigidbody2D rb;


    void Update()
    {
        rb.velocity = transform.right * speed;
    }

    void OnTriggerEnter2D()
    {
        Destroy(gameObject);
    }
}
