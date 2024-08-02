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

    public float motionMultiplier = 1;
    public AudioSource[] allAudios;
    private float[] volumes_, pitches_;
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
        volumes_ = new float[allAudios.Length];
        pitches_ = new float[allAudios.Length];
        
        for (int i = 0; i < allAudios.Length;i++)
        {
            volumes_[i] = allAudios[i].volume;
            pitches_[i] = allAudios[i].pitch;
        }
    }

    private void Start()
    {
        SyncAllAudioWithAliveTime();
    }

    public void SetPitch(float value)
    {
        for (int i = 0 ;i < allAudios.Length;i++)
            allAudios[i].pitch = value + pitches_[i];
    }
    
    public void SetVolume(float value)
    {
        for (int i = 0 ;i < allAudios.Length;i++)
            allAudios[i].volume = value + volumes_[i];
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
