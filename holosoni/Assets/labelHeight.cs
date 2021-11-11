using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class labelHeight : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        double dist2D = Math.Sqrt((double)((transform.position.x - Camera.main.transform.position.x) * (transform.position.x - Camera.main.transform.position.x) + (transform.position.z - Camera.main.transform.position.z) * (transform.position.z - Camera.main.transform.position.z)));

        transform.localPosition = new Vector3(transform.localPosition.x, 0.37f +(float)dist2D/7f, transform.localPosition.z);


    }
}
