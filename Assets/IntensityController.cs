using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class IntensityController : MonoBehaviour
{
    [FormerlySerializedAs("speedAmplifier")] 
    public float motionAmplifier = 1.0f;
    
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Rotate(transform.up, movementSpeed*Time.deltaTime);
    }
}
