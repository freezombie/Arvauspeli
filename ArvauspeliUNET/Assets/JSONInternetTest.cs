using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SimpleJSON;

public class JSONInternetTest : MonoBehaviour
{
    string articleName;
    string url;
    JSONNode jsonFromUrl;
    bool downloading = false;
    bool downloaded = false;
    Text textComponent;

	// Use this for initialization
	void Start ()
    {
        textComponent = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(downloading)
        {
            GetComponent<Text>().text = "Downloading info... please wait";
        }
        else if(!downloading && downloaded)
        {
            return;
        }
        else
        {
            GetComponent<Text>().text = "";
        }
	}

    public void UpdateString(string text)
    {
        /*if(text.Contains(" "))
        {
            for(int i=0;i<text.Length; i++)
            {
                if(text[i]==' ')
                {
                    text.Insert(i,"%20");
                    Debug.Log("text was corrected to" + text);
                    UpdateString(text);
                }
            }
        }*/ // THIS CURRENTLY SEEMS TO JUST INSERT ANOTHER FUCKING SPACE THERE
        articleName = text;
        Debug.Log(text + " inputted");
        articleName = WWW.EscapeURL(articleName);
        url = "https://en.wikipedia.org/w/api.php?format=json&action=query&redirects=1&prop=extracts&exintro=&explaintext=&titles=" + articleName;
        downloading = true;
        StartCoroutine(DownloadJSON(url));        
    }

    IEnumerator DownloadJSON(string url)
    {
        WWW www = new WWW(url);
        Debug.Log("bytes downloaded " + www.bytesDownloaded + " of " + www.bytes.Length + " progress: " + www.progress*100 + "%");
        yield return www;
        downloading = false;
        jsonFromUrl = JSON.Parse(www.text);
        Debug.Log(jsonFromUrl); // tässä on oikeat jutut wtf.
        //Debug.Log(www);
        /*foreach(byte bt in www.bytes)
        {
            Debug.Log(bt.ToString());
        }*/
        if(jsonFromUrl == null)
        {
            Debug.Log("Didn't find anything with url " + url);
            GetComponent<Text>().text = "Couldn't download a wikipedia article with that name";
        }
        else
        {
            downloaded = true;
            if(jsonFromUrl["query"]["pages"][0]["extract"] == null)
            {
                textComponent.text = "Wikipedia returned an empty article. Please enter another";
            }
            else
            {
                textComponent.text = jsonFromUrl["query"]["pages"][0]["extract"];
            }
            Debug.Log(jsonFromUrl["query"]["pages"][0]["extract"]);
            //GetComponent<Text>().text = "Downloaded info, now do something with it";

        }
        
    }
}
