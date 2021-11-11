using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetometerController : MonoBehaviour {

    private bool wasSetOnce = false;

    public GameObject mainCamera;

    // Use this for initialization
    void Start () {



        //Input.location.Start();


    }
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(transform.position.x, transform.position.y - (transform.position.y - mainCamera.transform.position.y + 1f) / 20f, transform.position.z);

    }

    public void reRotateNorth(float degrees)
    {
        if (!wasSetOnce)        //sets it once so that the user doesn't need to hold the phone right every time it updates. hopefully the HL deals with the corrections on its own.
        {
            wasSetOnce = true;
            transform.rotation = Quaternion.Euler(-90, -degrees+16.95f, 0);        //-16°57' correction based on Porto Alegre http://www.magnetic-declination.com/
                                                                                    //correction could be calculated or gotten from a webservice based on the gps coordinates, but I haven't found a proper source yet
        }
    }
}
