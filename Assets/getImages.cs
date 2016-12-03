using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getImages : MonoBehaviour {
    string API_KEY = "AIzaSyAPJ3rOjGocUB1WW_guGVf3FwHpryKY_w8";
    public Image myImage;
    string s;
    IEnumerator coRoutine;
    // Use this for initialization
    void Start () {
        coRoutine = retrieveImage(46.414382, 10.013988, 150);
        StartCoroutine(coRoutine);
        Debug.Log(s);
    }
	
    public IEnumerator retrieveImage(double Lat, double Long, double Direction)
    {
        //https://maps.googleapis.com/maps/api/streetview?size=600x300&location=46.414382,10.013988&heading=151.78&pitch=-0.76&key=YOUR_API_KEY
        //WWW www = new WWW("https://maps.googleapis.com/maps/api/streetview?size=600x400&location=" + Lat + "," + Long + "&heading=" + Direction + "&key=" + API_KEY);
        s = "https://maps.googleapis.com/maps/api/streetview?size=600x400&location=" + Lat + "," + Long + "&heading=" + Direction + "&key=" + API_KEY;
        
        WWW www = new WWW(s);
        yield return www;
        Debug.Log(www.url);
        myImage.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
        
    }

    // Update is called once per frame
    void Update () {

        
    }
}
