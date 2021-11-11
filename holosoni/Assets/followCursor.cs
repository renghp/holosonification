using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followCursor : MonoBehaviour {


    public Transform placeHolder;

    // Use this for initialization
    void Start () {
		
	}
	

    void Update()
    {

        transform.position = placeHolder.position;
        transform.eulerAngles = placeHolder.eulerAngles;

    }
}
