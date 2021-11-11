using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class RequestData : MonoBehaviour
{
    //[Header ("url server")]
    //[Tooltip("Url name of the server")]
    public string url = "http://192.168.0.29:3000/obj/";
    [Header ("Object")]
    [Tooltip("Name of the Object")]
    //public string ObjectName;
    int objIndex = 0;            //0: axela; 1: potBoiling; 2: alarmClock; 3: smokeAlarm; 4: window; 5: rest

    [Header ("GameObject")]
    [Tooltip("Name of the GameObject")]
    //public GameObject GameObjectName;

    public List<GameObject> GameObjectName = new List<GameObject>();

    public string lastTimeStampRead = "0";

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("starting web object");

        StartCoroutine(GetRequest(url));
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(GetRequest(url));
    }

    IEnumerator GetRequest(string url)
    {
        //using (UnityWebRequest webRequest = UnityWebRequest.Get(url + ObjectName))
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();

            Debug.Log("starting web request");

            if (webRequest.isNetworkError)
            {
                
                Debug.Log("error");
                yield break;
            }
            else
            {
                if ((webRequest.isDone)  && (GeneralObj.CreateFromJson(webRequest.downloadHandler.text).timeStamp != lastTimeStampRead))
                {

                    //Debug.Log("TS: " + GeneralObj.CreateFromJson(webRequest.downloadHandler.text).timeStamp);

                    lastTimeStampRead = GeneralObj.CreateFromJson(webRequest.downloadHandler.text).timeStamp;

                    objIndex = GeneralObj.CreateFromJson(webRequest.downloadHandler.text).objNumber;

                    if (objIndex < 5)
                    { 

                        if ((GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 0)) {
                            Debug.Log("play audio");
                            GameObjectName[objIndex].GetComponent<controlAudio>().audioOn();
                            yield break;
                        }
                        else if(GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 1)
                        {
                            Debug.Log("stop audio");
                            GameObjectName[objIndex].GetComponent<controlAudio>().audioOff();
                           // Debug.Log("stop");
                            yield break;
                        }
                        else if (GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 2)
                        {
                            Debug.Log("play next message");
                            GameObjectName[objIndex].GetComponent<controlAudio>().playNextMessage();
                            yield break;

                        }
                        else if (GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 3)
                        {
                            Debug.Log("play message number "+ GeneralObj.CreateFromJson(webRequest.downloadHandler.text).specific);
                            GameObjectName[objIndex].GetComponent<controlAudio>().playspecificMessage((int)GeneralObj.CreateFromJson(webRequest.downloadHandler.text).specific);
                            yield break; 

                        }
                    }
                    else 
                    { 
                    
                        for (int i = 5; i<= 12; i++)
                        {


                            if ((GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 0))
                            {
                                Debug.Log("play audio");
                                GameObjectName[i].GetComponent<controlAudio>().audioOn();
                                //yield break;
                            }
                            else if (GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 1)
                            {
                                Debug.Log("stop audio");
                                GameObjectName[i].GetComponent<controlAudio>().audioOff();
                                // Debug.Log("stop");
                                //yield break;
                            }
                          /*  else if (GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 2)
                            {
                                Debug.Log("play next message");
                                GameObjectName[i].GetComponent<controlAudio>().playNextMessage();
                                yield break;

                            }*/
                           /* else if (GeneralObj.CreateFromJson(webRequest.downloadHandler.text)._out == 3)
                            {
                                Debug.Log("play message number " + GeneralObj.CreateFromJson(webRequest.downloadHandler.text).specific);
                                GameObjectName[i].GetComponent<controlAudio>().playspecificMessage((int)GeneralObj.CreateFromJson(webRequest.downloadHandler.text).specific);
                                yield break;

                            }*/

                        }

                    }
                    // Here you use GameObjectName
                    // You can use its methods, translation or add flag for anything do you need
                    // Debug.Log("ok");
                    //GameObjectName.SetActive(false);                   
                    yield break;
                }
            }
        }
    }
}
