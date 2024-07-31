using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RhizomaRetainer
{
    public Vector3 memoryPlace = Vector3.zero;
    public float volume;
    // Start is called before the first frame update
    public RhizomaRetainer() { }

    public RhizomaRetainer(Vector3 _pos, float _vol)
    {
        memoryPlace = _pos;
        volume = _vol;
    }
}
