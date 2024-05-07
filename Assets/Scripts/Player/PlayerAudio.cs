using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    AudioSource audioSource;
    PlayerMove playerMove;
    [SerializeField] AudioClip walk, jump, match;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerMove = GetComponent<PlayerMove>();
    }

    public void WalkAudio()
    {
        if(playerMove.rb.velocity.x <= 0.1 || !playerMove.isOnGround)
        {
            audioSource.Stop();
        }
        audioSource.clip = walk;
        audioSource.Play();
    }

    public void JumpAudio()
    {
        if(audioSource.isPlaying)
            return;
        audioSource.clip = jump;
        audioSource.Play();
    }

    public void MatchAudio()
    {
        audioSource.clip = match;
        audioSource.Play();
    }
}
