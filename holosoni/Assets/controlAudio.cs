using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlAudio : MonoBehaviour
{

    public List<AudioClip> Clips = new List<AudioClip>();
    int clipCounter = 0;

    // Use this for initialization
    void Start()
    {
        Debug.Log("eyy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void audioOff()
    {
        gameObject.GetComponent<AudioSource>().Pause();
        //Debug.Log("said audio off");
    }

    public void audioOn()
    {
        gameObject.GetComponent<AudioSource>().Play();
       
    }

    public void playNextMessage()
    {
        

        clipCounter++;

        if (clipCounter < Clips.Count)
        {
            gameObject.GetComponent<AudioSource>().clip = Clips[clipCounter];
            gameObject.GetComponent<AudioSource>().Play();

        }
        
    }

    public void playspecificMessage(int messageIndex)
    {
        

        if (messageIndex < Clips.Count)
        {
            gameObject.GetComponent<AudioSource>().clip = Clips[messageIndex];
            gameObject.GetComponent<AudioSource>().Play();

        }
        //Debug.Log("said audio ON");
    }


}

