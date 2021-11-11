using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HoloLensHandTracking;

public class meshOnOff : MonoBehaviour {

    public Transform placeHolder;

    private bool meshStatus;

    public GameObject roomMesh;

	// Use this for initialization
	void Start () {

        meshStatus = false;
        OnSelect();

    }
	
	// Update is called once per frame
	void Update () {

        transform.position = placeHolder.position;
        transform.eulerAngles = placeHolder.eulerAngles;

    }

    void OnSelect()
    {

        Debug.Log("entrou");
        meshStatus = !meshStatus;



        if (meshStatus)
        {
            roomMesh.SetActive(false);


        }
        else
        {

            roomMesh.SetActive(true);


        }



    }
}


