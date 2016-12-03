using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour {
    public Light myLight;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey("space"))
        {
            myLight.enabled = true;
        } else
        {
            myLight.enabled = false;
        }
	}
}
