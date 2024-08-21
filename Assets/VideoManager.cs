using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public static VideoManager Instance{ get; private set; }
    private VideoPlayer vp;
    public VideoClip vClip;
    private Camera eye;
    // Start is called before the first frame update
    void Start()
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
        
        
        eye = GetComponentInParent<Camera>();
        vp = GetComponent<VideoPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fade( VideoClip clip, Color? cq =null,float s = 1)
    {
        Color c = cq ?? Color.black;
        StartCoroutine(FadeNow(clip, c,s));
        
    }

    IEnumerator FadeNow(VideoClip clip, Color c,float s)
    {
        //yield return w
        //yield return new WaitForSeconds(s/2);
        eye.backgroundColor = c;
        float time = Time.unscaledTime;
        while (Time.unscaledTime <= time + (s / 2))
        {
            vp.targetCameraAlpha = Mathf.Lerp(1, 0, (Time.unscaledTime - time) / (s * 0.5f));
            yield return null;
            
        }
        
        // Switch clips
        vp.clip = clip;
        
        time = Time.unscaledTime;
        while (Time.unscaledTime <= time + (s / 2))
        {
            vp.targetCameraAlpha = Mathf.Lerp(0, 1, (Time.unscaledTime - time) / (s * 0.5f));
            yield return null;
            
        }

    }
}
