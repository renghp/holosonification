using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clickToType : MonoBehaviour
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
        inputText.GetComponent<InputField>().text += gameObject.name;
        Debug.Log("digitou: " + gameObject.name);
        Debug.Log("string atual: " + inputText.GetComponent<InputField>().text);
    }
}
