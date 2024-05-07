using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDamage : MonoBehaviour
{
    private void Start() 
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            if(this.tag == "Arrow")
                GameObject.Destroy(this.gameObject);

            other.GetComponent<PlayerDie>().TakeDamage();
        }
        else if(other.CompareTag("Enemy"))
        {
            if(this.tag == "Arrow")
                GameObject.Destroy(this.gameObject);
                
            other.GetComponent<OrcDie>().TakeDamage();
        }
        else if(other.gameObject.layer == 6 && this.tag == "Arrow")
            GameObject.Destroy(this.gameObject);
    }
}
