using System.Collections;using System.Collections.Generic;
using UnityEngine;

public class RhizomaStateManager : MonoBehaviour
{
    public Rhizoma.Rhizoma_States RhizomaCurrentState;


    void ChangeStateTo(Rhizoma.Rhizoma_States _state) {
        //For now just enable one script RStateDreaming
        switch (_state)
        {
            case Rhizoma.Rhizoma_States.Dreaming:
                break;
            case Rhizoma.Rhizoma_States.Sleeping:
                break;
            case Rhizoma.Rhizoma_States.Thinking:
                break;
            case Rhizoma.Rhizoma_States.Attending:
                break;
            case Rhizoma.Rhizoma_States.Pausing:
                break;
            default:
                break;
        }
        RStateDreaming rizomaDreaming = GetComponent<RStateDreaming>();
        rizomaDreaming.enabled = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        ChangeStateTo(Rhizoma.Rhizoma_States.Dreaming);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
