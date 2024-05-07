using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class PlayerLight : MonoBehaviour
{
    public Light2D followLight;
    public GameObject matchLight;
    PlayerMove playerMove;
    public Animator animator;
    public bool isLighting;
    float timer;
    public float lightingTime;
    public int matchNum;
    // Start is called before the first frame update
    void Start()
    {
        playerMove = GetComponent<PlayerMove>();
        isLighting = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftShift) && !isLighting && playerMove.isOnGround && Mathf.Abs(playerMove.rb.velocity.x) <= 0.01f && matchNum > 0)
        {
            matchNum--;
            followLight.enabled = false;
            matchLight.SetActive(true);
            isLighting = true;
            animator.SetBool("lighting", true);
            timer = 0;
            playerMove.enabled = false;
        }
        else if(isLighting)
        {
            playerMove.rb.velocity = new Vector2(0, playerMove.rb.velocity.y);
            timer += Time.deltaTime;
            if(timer >= lightingTime || Input.GetAxisRaw("Horizontal") != 0 || Input.GetKeyDown(KeyCode.Space))
            {
                followLight.enabled = true;
                matchLight.SetActive(false);
                isLighting = false;
                animator.SetBool("lighting", false);
                playerMove.enabled = true;
            }
        }
    }
}
