using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBreath : MonoBehaviour
{
    public float breathSpeed;
    int sizeChange;
    private void Start() 
    {
        sizeChange = -1;
        transform.localScale = Vector3.one;
    }
    
    private void FixedUpdate() 
    {
        transform.localScale += new Vector3(0, sizeChange * Time.deltaTime * breathSpeed, 0);
        if(Mathf.Abs(0.99f - transform.localScale.y) >= 0.015)
        {
            sizeChange *= -1;
            transform.localScale += new Vector3(0, sizeChange * Time.deltaTime * breathSpeed, 0);
        }
    }

    private void OnDisable() 
    {
    transform.localScale = new Vector3(transform.localScale.x, 1,1);
    }

}

