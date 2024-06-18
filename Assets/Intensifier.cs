using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
public class Intensifier : MonoBehaviour
{
    Rigidbody Body;
    // Start is called before the first frame update
    Vector3 inForce;

    void Start()
    {
        Body = GetComponent<Rigidbody>(); //Gain access to the body
        Ponder();
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
       
    }

    void Ponder() {
        StartCoroutine(MyCoroutine());
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
