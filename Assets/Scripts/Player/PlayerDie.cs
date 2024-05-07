using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    PlayerLight playerLight;
    PlayerMove playerMove;
    Animator animator;
    public bool isLit;
    public float timeInDark;
    [SerializeField] float darkToDieExposeTime;
    static public bool isDead;

    void Awake()
    {
        playerMove = GetComponent<PlayerMove>();
        playerLight = GetComponent<PlayerLight>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
        isDead = false;
        timeInDark = 0;
    }

    public void TakeDamage()
    {
        if(isDead)
            return;
        isDead = true;
        playerLight.matchLight.SetActive(false);
        playerMove.enabled = false;
        playerLight.enabled = false;
        animator.SetTrigger("damage");
        FindObjectOfType<AudioManager>().die.Play();
    }

    private void Update()
    {
        if(isDead)
        {
            playerMove.rb.velocity = new Vector2(0, playerMove.rb.velocity.y);
        }
        else if(isLit || playerLight.isLighting)
        {
            timeInDark = 0;
        }
        else
        {
            timeInDark += Time.deltaTime;
        }

        if(timeInDark >= darkToDieExposeTime)
        {
            TakeDamage();
        }
    }

    public void Die() //在動畫中觸發
    {
        Debug.Log("PlayerDie");
        playerLight.followLight.intensity = 0;
        gameManager.RestartFromSavePoint();
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if(other.CompareTag("Light"))
        {
            isLit = true;
        }    
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.CompareTag("Light"))
        {
            isLit = false;
        }    
    }
}
