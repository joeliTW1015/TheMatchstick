using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Water : MonoBehaviour
{
    BoxCollider2D boxColl;
    SpriteRenderer spRend;
    [SerializeField] AudioSource audioSourceA, audioSourceB;
    // Start is called before the first frame update
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        boxColl.size = new Vector2(spRend.size.x, 0.8f);
        audioSourceA.minDistance = spRend.size.x / 2;
        audioSourceA.maxDistance = audioSourceA.minDistance * 2;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.CompareTag("Player"))
        {
            audioSourceB.Play();
            other.GetComponent<PlayerDie>().TakeDamage();
        }
        else if(other.CompareTag("Enemy"))
        {
            other.GetComponent<OrcDie>().TakeDamage();
        }
    }
}
