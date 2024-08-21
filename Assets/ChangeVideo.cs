using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeVideo : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter(Collision col)
    {
        var smc = col.transform.GetComponent<SoundMotionController>();
        if (smc)
        {
            if (smc.clip)
            {
                VideoManager.Instance.Fade(smc.clip, smc.fadeToColor, smc.fadeSpeed);
            }
        }
    }
}
