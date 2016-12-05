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
	string url = "";
    //bool startHTTPrequest = false;
	List<Coordinate> lats = new List<Coordinate>();
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
            if (input.text.Substring(x, input.text.Length - x).Equals(","))
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
        url = "https://maps.googleapis.com/maps/api/directions/json?origin=" + val + "&destination=" + val2 + "&key=AIzaSyD1cy06_XQRe1285aw1gbItKK98jDTA3lY";
        u = new UnityWebRequest(url);
        u.downloadHandler = new DownloadHandlerBuffer();
        coroutine = request(u);
        StartCoroutine(coroutine);
        startEverything = false;
            dummy = true;
     }
        while(dummy && !u.downloadHandler.isDone)
        {
            Debug.Log("NOT DONE");
        }

        if (dummy && u.downloadHandler.isDone)
        {
            
            //Debug.Log(u.downloadHandler.isDone);
            JSON = ((DownloadHandlerBuffer)(u.downloadHandler)).text;
            //Debug.Log(((DownloadHandlerBuffer)(u.downloadHandler)).text);
            //Debug.Log("fghfh:+ " + JSON);
            while (JSON.Contains(startLoc))
            {
                int lastindex = JSON.IndexOf("}", JSON.IndexOf(startLoc));
                string inProg = JSON.Substring(JSON.IndexOf(startLoc), JSON.Length - lastindex + 1);
                //Debug.Log("inprog " + inProg);
                //Debug.Log("inProg " + inProg);
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
                //Debug.Log("lat " + lat);
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
                //Add heading (use next lat and long  - previous and calculate heading with tangent
                //TODO
                //Debug.Log("lng " + lng);
                JSON = JSON.Substring(brace);
                if (lats.Count > 0)
                {
                    Debug.Log("Equal to last? : " + lats.ToArray()[lats.Count - 1].Equals(new Coordinate(lat, lng, 0)));
                }
                if (lats.Count == 0 || !lats.ToArray()[lats.Count - 1].Equals(new Coordinate(lat, lng, 0))) { 
                    lats.Add(new Coordinate(lat, lng, lats.Count > 0 ? calcDir(lats.ToArray()[lats.Count - 1], new Coordinate(lat, lng, 0)) : 0));//TODO figure out how to get this working to make the headings
                    if (lats.Count > 1)
                    {
                        lats.ToArray()[lats.Count - 2].setDir(lats.ToArray()[lats.Count - 1].getDir());
                    }
                }
                //Maybe check difference between latitude and longitude and skip over close points
            }
        }
        if (dummy && u.downloadHandler.isDone && imagesRendered == false)
        {

            foreach (Coordinate c in lats)
            {
                    //implment direction calculation and check to see if same as previous coordinate
                Debug.Log(c.ToString());
                //Debug.Log(lats.Count);//Logs lats size  
                StartCoroutine(retrieveImage(c.getLat(), c.getLng(), c.getDir()));
            }
            imagesRendered = true;
        }





    }
    double calcDir(Coordinate thisPoint, Coordinate nextPoint)
    {
        Debug.Log("Calculating Tangent: " + "\nlat1: " + thisPoint.getLat() + " lng1: " + thisPoint.getLng() + "\nlat2: " + nextPoint.getLat() + " lng2: " + nextPoint.getLng());
        double dLat = nextPoint.getLat() - thisPoint.getLat();
        double dLng = nextPoint.getLng() - thisPoint.getLng();
        double heading = 180 * (Math.Atan(dLat / dLng)) / Math.PI;
        return heading;
    }
    IEnumerator request(UnityWebRequest unit) {
		//yield return unit.Send();
		yield return u.Send();
		//p = unit.responseCode;
	}
    public IEnumerator retrieveImage(double Lat, double Long, double Direction)
    {
            WWW www = null;
            yield return new WaitForSeconds(20);
            string s = "https://maps.googleapis.com/maps/api/streetview?size=600x400&location=" + Lat + "," + Long + "&heading=" + Direction + "&key=" + API_KEY;
            www = new WWW(s);
            yield return www;
            //Debug.Log(www.url);
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
            alert.text = "Calculating route....";
            val = input.text;
            val2 = input2.text;
            Debug.Log(val);
            Debug.Log(val2);
            
            startEverything = true;
        }
    }
    private class Coordinate {
        double lat;
        double lng;
        double dir;
        int hashCode = 1;

        public Coordinate(String lat, String lng, double dirIn)
        {
            this.lat = Double.Parse(lat);
            this.lng = Double.Parse(lng);
            dir = dirIn;
            
        }
        public double getLat()
        {
            return lat;
        }
        public double getLng()
        {
            return lng;
        }
        public double getDir()
        {
            return dir;
        }
        public void setDir(double dirIn)
        {
            dir = dirIn;
        }

        override
        public Boolean Equals(System.Object obj)
        {
            if (obj == null) return false;
            if (!(obj is Coordinate)) return false;
            Coordinate c = (Coordinate) obj;
            if (c == this) return true;
            if (this.lat == c.lat && this.lng == c.lng) return true;
            return false;
        }

        override
        public int GetHashCode()
        {
            hashCode = (37 * hashCode) + (int)(BitConverter.DoubleToInt64Bits(lat) ^ (BitConverter.DoubleToInt64Bits(lat) >> 32));
            hashCode = (37 * hashCode) + (int)(BitConverter.DoubleToInt64Bits(lng) ^ (BitConverter.DoubleToInt64Bits(lng) >> 32));
            hashCode = (37 * hashCode) + (int)(BitConverter.DoubleToInt64Bits(dir) ^ (BitConverter.DoubleToInt64Bits(dir) >> 32));
            return hashCode;
        }

        override
        public String ToString()
        {
            return "lat: " + lat + " long: " + lng + " direction: " + dir;
        }
    }

}