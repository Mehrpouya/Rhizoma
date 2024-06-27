using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
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

    void FixedUpdate()
    {
        if (Is_Composing)
        {
            /*  Controller Mappings */
            vaxis = Input.GetAxis("Vertical");
            haxis = Input.GetAxis("Horizontal");
            isJumping = Input.GetButton("Jump");
            isJumpingAlt = Input.GetKey(KeyCode.Joystick1Button0);

            //Simplified...
            runningSpeed = vaxis;


            if (isGrounded)
            {
                movement = new Vector3(0, 0f, runningSpeed * 8);        // Multiplier of 8 seems to work well with Rigidbody Mass of 1.
                movement = transform.TransformDirection(movement);      // transform correction A.K.A. "Move the way we are facing"
            }
            else
            {
                movement *= 0.70f;                                      // Dampen the movement vector while mid-air
            }

            GetComponent<Rigidbody>().AddForce(movement * moveSpeed);   // Movement Force


            if ((isJumping || isJumpingAlt) && isGrounded)
            {
                Debug.Log(this.ToString() + " isJumping = " + isJumping);
                Body.AddForce(Vector3.up * 150);
            }



            if ((Input.GetAxis("Vertical") != 0f || Input.GetAxis("Horizontal") != 0f) && !isJumping && isGrounded)
            {
                if (Input.GetAxis("Vertical") >= 0)
                    transform.Rotate(new Vector3(0, haxis * rotationSpeed, 0));
                else
                    transform.Rotate(new Vector3(0, -haxis * rotationSpeed, 0));

            }
        }

    }
    void Start()
    {
        Body = GetComponent<Rigidbody>(); //Gain access to the body
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
