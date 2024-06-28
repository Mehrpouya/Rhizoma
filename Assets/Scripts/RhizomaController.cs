using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Android.Gradle.Manifest;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;
public class RhizomaController : MonoBehaviour
{
    Rigidbody Body;
    // Start is called before the first frame update
    Vector3 inForce;

    public bool Is_Composing=false;


    RhyzomaRecordingMode MemoryRetainer;

    float moveSpeed = 2;
    float rotationSpeed = 4;
    float runningSpeed;
    float vaxis, haxis;
    public bool isJumping, isJumpingAlt, isGrounded = false;
    Vector3 movement;


    public float speed = 6.0F;
    public float jumpSpeed = 8.0F;
    public float gravity = 20.0F;
    // Drag & Drop the camera in this field, in the inspector
    public Transform cameraTransform;
    private Vector3 moveDirection = Vector3.zero;

    Vector3 prevGaze = Vector3.zero;

    void FixedUpdate()
    {
        if (Is_Composing)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            moveDirection = cameraTransform.TransformDirection(moveDirection);
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
                moveDirection.y = jumpSpeed;

            moveDirection.y -= gravity * Time.deltaTime;
            Body.AddForce(moveDirection * Time.deltaTime);
            //float xAngle = prevGaze.x - Input.mousePosition.x;
            //float yAngle = prevGaze.y - Input.mousePosition.y;
            //transform.Rotate(0, xAngle, 0);
            //transform.Rotate(yAngle, 0, 0);
            prevGaze = Input.mousePosition;

        }


    }
    void Start()
    {
        Body = GetComponent<Rigidbody>(); //Gain access to the body
        cameraTransform = Camera.main.transform;
        Vector3 prevGaze = Input.mousePosition;
        if (!Is_Composing)
        {
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
      if(Is_Composing)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                MemoryRetainer.Record_RMemory(transform.position, 5);
            }
            if (Input.GetKeyDown(KeyCode.T))
            {
                MemoryRetainer.SaveStringToText();
            }
        }  
    }

    void ApplyManualForce()
    {


    }

    void Ponder() {
        StartCoroutine(MyCoroutine());
    }

    void EnableManualControl() {
    
    }

        

IEnumerator MyCoroutine()
{
    while (true)
    {
            AttendTo();
            Notice(-3, 3, 2);
            // wait for seconds
            yield return new WaitForSeconds(2f);
    }
}
}
