using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class weatherAndTimer : MonoBehaviour
{
    public TimeManager timeManager;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI initTimeText;
    public TextMeshProUGUI initDayText;
    public Image weatherImage;
    public ambientSoundManager ambientSndMng;
    int weather;
    [Space]
    public Sprite[] weatherSprites;
    public string[] weatherStrings;
    public Vector2[] weatherThresholds;
    private void Update()
    {
        initTimeText.text = timeManager.timeString;
        initDayText.text = "- Day " + timeManager.day + " -";

        timeText.text = timeManager.timeString + "\n" + weatherStrings[weather];
        for (int i = 0; i < weatherThresholds.Length; i++)
        {
            if (weatherThresholds[i].x < timeManager.time && weatherThresholds[i].y >= timeManager.time)
            {
                weatherImage.sprite = weatherSprites[i];
                weather = i;
                if (weatherImage.sprite == weatherSprites[0] || weatherImage.sprite == weatherSprites[4])
                {
                    if (ambientSndMng.audioSrc.clip != ambientSndMng.ambientSounds[1])
                    {
                        ambientSndMng.audioSrc.clip = ambientSndMng.ambientSounds[1];
                        ambientSndMng.audioSrc.Play();
                    }
                }
                else
                {
                    if (ambientSndMng.audioSrc.clip != ambientSndMng.ambientSounds[0])
                    {
                        ambientSndMng.audioSrc.clip = ambientSndMng.ambientSounds[0];
                        ambientSndMng.audioSrc.Play();
                    }
                }
            }
        }
    }
}
