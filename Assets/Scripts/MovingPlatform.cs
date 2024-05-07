using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] List<Transform> movingPoints;
    [SerializeField] float speed;
    Rigidbody2D rb;
    int  targetPointIndex;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        transform.position = movingPoints[0].position;
        targetPointIndex = 1;
    }

    void FixedUpdate()
    {
        if((rb.position - new Vector2(movingPoints[targetPointIndex].position.x, movingPoints[targetPointIndex].position.y)).sqrMagnitude <= 0.1f)
        {
            targetPointIndex++;
            if(targetPointIndex == movingPoints.Count)
            {
                targetPointIndex = 0;
            }
        }

        //transform.position = Vector2.MoveTowards(rb.position, movingPoints[targetPointIndex].position, speed * Time.deltaTime);
        rb.position = Vector2.MoveTowards(rb.position, movingPoints[targetPointIndex].position, speed * Time.deltaTime);
    }

    /*
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Player"))
        {
            other.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other) 
    {
        if(other.collider.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
    */
}
