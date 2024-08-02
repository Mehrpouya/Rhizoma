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

    public float PitchOffset=0.0f;
    public float VolumeOffset=0.0f;
    private float pitchOffsetOld_, volumeOffsetOld_;
    
    RhizomaRecorder MemoryRetainer;
    private FreeCam freeCam_;
    
    
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
        MemoryRetainer = GetComponent<RhizomaRecorder>();
        timer = Time.time;
        
        freeCam_.enabled = currentMode==Mode.IsComposing;
        if (currentMode== Mode.IsPlaying)
        {
            MemoryRetainer.ReadData(FileNameToReadFrom);
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
            updateAudio();
            
            if (automatic_recording)
            {
                if (Time.time - timer > MemoryRetainer.timeInterval)
                {
                    timer = Time.time;
                    MemoryRetainer.Record_RMemory(transform.position,transform.rotation, VolumeOffset,PitchOffset);
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
                MemoryRetainer.SaveStringToText(FileNameToWriteTo);
            }
            if (Input.GetKeyDown(KeyCode.Y))
            {
                MemoryRetainer.ReadData(FileNameToReadFrom);
            }
            
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                transform.position = MemoryRetainer.Rewind();
                
            }
            
            if(Input.GetKeyDown(KeyCode.P))
                AudioController.Instance.SyncAllAudioWithAliveTime();

            
        }  
        
        
    }


    void updateAudio()
    {
        if (Math.Abs(volumeOffsetOld_ - VolumeOffset)>0.0001f)
        {
            AudioController.Instance.SetVolume(VolumeOffset);
            volumeOffsetOld_ = VolumeOffset;
        }
        
        
        if (Math.Abs(pitchOffsetOld_ - PitchOffset)>0.0001f)
        {
            AudioController.Instance.SetPitch(PitchOffset);
            pitchOffsetOld_ = PitchOffset;
        }
    }
    void Ponder() {
        StartCoroutine(PonderFromMemory());
    }


    private void OnDisable()
    {
        if (currentMode==Mode.IsComposing && AreYouSureButton.CreateWizard())
        {
            MemoryRetainer.SaveStringToText(FileNameToWriteTo);
        };
    }


    IEnumerator PonderFromMemory()
    {
     
        var currentState = MemoryRetainer.GetState();
        var nextState = MemoryRetainer.GetState(1);
        while (true)
        {
            if (Time.time - timer > MemoryRetainer.timeInterval)
            {
                timer = Time.time;
                if (MemoryRetainer.currentReadIndex >= MemoryRetainer.RMemory.Count-1)
                    break;

                currentState = MemoryRetainer.GetState();
                nextState = MemoryRetainer.GetState(1);
                
                
                
                MemoryRetainer.currentReadIndex++;

            }

            float t = (Time.time - timer) / MemoryRetainer.timeInterval;
            
            transform.position = Vector3.Lerp(currentState.memoryPlace, nextState.memoryPlace, t);
            transform.rotation = Quaternion.Lerp(currentState.memoryOrientation, nextState.memoryOrientation, t);
            VolumeOffset = Mathf.Lerp(currentState.volume, nextState.volume, t);
            PitchOffset = Mathf.Lerp(currentState.pitch, nextState.pitch, t);
            
            updateAudio();
            
            yield return null;
            
            
        }
        
        
    }

    
}
