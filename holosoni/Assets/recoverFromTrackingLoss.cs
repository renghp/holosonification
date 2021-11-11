using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class recoverFromTrackingLoss : MonoBehaviour {

    // Use this for initialization

    public int navMode = 0;

    public GameObject mainCamera;
    public GameObject map;
    public GameObject fakeNorth;
    /* public GameObject northController;

     public GameObject gpsInfo;
     string patternLat = @"(\n|.)*Truenorth: ([-+]?[0-9]*\.[0-9]+|[0-9]+)";
     public float currentNorthDislocation;*/

    public Text feedbackText;
    int counter = 0;
    string currentMsg;

    public GameObject[] waypoints;

    void Start()
    {

       
        UnityEngine.XR.WSA.WorldManager.OnPositionalLocatorStateChanged += WorldManager_OnPositionalLocatorStateChanged;
    }

    // Update is called once per frame
    void Update () {

        feedbackText.text = currentMsg;

        map.gameObject.GetComponent<googleAPI>().navMode = navMode;


    }




    private void WorldManager_OnPositionalLocatorStateChanged(PositionalLocatorState oldState, PositionalLocatorState newState)
    {
        if (newState != PositionalLocatorState.Active) //v1     // if (newState == PositionalLocatorState.Active && newState != oldState) //v2
        {

            forceMapReset();
        }
        //else
        //{
        //    //forceMapReset();
       // }
    }


    public void forceMapReset()
    {

       // Debug.Log("111111111111111111RESETOU POR CAUSA DA LAT LON111111111111111111111");

        /* Regex regexLat = new Regex(patternLat, RegexOptions.IgnoreCase);

         Match matchLat = regexLat.Match(gpsInfo.GetComponent<Text>().text);

         if (matchLat.Success) //&& !map.gameObject.GetComponent<googleAPI>().lockUpdates)      //if updates arent locked -  impedes the overupdating of these values causing the connection to crash
         {

             currentNorthDislocation = float.Parse(matchLat.Groups[2].Value);

             northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);

         }

         northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);*/

        //Debug.Log("forcing a reset");

        turnMapOn(true);

       // Debug.Log("forcing a reset2");

        GameObject mapClone;
        Transform spawnPos;
       // Debug.Log("forcing a reset3");

        spawnPos = map.transform;

        waypoints = GameObject.FindGameObjectsWithTag("newWayPoint");

        foreach (GameObject waypoint in waypoints)
        {
            waypoint.tag = "wayPoint";
        }

       // Debug.Log("retagged all current waypoints before creating the new ones");

        mapClone = Instantiate(map, spawnPos.position, spawnPos.rotation, map.transform.parent);

        mapClone.gameObject.GetComponent<googleAPI>().navMode = map.gameObject.GetComponent<googleAPI>().navMode;   //repasses the navigation mode to the new cloned map. need to do this to the destination once it isn't constant anymore 

        Destroy(map);

        map = mapClone;

       /* waypoints = GameObject.FindGameObjectsWithTag("wayPoint");

        Debug.Log("forcing a reset4");

        foreach (GameObject waypoint in waypoints)
        {
            Destroy(waypoint);
        }

        Debug.Log("destroyed all old waypoints");*/


        counter++;
        //currentMsg = "Mapa foi destruido e reinstanciado #" + counter;

        fakeNorth.transform.position = mainCamera.transform.position;       //força todos os  a voltar pra origem sempre que reseta a rota, a fim de não recriar uma rota longe de sua própria origem

        //mainCamera.transform.localPosition = new Vector3(0f, 0f, 0f);    

        currentMsg = "Instance #" + counter;

        //Debug.Log("2222222222222222222RESETOU POR CAUSA DA LAT LON222222222222222222");

    }

    public void turnMapOn(bool status)
    {
        map.gameObject.SetActive(status);
    }
    

}