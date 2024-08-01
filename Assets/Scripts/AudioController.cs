using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance{ get; private set; }
    public Vector3Int StartTimeHHMMSS;
    public Vector3Int StartDateDDMMYYYY=new Vector3Int(01,08,2024);
    private System.DateTime StartTime;
    public AudioSource[] allAudios;
    void Awake()
    {
        Instance = this;
         
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        }

        StartTime = new System.DateTime(StartDateDDMMYYYY.z,StartDateDDMMYYYY.y,StartDateDDMMYYYY.x,StartTimeHHMMSS.x,StartTimeHHMMSS.y,StartTimeHHMMSS.z);
        allAudios = GetComponentsInChildren<AudioSource>();
    }

    private void Start()
    {
        SyncAllAudioWithAliveTime();
    }

    public void SetPitch(float value)
    {
        foreach (var aud in allAudios)
        {
            aud.pitch = value;
        }
        
    }
    
    public void SyncAllAudioWithAliveTime(){
        var aliveFor = Math.Abs((System.DateTime.Now - StartTime).TotalSeconds);
        foreach (var aud in allAudios)
        {
            var communalTime = aliveFor % aud.clip.length;
            print(aliveFor);
            aud.time = (float)communalTime;
            
        }
    }

}
