using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlAudioMotion : MonoBehaviour
{
    public float movementSpeed = 1.0f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(transform.up, movementSpeed*Time.deltaTime);
    }
}
