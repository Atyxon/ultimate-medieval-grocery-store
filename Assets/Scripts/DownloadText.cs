using UnityEngine;
using System.Collections;
using TMPro;

public class DownloadText : MonoBehaviour
{
    public TextMeshProUGUI logText;
    public string text;
    private string url = "https://pastebin.com/raw/xH6JT9tE"; // <-- enter your url here

    void Start()
    {
        StartCoroutine(GetTextFromWWW());
    }

    IEnumerator GetTextFromWWW()
    {
        WWW www = new WWW(url);

        yield return www;

        if (www.error != null)
        {
            logText.text = "<color=orange>Connection error:</color> " + www.error ;
        }
        else
        {
            text = www.text;
            logText.text = text;
        }
    }
}