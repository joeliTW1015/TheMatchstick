using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcDie : MonoBehaviour
{
    [SerializeField] Animator animator;
    public void TakeDamage()
    {
        animator.SetTrigger("die");
        GetComponent<OrcAI>().enabled = false;
        GetComponent<OrcDetect>().enabled = false;
        Invoke("Die", 1.2f);
    }

    public void Die()
    {
        GameObject.Destroy(this.gameObject);
    }
}
