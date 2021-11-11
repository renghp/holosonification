using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class googleAPI : MonoBehaviour
{


    private string pathDistAndTime;

    private bool vaiPassarWP = false;
    private bool passouWP = false;

    public GameObject northController;

    public GameObject gpsInfo;

    private RawImage img;

    string urlMap;

    string urlPath;

    public string localOrig;

    public string localDest;

    public GameObject threeDWaypointObjBase;
    public GameObject snappedPathObjBase;

    public int navMode = 0;


    LocationInfo li;

    public int zoom;
    public int mapWidth;
    public int mapHeight;

    public enum mapType { roadmap, satellite, hybrid, terrain }
    public mapType mapSelected;
    public int scale;

    private float[] turnPointsLat = new float[100];
    private float[] turnPointsLong = new float[100];

    private string[] instructions = new string[100];

    private string[] maneuver = new string[100];

    private string wholePolylineEncoded;

    struct polylineCoord
    {
        public double lat;
        public double lon;
    };

    private List<polylineCoord> polylineCoordinates = new List<polylineCoord>();

   // private List<double> polylineLat = new List<double>();
   // private List<double> polylineLong = new List<double>();

    // private string[] polylinePointsLong = new string[500];


    public float currentLat;
    public float currentLon;


    //public Text errorText;
    public Text errorText2;

    public Text fullTrajectoryInfo;

    public Text newCoordinatesText;

    public bool lockUpdates = false;

   
    IEnumerator Map()
    {

        


        //localDest




        string wholePathJSON;

        string navigationMode;

        switch (navMode)
        {
            case 0:
                navigationMode = "&mode=driving";
                 break;
            case 1:
                navigationMode = "&mode=bicycling";
                break;
            case 2:
                navigationMode = "&mode=walking";
                break;
            default:
                navigationMode = "&mode=driving";
                break;
        }

        if (currentLat != 0f && currentLon!= 0f)
        {
            Debug.Log("diferente de zero");
            urlPath = "https://maps.googleapis.com/maps/api/directions/json?origin=" + currentLat + "," + currentLon + "&destination=" + localDest + navigationMode + "&key=AIzaSyDLCPwQAFruyIwVq8iK4gO6M4JbxkIYv7c"; //add &language=pt-BR to the url to change the instructions
            Debug.Log(urlPath);

            turnPointsLat[0] = currentLat;
            turnPointsLong[0] = currentLon;
        }

        else
        {
            Debug.Log("igual a zero");
            urlPath = "https://maps.googleapis.com/maps/api/directions/json?origin=" + localOrig + "&destination=" + localDest + navigationMode + "&key=AIzaSyDLCPwQAFruyIwVq8iK4gO6M4JbxkIYv7c";

            Debug.Log(urlPath);

            turnPointsLat[0] = 0f;
            turnPointsLong[0] = 0f;

        }

        WWW www2 = new WWW(urlPath);
        yield return www2;


        if (www2.text == "")
        {
            errorText2.text = www2.error;
          //  errorText.text = "Got connection problems";
            yield break;
        }

      //  else
         //   errorText.text = "I got 99 problems but my connection ain't one";


        wholePathJSON = www2.text;

        //////////////////////
        

        string patternPathDistTime = @"legs""(\n|.)*?""distance""((\n|.)*?)""text""((\n|.)*?)""((\n|.)*?)""((\n|.)*?)""duration""((\n|.)*?)""text""((\n|.)*?)""((\n|.)*?)""";



        Regex regexPathDistTime = new Regex(patternPathDistTime, RegexOptions.IgnoreCase);

        Match matchrPathDistTime = regexPathDistTime.Match(wholePathJSON);

        if (matchrPathDistTime.Success)                                                     //getting the polyline overview (encrypted graph of snappedToRoad locations)
        {

            pathDistAndTime = "Planned route: " +  matchrPathDistTime.Groups[6].Value + ", " + matchrPathDistTime.Groups[14].Value;

            fullTrajectoryInfo.text = pathDistAndTime;
            Debug.Log(pathDistAndTime);

            // Debug.Log("sobrou algo = " + matchPolyline.Groups[4].Value);
            //turnPointsLong[1] = float.Parse(matchStartLoc.Groups[4].Value);
        }
        else
            Debug.Log("nao deu match");

       
        //////////////////////


        string patternInstruc = @"html_instructions""(\n|.)*?""((\n|.)*?)""";      //regex to get all end_locations
        Regex regexInstruc = new Regex(patternInstruc, RegexOptions.IgnoreCase);                                        //need to ignore the first (which is a duplicate of the final endlocation) and 
                                                                                                                         //get groups 2 and 4 as the latitude and longitude, respectively
        Match matchesInstruc = regexInstruc.Match(wholePathJSON);
        int matchInstCount = 1;

        instructions[0] = "Find the green arrows placed around you and follow their directions";


        while (matchesInstruc.Success)
        {
            instructions[matchInstCount] = matchesInstruc.Groups[2].Value;

           // string str = "~rajeev~ravi";
            //string strRemove = "rajeev";
            instructions[matchInstCount] = instructions[matchInstCount].Replace("\\u003cb\\u003e", "");
            instructions[matchInstCount] = instructions[matchInstCount].Replace("\\u003c/b\\u003e", "");
            instructions[matchInstCount] = instructions[matchInstCount].Replace("\\u003cdiv style=\\", ""); 

            Debug.Log(instructions[matchInstCount]);

            matchesInstruc = matchesInstruc.NextMatch();
            matchInstCount++;
        }

        Debug.Log("total de " + matchInstCount + " instrucoes");


        ////////////
        string patternManeuver = @"html_instructions""(\n|.)*?""((\n|.)*?)""((\n|.)*?)((""polyline"")|(""maneuver""))(\n|.)*?""((\n|.)*?)""";
        Regex regexManeuver = new Regex(patternManeuver, RegexOptions.IgnoreCase);                                         
                                                                                                                        
        Match matchesManeuver = regexManeuver.Match(wholePathJSON);
        int matchManeuverCount = 1;

        maneuver[0] = "";


        while (matchesManeuver.Success)
        {
            maneuver[matchManeuverCount] = matchesManeuver.Groups[10].Value;

           

            Debug.Log("uma manobra: " + maneuver[matchManeuverCount]);

            matchesManeuver = matchesManeuver.NextMatch();
            matchManeuverCount++;
        }

        Debug.Log("total de " + matchInstCount + " maneuvers");

        ////////////////

        string patternEnd = @"end_location(\n|.)*?([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*?([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*?}";      //regex to get all end_locations
        Regex regexEndLocations = new Regex(patternEnd, RegexOptions.IgnoreCase);                                        //need to ignore the first (which is a duplicate of the final endlocation) and 
                                                                                                                            //get groups 2 and 4 as the latitude and longitude, respectively
        Match matchesEndLoc = regexEndLocations.Match(wholePathJSON);
        int matchCount = 1;
        while (matchesEndLoc.Success)
        {
            /* Debug.Log(" Group 2 (lat): " + matchesEndLoc.Groups[2].Value);
            Debug.Log(" Group 4 (long): " + matchesEndLoc.Groups[4].Value);*/

            turnPointsLat[matchCount] = float.Parse(matchesEndLoc.Groups[2].Value);
            turnPointsLong[matchCount] = float.Parse(matchesEndLoc.Groups[4].Value);

            Debug.Log(turnPointsLat[matchCount] + "," + turnPointsLong[matchCount] + "|");

            matchesEndLoc = matchesEndLoc.NextMatch();
            matchCount++;
        }


        string patternStart = @"start_location(\n|.)*?([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*?([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*?}";      //regex to get all startLocations 
        Regex regexStartLocations = new Regex(patternStart, RegexOptions.IgnoreCase);

        Match matchStartLoc = regexStartLocations.Match(wholePathJSON);

        if (matchStartLoc.Success)                                                     //getting only the first startLocation with an if instead of a while
        {
            turnPointsLat[1] = float.Parse(matchStartLoc.Groups[2].Value);         //substitutes the first endlocation with the startLocation
            turnPointsLong[1] = float.Parse(matchStartLoc.Groups[4].Value);
        }

        if  (turnPointsLat[0] == 0f && turnPointsLong[0] == 0f)
        {
            turnPointsLat[0] = turnPointsLat[1];
            turnPointsLong[0] = turnPointsLong[1];

        }

        string patternPolylineCode = @"overview_polyline(\n|.)*?points""(\n|.)*?""((\n|.)*?)""";

        

        Regex regexPolyline = new Regex(patternPolylineCode, RegexOptions.IgnoreCase);

        Match matchPolyline = regexPolyline.Match(wholePathJSON);

        if (matchPolyline.Success)                                                     //getting the polyline overview (encrypted graph of snappedToRoad locations)
        {
            
            wholePolylineEncoded = matchPolyline.Groups[3].Value;
            Debug.Log(wholePolylineEncoded);

           // Debug.Log("sobrou algo = " + matchPolyline.Groups[4].Value);
            //turnPointsLong[1] = float.Parse(matchStartLoc.Groups[4].Value);
        }
        else
            Debug.Log("nao deu match");

        string decodedPolyline = decodePolylinePoints(wholePolylineEncoded);

        Debug.Log(decodedPolyline);

        Transform spawnPos2;
        GameObject obj2Bcloned2;
        // GameObject clone1;

        spawnPos2 = snappedPathObjBase.transform.GetChild(0).transform;
        obj2Bcloned2 = snappedPathObjBase.transform.GetChild(0).gameObject;

        foreach (polylineCoord polypoint in polylineCoordinates)
        {
            Debug.Log("uma lat = " + polypoint.lat.ToString() + " uma long = " + polypoint.lon.ToString());

           
            GameObject clone1;
            clone1 = Instantiate(obj2Bcloned2, spawnPos2.position, spawnPos2.rotation);

            clone1.transform.parent = snappedPathObjBase.transform.parent;

            //Debug.Log("long = " + turnPointsLong[i].ToString());

            clone1.tag = "newWayPoint";

            

            clone1.transform.localPosition = new Vector3(longitudeToMeters((float)polypoint.lon, (float)polypoint.lat, turnPointsLong[0], turnPointsLat[0]), 0f, latitudeToMeters((float)polypoint.lat, turnPointsLat[0]));
                // turnPointsLat[i].ToString() + "," + turnPointsLong[i].ToString();      //encodes all locations as markers
              


        }


        string markers = null;

        int i = 0;

        while (i < matchCount)
        {
            markers = markers + "&markers=color:blue%7Clabel:" + i.ToString() + "%7C" + turnPointsLat[i].ToString() + "," + turnPointsLong[i].ToString();      //encodes all locations as markers
            i++;
        }

        Debug.Log(markers);

        if (markers == null)
        {
            // Debug.Log("Não há caminho disponível");
            errorText2.text = "Não há caminho disponível\nentre os pontos " + localOrig + " e " + localDest;
        }

        threeDWaypointObjBase.transform.GetChild(0).gameObject.SetActive(true);


        Transform spawnPos;
        GameObject obj2Bcloned;
       // GameObject clone1;

        spawnPos = threeDWaypointObjBase.transform.GetChild(0).transform;
        obj2Bcloned = threeDWaypointObjBase.transform.GetChild(0).gameObject;

        //  clone1 = Instantiate(obj2Bcloned, spawnPos.position, spawnPos.rotation);
        //Instantiate(obj2Bcloned, spawnPos.position, spawnPos.rotation);
        // Instantiate(obj2Bcloned, spawnPos.position, spawnPos.rotation);

        //  clone1.transform.localPosition = new Vector3(0.1f, 0.1f, 2f);
        

        i = 0;

      //  float origLatMeters = latitudeToMetersOrig(turnPointsLat[0]);
       // float origLonMeters = longitudeToMetersOrig(turnPointsLong[0], turnPointsLat[0]);

        while (i < matchCount)
        {
            GameObject clone1;
            clone1 = Instantiate(obj2Bcloned, spawnPos.position, spawnPos.rotation);

            clone1.transform.parent = threeDWaypointObjBase.transform.parent;

            //Debug.Log("long = " + turnPointsLong[i].ToString());

            clone1.tag = "newWayPoint";

            clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text = instructions[i]; //+ " " maneuver[i];
            clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text = instructions[i];// + " " maneuver[i];

            clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;

            if (maneuver[i] != "" && maneuver[i] != "points")
            {
                switch (maneuver[i])
                {
                    case "turn-right":
                    
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(1).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;

                    case "turn-left":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(2).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "turn-slight-right":
                    case "ramp-right":
                    case "fork-right":
                    case "keep-right":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(3).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "turn-slight-left":
                    case "ramp-left":
                    case "fork-left":
                    case "keep-left":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(4).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(4).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "turn-sharp-right":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(5).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(5).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "turn-sharp-left":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(6).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(6).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(6).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "straight":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(7).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(7).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(7).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "merge":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(8).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(8).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(8).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "roundabout-right":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(9).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(9).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(9).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "roundabout-left":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(10).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(10).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(10).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "uturn-right":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(11).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(11).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(11).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;
                    case "uturn-left":
                        clone1.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                        clone1.transform.GetChild(0).GetChild(12).gameObject.SetActive(true);
                        clone1.transform.GetChild(0).GetChild(12).GetChild(1).GetChild(0).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text;
                        clone1.transform.GetChild(0).GetChild(12).GetChild(1).GetChild(1).GetComponent<Text>().text = clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text;
                        break;

                    default:
                        clone1.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
                    break;



                }
                //clone1.transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Text>().text += maneuver[i];
                //clone1.transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>().text += maneuver[i];
            }

            clone1.transform.localPosition = new Vector3(longitudeToMeters(turnPointsLong[i], turnPointsLat[i], turnPointsLong[0], turnPointsLat[0]), 0f, latitudeToMeters(turnPointsLat[i], turnPointsLat[0]));
           // turnPointsLat[i].ToString() + "," + turnPointsLong[i].ToString();      //encodes all locations as markers
            i++;
        }

        threeDWaypointObjBase.transform.GetChild(0).gameObject.SetActive(false);

        ////destroying old waypoints/////////////////////

        GameObject[] waypoints;

        waypoints = GameObject.FindGameObjectsWithTag("wayPoint");

        Debug.Log("forcing a reset4");

        foreach (GameObject waypoint in waypoints)
        {
            Destroy(waypoint);
        }

        Debug.Log("destroyed all old waypoints");

        //////////////////////////////////////////////////////



        string patternLat = @"Lat: ([-+]?[0-9]*\.[0-9]+|[0-9]+)(\n|.)*Lon: ([-+]?[0-9]*\.[0-9]+|[0-9]+)";
        string gpsInfo;

        while (true)
        {
            gpsInfo = newCoordinatesText.gameObject.GetComponent<Text>().text;

            Regex regexLat = new Regex(patternLat, RegexOptions.IgnoreCase);

            Match matchLat = regexLat.Match(gpsInfo);

            if (matchLat.Success)      //if updates arent locked -  impedes the overupdating of these values causing the connection to crash
            {

                currentLat = float.Parse(matchLat.Groups[1].Value);
                currentLon = float.Parse(matchLat.Groups[3].Value);
                
            }


           // lockUpdates = true;

            Debug.Log("Lat detectada = " + currentLat.ToString() + "Lon detectada: " + currentLon.ToString());

            if (currentLat != 0f && currentLon != 0f)
            { 
                urlMap = "https://maps.googleapis.com/maps/api/staticmap?center=" + currentLat + "," + currentLon +       //centers the map in current gps coordinate
                "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + scale
                + "&maptype=" + mapSelected + markers +                 //inserts the encoded markers into the display map
                "&key=AIzaSyDLCPwQAFruyIwVq8iK4gO6M4JbxkIYv7c";
            }
            else
            {
                urlMap = "https://maps.googleapis.com/maps/api/staticmap?center=" + turnPointsLat[0] + "," + turnPointsLong[0] +       //centers the map in the geographic middle point between origin and destination
                "&zoom=" + zoom + "&size=" + mapWidth + "x" + mapHeight + "&scale=" + scale
                + "&maptype=" + mapSelected + markers +                 //inserts the encoded markers into the display map
                "&key=AIzaSyDLCPwQAFruyIwVq8iK4gO6M4JbxkIYv7c";
            }

            WWW www = new WWW(urlMap);
            yield return www;
            img.texture = www.texture;
            img.SetNativeSize();

            //yield return new WaitForSeconds(2);

           // lockUpdates = false;

        }
        
    }
    // Use this for initialization
    void Start()
    {
        img = gameObject.GetComponent<RawImage>();

       
        string patternNorth = @"(\n|.)*Truenorth: ([-+]?[0-9]*\.[0-9]+|[0-9]+)";
        float currentNorthDislocation;

        Regex regexLat = new Regex(patternNorth, RegexOptions.IgnoreCase);

        Match matchLat = regexLat.Match(gpsInfo.GetComponent<Text>().text);

        if (matchLat.Success) //&& !map.gameObject.GetComponent<googleAPI>().lockUpdates)      //if updates arent locked -  impedes the overupdating of these values causing the connection to crash
        {

            currentNorthDislocation = float.Parse(matchLat.Groups[2].Value);

            northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);

        }


        string patternDest = @"(\n|.)*destination: (.*)";


        Regex regexDest = new Regex(patternDest, RegexOptions.IgnoreCase);

        Match matchDest = regexDest.Match(gpsInfo.GetComponent<Text>().text);

        if (matchDest.Success) //&& !map.gameObject.GetComponent<googleAPI>().lockUpdates)      //if updates arent locked -  impedes the overupdating of these values causing the connection to crash
        {
            if (matchDest.Groups[2].Value != "")
                localDest = matchDest.Groups[2].Value;

            Debug.Log("Destino é " + localDest);


        }


        // northController.gameObject.GetComponent<magnetometerController>().reRotateNorth(currentNorthDislocation);

        StartCoroutine(Map());
        //Map();
    }

    // Update is called once per frame
    void Update()
    {
        GetClosestWaypoint();
    }


    float latitudeToMeters(float lat, float origLat)
    {
        lat = lat - origLat;
        float meters = lat * 111111;
        return meters;
    }

    float longitudeToMeters(float lon, float lat, float origLon, float origLat)
    {
        float meters = 0;
        // 111111 * cos(lat) meters = 1 degree lon
        lon = lon - origLon;

        double Pi = 3.1415;

        meters = lon * 111111 * (float)(Math.Cos((Pi * lat / 180)));

        return meters;
    }

    void GetClosestWaypoint()      /* font: https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/ */
    {
        GameObject bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = transform.position;


        GameObject[] descriptionTexts = GameObject.FindGameObjectsWithTag("description");

        if (descriptionTexts != null)
        { 
            foreach (GameObject potentialTarget in descriptionTexts)
            {
                Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
                float dSqrToTarget = directionToTarget.sqrMagnitude;
                if (dSqrToTarget < closestDistanceSqr)
                {
                    closestDistanceSqr = dSqrToTarget;
                    bestTarget = potentialTarget;
                }
            }



            if (vaiPassarWP && closestDistanceSqr > 5f)
            {
                vaiPassarWP = false;

                if (bestTarget.GetComponent<Text>().text != instructions[0])
                    bestTarget.GetComponent<Text>().text = "Passed the checkpoint!";

            }
           
            else if (closestDistanceSqr < 5f)
                vaiPassarWP = true;


            if (bestTarget.GetComponent<Text>().text == "Passed the checkpoint!")
                errorText2.text = "";

            else if (bestTarget.GetComponent<Text>().text == instructions[0])
                errorText2.text = instructions[0];

            else
                errorText2.text = "In " + (int)closestDistanceSqr + " m: " + bestTarget.GetComponent<Text>().text;
            
                
        }

        

        //return bestTarget;
    }



    private string decodePolylinePoints(string encodedPoints)
    {
        if (encodedPoints == null || encodedPoints == "") return null;
        string poly = "";
        char[] polylinechars = encodedPoints.ToCharArray();
        int index = 0;

        int currentLat = 0;
        int currentLng = 0;
        int next5bits;
        int sum;
        int shifter;

        try
        {
            while (index < polylinechars.Length)
            {
                // calculate next latitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length)
                    break;

                currentLat += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);

                //calculate next longitude
                sum = 0;
                shifter = 0;
                do
                {
                    next5bits = (int)polylinechars[index++] - 63;
                    sum |= (next5bits & 31) << shifter;
                    shifter += 5;
                } while (next5bits >= 32 && index < polylinechars.Length);

                if (index >= polylinechars.Length && next5bits >= 32)
                    break;

                currentLng += (sum & 1) == 1 ? ~(sum >> 1) : (sum >> 1);
                double lat, lon;

                lat = (double)currentLat / 100000.0;
                lon = (double)currentLng / 100000.0;
                poly += " | " + lat.ToString() + "," + lon.ToString();

                polylineCoord p = new polylineCoord();
                p.lat = lat;
                p.lon = lon;
                polylineCoordinates.Add(p);
            }
        }
        catch (Exception ex)
        {
            // logo it
        }

        //Debug.Log(poly);

        return poly;
    }
}