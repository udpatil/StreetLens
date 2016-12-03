using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class typeScript : MonoBehaviour, IPointerClickHandler {
    UnityEngine.TouchScreenKeyboard keyboard;
    public static string keyboardText = "";
    public Button myButton;

    // Use this for initialization
    void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}

    public void OnPointerClick(PointerEventData eventData)
    {
        keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false);
        if (TouchScreenKeyboard.visible == false && keyboard != null)
        {
            if (keyboard.done == true)
            {
                keyboardText = keyboard.text;
                keyboard = null;
            }
        }
    }
}
