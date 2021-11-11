using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensHandTracking;

public class tapToFreeze : MonoBehaviour {

    bool frozen = false;

    // public GameObject TrackingObject;

    // public GameObject TrackingObject2;


    Transform originalParent;

    Transform originalTransform;

    

    // Use this for initialization
    void Start () {

        originalParent = transform.parent;
        originalTransform = transform;

    }
	
	// Update is called once per frame
	void Update () {

        /*if (handTracker.holdingHand1)
        {
            if (frozen)
            {
                float size = Vector3.Distance(TrackingObject.transform.localPosition, TrackingObject2.transform.localPosition);
                transform.localScale = new Vector3(size, size, size);
            }
        }*/ 


    }

    void OnSelect()
    {
        frozen = !frozen;

       

        if (frozen)
        {
            transform.parent = Camera.main.transform;
            

        }
        else
        { 
            transform.parent = originalParent;
            transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);     //preserves the pitch and roll rotations (hololens takes care of them), only changing the yaw

        }



    }
}
