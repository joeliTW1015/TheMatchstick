using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    Transform cameraTra;
    Vector3 lastCamPos;
    float textureUnitSize;
    [SerializeField] Vector2 delay = new Vector2(1, 0);
    void Start()
    {
        cameraTra = Camera.main.transform;
        lastCamPos = cameraTra.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSize = texture.width / sprite.pixelsPerUnit;
    }   
    
    void FixedUpdate()
    {
        Vector3 deltaMovement = (cameraTra.position - lastCamPos);
        transform.position += new Vector3(deltaMovement.x * delay.x, deltaMovement.y * delay.y, 0);
        lastCamPos = cameraTra.position;

        if(Mathf.Abs(cameraTra.position.x - transform.position.x) >= textureUnitSize)
        {
            float offsetPositionX = (cameraTra.position.x - transform.position.x) % textureUnitSize;
            transform.position = new Vector3(cameraTra.position.x + offsetPositionX, transform.position.y);
        }
    }

    
}
