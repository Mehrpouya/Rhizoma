using System.Collections;
using System.Collections.Generic;
using TinyGiantStudio.Text;
using UnityEngine;

public class RStateThinking : MonoBehaviour
{
    public Modular3DText rText;
    // Start is called before the first frame update
    void Start()
    {
        print("Thinking!");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K)) {
            print("adding k");
            rText.UpdateText(" k");
        }
    }
}
