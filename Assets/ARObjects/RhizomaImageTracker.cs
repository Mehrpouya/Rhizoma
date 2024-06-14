using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
public class RhizomaImageTracker : MonoBehaviour
{

    //Good tutorial on AR dev https://www.youtube.com/watch?v=XMLf24yTV4Y

    private ARTrackedImageManager RhizomaImageTrackerManager;
    public GameObject[] RhizomaImaginations;



    List<GameObject> RhizomaObjects = new List<GameObject>();


    void Awake()
    {
        RhizomaImageTrackerManager = GetComponent<ARTrackedImageManager>();
    }
    void OnEnable()
    {
        RhizomaImageTrackerManager.trackedImagesChanged += OnRhizomaNewRealisation;
    }

    void OnDisable()
    {
        RhizomaImageTrackerManager.trackedImagesChanged -= OnRhizomaNewRealisation;
    }
    private void OnRhizomaNewRealisation(ARTrackedImagesChangedEventArgs RhizomaSenses)
    {
        foreach (var sense in RhizomaSenses.added)
        {
            foreach (var concept in RhizomaImaginations)
            {
                if (sense.referenceImage.name == concept.name)
                {
                    var newObject = Instantiate(concept, sense.transform);
                    RhizomaObjects.Add(newObject);
                }
            }

        }
        foreach (var sense in RhizomaSenses.updated) 
        { 
            foreach(var gameObject in RhizomaObjects)
            {
                if (gameObject.name == sense.name) 
                {
                    gameObject.SetActive(sense.trackingState == TrackingState.Tracking);
                }
            }
        }


    }
}
