using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class RhizomaRetainer
{
    public Vector3 memoryPlace = Vector3.zero;
    public Quaternion memoryOrientation = quaternion.identity;
    public float pitch;
    public float volume;
    // Start is called before the first frame update
    public RhizomaRetainer() { }

    public RhizomaRetainer(Vector3 _pos, Quaternion _rot,float _vol, float _pitch)
    {
        memoryPlace = _pos;
        memoryOrientation = _rot;
        volume = _vol;
        pitch = _pitch;
    }
}
