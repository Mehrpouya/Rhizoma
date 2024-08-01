using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysFaceCamera : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private Transform mainCam;
 
    private void OnEnable() {
        mainCam =  FindObjectOfType<Camera>().transform;
        Debug.Log("Main Cam = " + mainCam.name);
    }
    
    private void LateUpdate() {
        transform.LookAt(mainCam);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
