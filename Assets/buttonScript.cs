using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonScript : MonoBehaviour {
    public InputField input;
    public InputField input2;
    public Button myButton;
    public Text alert;
    string val;
    string val2;

	// Use this for initialization
	void Start () {
        Debug.Log("Test");
        input.text = "";
        input2.text = "";
        myButton = GetComponent<Button>();
        try
        {
            myButton.onClick.AddListener(() => getScreenView());
        }
        catch (System.Exception e)
        {
            //DO nothing
        }
    }


	
	// Update is called once per frame
	void Update () {
	}

    void getScreenView()
    {
        

        if (input.text.Equals("") || input2.text.Equals(""))
        {
            alert.text = "Please input both locations"; 
        }
        else
        {
            val = input.text;
            val2 = input2.text;
            Debug.Log(val);
            Debug.Log(val2);
            alert.text = "Calculating route....";
        }
    }
}
