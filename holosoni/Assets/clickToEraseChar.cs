using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickToEraseChar : MonoBehaviour
{

    public GameObject inputText;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnSelect()
    {
        string buffer = inputText.GetComponent<InputField>().text;
        buffer = buffer.Remove(buffer.Length - 1);
        inputText.GetComponent<InputField>().text = buffer;
    }
}
