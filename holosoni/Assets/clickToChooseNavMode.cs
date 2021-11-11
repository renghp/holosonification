using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickToChooseNavMode : MonoBehaviour {

    int navMode = 0;
    public GameObject mapController;

    public GameObject carIcon;
    public GameObject bikeIcon;
    public GameObject walkIcon;
    private Color faded;
    private Color full;

    // Use this for initialization
    void Start () {
        faded = new Color(1f, 1f, 1f, 0.4f);
        full = new Color(1f, 1f, 1f, 1f);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnSelect()
    {
        if (gameObject.name == "drivingButton")
        {
            navMode = 0;
            carIcon.gameObject.GetComponent<Image>().color = faded;
            walkIcon.gameObject.GetComponent<Image>().color = full;
            bikeIcon.gameObject.GetComponent<Image>().color = full;
        }
        else if (gameObject.name == "walkingButton")
        {
            navMode = 1;
            carIcon.gameObject.GetComponent<Image>().color = full;
            walkIcon.gameObject.GetComponent<Image>().color = faded;
            bikeIcon.gameObject.GetComponent<Image>().color = full;
        }
        else if (gameObject.name == "cyclingButton")
        {
            navMode = 2;
            carIcon.gameObject.GetComponent<Image>().color = full;
            walkIcon.gameObject.GetComponent<Image>().color = full;
            bikeIcon.gameObject.GetComponent<Image>().color = faded;
        }

        mapController.gameObject.GetComponent<recoverFromTrackingLoss>().navMode = navMode;

        mapController.gameObject.GetComponent<recoverFromTrackingLoss>().forceMapReset();
    }
}
