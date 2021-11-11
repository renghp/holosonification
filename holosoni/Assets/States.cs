using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class States 
{
    public int state;
    public int pot;
	public int sink;
	public int pasta;
	public int cheese;
	public int plate;
	public int table;	
	public int fan;
	public int stove;
	public int window;
	public int axela;
	public int airconditioner;

    public static States CreateFromJson(string json)
    {
        return JsonUtility.FromJson<States>(json);
    }
}
