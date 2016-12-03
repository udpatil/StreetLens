using UnityEngine;
using UnityEngine.Networking;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using System;

public class NewBehaviourScript : MonoBehaviour {
    public InputField input;
    public InputField input2;
    public Image myImage;
    public Button myButton;
    string val;
    string val2;
    private bool startEverything = false;
    public Text alert;
    string one = "16512 Blue Whetstone Lane, Odessa, FL 33556";
	string two = "5000 North Central Avenue, Tampa, FL 33603";
	string url = "";
    //bool startHTTPrequest = false;
	HashSet<Coordinate> lats = new HashSet<Coordinate>();
	//HashSet<string> lngs = new HashSet<string>();
	string startLoc = "start_location";
	//Button myButton;
	Text myText;
    UnityWebRequest u;
	String JSON = "";
	private IEnumerator coroutine;
    //private IEnumeratorImages coRoutineForImages;
    private String API_KEY = "AIzaSyAPJ3rOjGocUB1WW_guGVf3FwHpryKY_w8";
    private bool imagesRendered = false;
    


    // Use this for initialization
    void Start () {
        u = new UnityWebRequest(url);
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
    void Update() {
        bool dummy = false;
    if (startEverything == true) {
            Debug.Log("input");
        for (int x = 0; x < input.text.Length; x++)
        {   
            if (input.text.Substring(x, input.text.Length - x).Equals(" "))
            {
                input.text = input.text.Substring(0, input.text.Length - x) + "+" + input.text.Substring(x);
            }
            if (one.Substring(x, one.Length - x).Equals(","))
            {
                input.text = input.text.Substring(0, input.text.Length - x) + input.text.Substring(x);
                x--;
            }

        }
        for (int x = 0; x < input2.text.Length; x++)
        {
            if (input2.text.Substring(x, input2.text.Length - x).Equals(" "))
            {
                input2.text = input2.text.Substring(0, input2.text.Length - x) + "+" + input2.text.Substring(x);
            }
            if (input2.text.Substring(x, input2.text.Length - x).Equals(","))
            {
                input2.text = input2.text.Substring(0, input2.text.Length - x) + input2.text.Substring(x);
                x--;
            }
        }
        url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + one + "&destination=" + two + "&key=AIzaSyD1cy06_XQRe1285aw1gbItKK98jDTA3lY";
        u = new UnityWebRequest(url);
        u.downloadHandler = new DownloadHandlerBuffer();
        coroutine = request(u);
        StartCoroutine(coroutine);
        startEverything = false;
            dummy = true;
     }
        while(dummy && !u.downloadHandler.isDone)
        {
            Debug.Log("NOT FKUIN DONE");
        }

        if (dummy && u.downloadHandler.isDone)
        {
            
            Debug.Log(u.downloadHandler.isDone);
            JSON = ((DownloadHandlerBuffer)(u.downloadHandler)).text;
            Debug.Log(((DownloadHandlerBuffer)(u.downloadHandler)).text);
            Debug.Log("fghfh:+ " + JSON);
            while (JSON.Contains(startLoc))
            {
                int lastindex = JSON.IndexOf("}", JSON.IndexOf(startLoc));
                string inProg = JSON.Substring(JSON.IndexOf(startLoc), JSON.Length - lastindex + 1);
                //Debug.Log("inprog " + inProg);
                Debug.Log("inProg " + inProg);
                int latIndex = inProg.IndexOf("\"lat\" :");
                string lat = inProg.Substring(latIndex, inProg.IndexOf(",") - latIndex);
                foreach (char c in lat)
                {
                    if (Char.IsDigit(c) || c.Equals('-'))
                    { 
                        lat = lat.Substring(lat.IndexOf(c));
                        lat.Trim();
                        break;
                    }
                }
                Debug.Log("lat " + lat);
                int brace = inProg.IndexOf("}");
                string lng = inProg.Substring(inProg.IndexOf("\"lng\" :"), inProg.IndexOf("}") - inProg.IndexOf("\"lng\" :"));
                foreach (char c in lng)
                {
                    if (Char.IsDigit(c) || c.Equals('-'))
                    {
                        lng = lng.Substring(lng.IndexOf(c));
                        lng.Trim();
                        break;
                    }
                }
                Debug.Log("lng " + lng);
                JSON = JSON.Substring(brace);
                lats.Add(new Coordinate(lat, lng));

            }
        }
        if (dummy && u.downloadHandler.isDone && imagesRendered == false)
        {
            foreach (Coordinate c in lats)
            {
                StartCoroutine(retrieveImage(c.getLat(), c.getLng(), c.getDir()));
            }
            imagesRendered = true;
        }





    }
    IEnumerator request(UnityWebRequest unit) {
		//yield return unit.Send();
		yield return u.Send();
		//p = unit.responseCode;
	}
    public IEnumerator retrieveImage(String Lat, String Long, double Direction)
    {
        WWW www = null;
        string s = "https://maps.googleapis.com/maps/api/streetview?size=600x400&location=" + Double.Parse(Lat) + "," + Double.Parse(Long) + "&heading=" + Direction + "&key=" + API_KEY;
        www = new WWW(s);
        yield return www;
        yield return new WaitForSeconds(2);
        Debug.Log(www.url);
        if (www.isDone)
        myImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        //yield return new WaitForSeconds(2);

    }
    void getScreenView()
    {
        Debug.Log("Button CLicked");
        

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
            startEverything = true;
        }
    }
    private class Coordinate {
        String lat;
        String lng;
        int dir;
        public Coordinate(String lat, String lng)
        {
            this.lat = lat;
            this.lng = lng;
            dir = 150;
        }
        public String getLat()
        {
            return lat;
        }
        public String getLng()
        {
            return lng;
        }
        public int getDir()
        {
            return dir;
        }
    }

}