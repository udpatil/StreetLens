using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour {
    public Button myButton;

	// Use this for initialization
	void Start () {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(() => changeColor());
	}


	
	// Update is called once per frame
	void Update () {
	}

    void changeColor()
    {
        var colors = myButton.colors;
        colors.normalColor = Color.red;
        myButton.colors = colors;
    }
}
