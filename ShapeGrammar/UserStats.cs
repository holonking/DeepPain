using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserStats : MonoBehaviour {

    public static Material lineMat;
    public static Material LineMat
    {
        get
        {
            if (lineMat == null)
                lineMat = Resources.Load("LineMaterial") as Material;
            return lineMat;
        }
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
