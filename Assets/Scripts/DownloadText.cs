using UnityEngine;
using System.Collections;
using TMPro;

public class DownloadText : MonoBehaviour
{
    public bool devChangelog;
    [Space]
    public TextMeshProUGUI logText;
    public string text;
    private string urlDev = "https://raw.githubusercontent.com/Atyxon/UMGS-changelog/main/changelog-dev";
    private string urlProd = "https://raw.githubusercontent.com/Atyxon/UMGS-changelog/main/changelog-prod";
    void Start()
    {
        StartCoroutine(GetTextFromWWW());
    }

    IEnumerator GetTextFromWWW()
    {
        string url = "";
        if (devChangelog)
            url = urlDev;
        else
            url = urlProd;

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