using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 


public class RepeatDetector : MonoBehaviour
{ 
    public List<int> triggeredDialogues = new List<int>(20); 
    public List<int> pickedMatchBox = new List<int>(20); 
    private void Start()
    {
        for(int i = 0; i < 20; i ++)
        {
            triggeredDialogues.Add(-1);
            pickedMatchBox.Add(-1);
        }
        DontDestroyOnLoad(this.gameObject);
    }
}
