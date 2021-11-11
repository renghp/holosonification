using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObj 
{
    public int _out;
    public int specific;
    public string timeStamp;
    public int objNumber;

    public static GeneralObj CreateFromJson(string json)
    {
        return JsonUtility.FromJson<GeneralObj>(json);
    }

}
