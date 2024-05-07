using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesBallMove : MonoBehaviour
{
    // Start is called before the first frame update
    Rigidbody2D rb;
    [SerializeField] float speed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
       if(transform.localPosition.y <= -3.79)
       {
            if(rb.velocity.x > 0)
            {
                rb.velocity = new Vector2(speed, rb.velocity.y);
            }
            else
            {
                rb.velocity = new Vector2(-speed, rb.velocity.y);
            }
       } 
    }
}
