using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpikeModifier : MonoBehaviour
{
    BoxCollider2D boxColl;
    SpriteRenderer spRend;
    
    // Start is called before the first frame update
    void Start()
    {
        boxColl = GetComponent<BoxCollider2D>();
        spRend = GetComponent<SpriteRenderer>();
        boxColl.size = new Vector2(spRend.size.x , 0.4f);
    }
    
}
