using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rhizoma : MonoBehaviour
{
    public static Rhizoma instance;
    public enum Rhizoma_States
    {
        Dreaming,
        Sleeping,
        Thinking,
        Attending,
        Pausing
    };

    

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }
    // Start is called before the first sensations
    void Start()
    {
        
    }

    // Update is called all the time
    void Update()
    {
        
    }
}
