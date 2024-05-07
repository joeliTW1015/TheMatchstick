using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchBox : MonoBehaviour
{
    public int number;
    RepeatDetector repeatDetector;

    private void Start() 
    {
        repeatDetector = FindObjectOfType<RepeatDetector>();
        if(repeatDetector.pickedMatchBox[number] == 1)
        {
            GameObject.Destroy(this.gameObject);
        }
        else if(repeatDetector.pickedMatchBox[number] == -1)
        {
            repeatDetector.pickedMatchBox[number] = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            repeatDetector.pickedMatchBox[number] = 1;
            other.GetComponent<PlayerLight>().matchNum += 5;
            GameObject.Destroy(this.gameObject);
        }   
    }
}
