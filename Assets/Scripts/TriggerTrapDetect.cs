using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTrapDetect : MonoBehaviour
{
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player") || other.CompareTag("Enemy"))
        {
            animator.SetTrigger("Active");
        }
    }
}
