using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public float time;
    public float maxTime = 1440;
    public float timeMultiplier = 2;
    public int day = 1;
    public string timeString;
    public LightingManager lghManager;
    void Update()
    {
        time += Time.deltaTime * (1/timeMultiplier);
        int hours = (int)(time / 60);
        int minutes = (int)time - (hours * 60);
        timeString = hours.ToString("00") + ":" + minutes.ToString("00");
        lghManager.UpdateLighting(time / maxTime);
        if (time >= maxTime)
        {
            time = 0;
            day++;
        }
    }
}
