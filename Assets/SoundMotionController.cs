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
    
    private IntensityController parentController;
    // Start is called before the first frame update
    void Start()
    {
        parentController = GetComponentInParent<IntensityController>();
        
        par_mat = parentController.transform.localToWorldMatrix;
        localMat =par_mat.inverse * transform.localToWorldMatrix;
        axisRotation = Vector3.Cross(transform.position, parentController.transform.position);
    }

    // Update is called once per frame
    void Update()
    {

        var s = Time.deltaTime * speed * parentController.motionAmplifier *
                    AudioController.Instance.motionMultiplier;
        
        Quaternion rot = Quaternion.AngleAxis(s, axisRotation);
        var rotMat = Matrix4x4.Rotate(rot);
        par_mat = (par_mat * rotMat);
        var mat =   par_mat * localMat ;
        transform.position = mat.GetPosition();


    }
 
    private void OnEnable() {
        mainCam =  FindObjectOfType<Camera>().transform;
        Debug.Log("Main Cam = " + mainCam.name);
    }
    
    private void LateUpdate() {
        transform.LookAt(mainCam);
        transform.RotateAround(transform.position, transform.up, 180f);
    }
}
