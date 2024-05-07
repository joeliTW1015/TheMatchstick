using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void FixedUpdate() 
    {
        rb.velocity =  -transform.up * speed * Time.deltaTime;
        if(Mathf.Abs(transform.position.x) > 300 || Mathf.Abs(transform.position.y) > 300)
        {
            GameObject.Destroy(this.gameObject);
        }
    }
}
