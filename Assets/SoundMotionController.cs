using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class SoundMotionController : MonoBehaviour
{
    private Transform mainCam;

    private Matrix4x4 par_mat, localMat;

    private Vector3 axisRotation;

    public float speed = 1.0f;
    public float enhancement;

    public VideoClip clip;
    public float fadeSpeed = 1;
    public Color fadeToColor;
    public bool Looping = false;

    public Color[] LightColor;
    
    public bool TurnOnSteamer = false;
    public float SteamerTime = 1.0f;
    
    private IntensityController parentController;
    // Start is called before the first frame update
    void Start()
    {
        parentController = GetComponentInParent<IntensityController>();
        
        if (parentController)
        {
            par_mat = parentController.transform.localToWorldMatrix;
            localMat =par_mat.inverse * transform.localToWorldMatrix;
            axisRotation = Vector3.Cross(transform.position, parentController.transform.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        float intensitySpeed = 1.0f;
        if (parentController)
            intensitySpeed = parentController.motionAmplifier;
        
        var s = Time.deltaTime * speed * intensitySpeed *
                    AudioController.Instance.motionMultiplier;
        
        Quaternion rot = Quaternion.AngleAxis(s, axisRotation);
        var rotMat = Matrix4x4.Rotate(rot);
        par_mat = (par_mat * rotMat);
        var mat =   par_mat * localMat ;
        if(parentController)
            transform.position = mat.GetPosition();


    }
 
    private void OnEnable()
    {
        for (int i = 0; i < Camera.allCameras.Length; i++)
        {
            if (Camera.allCameras[i].targetDisplay == 0)
            {
                print("Camera = "+i);
                mainCam = Camera.allCameras[i].transform;
                break;
            }
        }
        //Debug.Log("Main Cam = " + mainCam.name);
    }
    
    private void LateUpdate() {
        transform.LookAt(mainCam);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
