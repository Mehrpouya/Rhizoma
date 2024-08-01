using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Numerics;
using Unity.VisualScripting;
//using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;


public enum Mode
{
    IsComposing,
    IsPlaying
}
public class RhizomaController : MonoBehaviour
{
    Rigidbody Body;
    // Start is called before the first frame update
    Vector3 inForce;

    public Mode currentMode;
    public bool automatic_recording ;

    public string FileNameToReadFrom = "composition.json";
    public string FileNameToWriteTo = "composition.json";
    

    RhyzomaRecordingMode MemoryRetainer;
    private FreeCam freeCam_;
    [Tooltip("Defines how often to save the position -- lower is better resolution")]
    public float timeInterval = 1.0f;

    public float amountToRewindS = 1.0f;

    public int currentReadIndex = 0;
    //float moveSpeed = 2;
    //float rotationSpeed = 4;
    //float runningSpeed;
    //float vaxis, haxis;
    //public bool isJumping, isJumpingAlt, isGrounded = false;
    //Vector3 movement;


    //public float speed = 6.0F;
    //public float jumpSpeed = 8.0F;
    //public float gravity = 20.0F;
    // Drag & Drop the camera in this field, in the inspector
    // public Transform cameraTransform;

    private float timer;
  
    void Start()
    {
        Body = GetComponent<Rigidbody>(); //Gain access to the body
        freeCam_ = GetComponent<FreeCam>();
        MemoryRetainer = GetComponent<RhyzomaRecordingMode>();
        timer = Time.time;
        
        freeCam_.enabled = currentMode==Mode.IsComposing;
        if (currentMode== Mode.IsPlaying)
        {
            MemoryRetainer.ReadData(FileNameToReadFrom,ref timeInterval);
            Ponder();
        }
    }

    

    void Notice(float minEffort,float maxEffort,float timeToPonder) { 
     inForce = new Vector3(Random.Range(minEffort, maxEffort), Random.Range(minEffort, maxEffort), UnityEngine.Random.Range(minEffort,maxEffort));
    }

    void AttendTo() {
        Body.AddForce(inForce, ForceMode.Impulse);
        print(inForce);
    }
    
    
    
    // Update is called once per frame
    void Update()
    {
        freeCam_.enabled = currentMode==Mode.IsComposing;
        if(currentMode==Mode.IsComposing)
        {
            if (automatic_recording)
            {
                if (Time.time - timer > timeInterval)
                {
                    timer = Time.time;
                    MemoryRetainer.Record_RMemory(transform.position,transform.rotation, 5,1);
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.R))
                {
                    MemoryRetainer.Record_RMemory(transform.position,transform.rotation, 5,1);
                }
                
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                MemoryRetainer.SaveStringToText(FileNameToWriteTo, timeInterval);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                MemoryRetainer.ReadData(FileNameToReadFrom, ref timeInterval);
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                transform.position = MemoryRetainer.Rewind(amountToRewindS, timeInterval);
                
            }
            
            if(Input.GetKeyDown(KeyCode.P))
                AudioController.Instance.SyncAllAudioWithAliveTime();

            
        }  
        
        
    }

   

    void Ponder() {
        StartCoroutine(PonderFromMemory());
    }


    private void OnDisable()
    {
        if (currentMode==Mode.IsComposing && AreYouSureButton.CreateWizard())
        {
            MemoryRetainer.SaveStringToText(FileNameToWriteTo, timeInterval);
        };
    }


    IEnumerator PonderFromMemory()
{
    var prevPos = transform.position;
    var newPos = transform.position;
    var prevRot = transform.rotation;
    var newRot = transform.rotation;
    while (true)
    {
        if (Time.time - timer > timeInterval)
        {
            timer = Time.time;
            if (currentReadIndex >= MemoryRetainer.RMemory.Count-1)
                break;

            prevPos = MemoryRetainer.RMemory[currentReadIndex].memoryPlace;
            newPos = MemoryRetainer.RMemory[currentReadIndex+1].memoryPlace;
            prevRot = MemoryRetainer.RMemory[currentReadIndex].memoryOrientation;
            newRot = MemoryRetainer.RMemory[currentReadIndex+1].memoryOrientation;

            
            currentReadIndex++;

        }

        transform.position = Vector3.Lerp(prevPos, newPos, (Time.time - timer)/timeInterval);
        transform.rotation = Quaternion.Lerp(prevRot, newRot, (Time.time - timer)/timeInterval);
        
        
        yield return null;
        
        
    }
}
}
