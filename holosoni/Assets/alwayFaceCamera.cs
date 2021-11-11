using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensHandTracking;

public class alwayFaceCamera : MonoBehaviour {

    public GameObject mainCamera;
    //public GameObject handTrackerScript;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {



        transform.LookAt(mainCamera.transform.localPosition);

        // if (!handTrackerScript.gameObject.GetComponent<handTracker>().mapIsHandGuided)
        //   transform.localPosition = new Vector3(mainCamera.transform.localPosition.x + 1f, mainCamera.transform.localPosition.y - 0.5f, mainCamera.transform.localPosition.z + 1f);
    }
}
