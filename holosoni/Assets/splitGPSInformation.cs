using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class splitGPSInformation : MonoBehaviour {

    //public GameObject map;

    public GameObject mapController;
   // public GameObject northController;

    public float currentLat;
    public float currentLon;
    public float currentAlt;
    public float currentAcc;
    public float currentTS;
    private string gpsInfo;
    string patternLat = @"Lat: ([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*Lon: ([-+]?[0-9]*\.[0-9]+|[0-9]+)";
    public float currentNorthDislocation;

    public float oldLat = 0f;
    public float oldLon = 0f;


    // Use this for initialization
    void Start () {

        mapController.gameObject.GetComponent<recoverFromTrackingLoss>().turnMapOn(false);
        //gpsInfo = "Lat: -30.06827\nkjdkfjdkjfdfkj Lon: -51.12038 Alt: 99 HorAcc: 18 TS: 54165465464\nijfdjfdjidfjidf Truenorth: 33.21453 magNorth: 43.54545\nkjdkfjdkjfdfkj destination: PUCRS";


    }

    // Update is called once per frame
    void Update () {


        gpsInfo =  gameObject.GetComponent<Text>().text;

        Regex regexLat = new Regex(patternLat, RegexOptions.IgnoreCase);                                         
                                                                                                                         
        Match matchLat = regexLat.Match(gpsInfo);

        if (matchLat.Success) //&& !map.gameObject.GetComponent<googleAPI>().lockUpdates)      //if updates arent locked -  impedes the overupdating of these values causing the connection to crash
        {
           
            currentLat = float.Parse(matchLat.Groups[1].Value);
            currentLon = float.Parse(matchLat.Groups[3].Value);
            //currentNorthDislocation = float.Parse(matchLat.Groups[5].Value);

            //northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);

            if ((System.Math.Abs(currentLat - oldLat) > 0.00004) || (System.Math.Abs(currentLon - oldLon) > 0.00004))
            {
                oldLat = currentLat;
                oldLon = currentLon;

                //mapController.gameObject.GetComponent<recoverFromTrackingLoss>().turnMapOn(true);

                mapController.gameObject.GetComponent<recoverFromTrackingLoss>().forceMapReset();

                
               // mapController.gameObject.GetComponent<recoverFromTrackingLoss>().reRotateNorth(currentNorthDislocation);
               // northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);
            }
        }

      // Debug.Log("Lat detectada = " + currentLat.ToString() + "Lon detectada: " + currentLon.ToString() + "deslocamento do norte detectado: " + currentNorthDislocation.ToString() + "º");

    }
}
