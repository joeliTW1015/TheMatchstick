using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class RedEye : MonoBehaviour
{
    [SerializeField] Sprite redEyeSprite1, redEyeSprite2;
    SpriteRenderer spriteRenderer;
    PlayerDie playerDie;
    private void Start() 
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerDie = FindObjectOfType<PlayerDie>();
        spriteRenderer.sprite = Random.Range(-1, 1) >= 0 ? redEyeSprite1 : redEyeSprite2;
    }

    private void Update() 
    {
        if(playerDie.timeInDark == 0)
        {
            DestroyRedEye();
        }
    }

    public void DestroyRedEye()
    {
        GameObject.Destroy(this.gameObject);
    }
}
