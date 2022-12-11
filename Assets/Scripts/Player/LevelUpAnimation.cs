using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpAnimation : MonoBehaviour
{
    public bool isAnimationPlaying;
    public List<string> textsList = new List<string>();
    public TextMeshProUGUI text;
    public AudioSource audioSrc;
    public AudioClip clip;
    public void SetText(string levelText)
    {
        if (!isAnimationPlaying)
        {
            text.text = levelText;
            isAnimationPlaying = true;
            audioSrc.clip = clip;
            audioSrc.Play();
        }
        else 
        {
            textsList.Add(levelText);
        }
    }
    public void EndOfAnimation()
    {
        if (textsList.Count == 0)
        {
            this.gameObject.SetActive(false);
            isAnimationPlaying = false;
        }
        else
        {
            text.text = textsList[0];
            textsList.Remove(textsList[0]);
            audioSrc.clip = clip;
            audioSrc.Play();
        }
    }
}
