using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class NewBehaviourScript : MonoBehaviour {
	string one = "16512 Blue Whetstone Lane, Odessa, FL 33556";
	string two = "5000 North Central Avenue, Tampa, FL 33603";
	string s = "";
	UnityWebRequest u;
	bool startHTTPrequest = false;
	HashSet<string> lats = new HashSet<string>();
	HashSet<string> lngs = new HashSet<string>();
	string startLoc = "start_location";
	Button myButton;
	Text myText;
	String JSON = "";
	private IEnumerator coroutine;


	// Use this for initialization
	void Start () {
		myButton = GetComponent<Button>();
		myButton.onClick.AddListener(() => startHTTPrequest = true);
		myText = GameObject.Find("LatText").GetComponent<Text>();
		for (int x = 0; x < one.Length; x++) {
			if (one.Substring(x, one.Length - x).Equals(" ")) {
				one = one.Substring (0, one.Length - x) + "+" + one.Substring (x);	
			}
			if (one.Substring(x, one.Length - x).Equals (",")) {
				one = one.Substring (0, one.Length - x) + one.Substring (x);
				x--;
			}

		}
		for (int x = 0; x < two.Length; x++) {
			if (two.Substring(x, two.Length - x).Equals(" ")){
				two = two.Substring(0,two.Length - x) + "+" + two.Substring(x);	
			}
			if (two.Substring(x, two.Length - x).Equals(",")) {
				two = two.Substring(0, one.Length - x) + two.Substring(x);
				x--;
			}
		}
		s = "https://maps.googleapis.com/maps/api/directions/json?origin=" + one + "&destination=" + two + "&key=AIzaSyD1cy06_XQRe1285aw1gbItKK98jDTA3lY";
		u = new UnityWebRequest(s);
		u.downloadHandler = new DownloadHandlerBuffer();
		coroutine = request (u);
		StartCoroutine (coroutine);
	}

	// Update is called once per frame
	void Update () {
			
		Debug.Log (u.downloadHandler.isDone);
		JSON = ((DownloadHandlerBuffer)(u.downloadHandler)).text;
		Debug.Log (((DownloadHandlerBuffer)(u.downloadHandler)).text);
		//Debug.Log ("fghfh:+ " + JSON);
			while (JSON.Contains(startLoc)) {
				int lastindex = JSON.IndexOf("}", JSON.IndexOf(startLoc));
				string inProg = JSON.Substring(JSON.IndexOf(startLoc), JSON.Length - lastindex + 1);
			//Debug.Log ("inProg " + inProg);
				int latIndex = inProg.IndexOf("\"lat\" :");
				string lat = inProg.Substring(latIndex, inProg.IndexOf(",") - latIndex);
			//Debug.Log ("lat " + lat);
				int brace = inProg.IndexOf("}");
				string lng = inProg.Substring(inProg.IndexOf("\"lng\" :"), inProg.IndexOf("}") - inProg.IndexOf("\"lng\" :"));
			//Debug.Log ("lng " + lng);
				JSON = JSON.Substring(brace);
				lats.Add(lat);
				lngs.Add(lng);
			}

		try {
			myText.text = (string)(lats[0]);
			Debug.Log (lats.Count);
			Debug.Log (lngs.Count);
		}
		catch (Exception e) {
			
		}
	}
	IEnumerator request(UnityWebRequest unit) {
		//yield return unit.Send();
		yield return u.Send();
		//p = unit.responseCode;
	}
}