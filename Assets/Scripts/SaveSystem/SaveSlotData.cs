using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveSlotData
{
    public string[] saveName;
    public int[] saveTime;
    public int[] saveDay;
    public int[] savePlayerLevel;
    public SaveSlotData(string[] name, int[] time, int[] day, int[] level)
    {
        saveName = new string[100];
        saveTime = new int[100];
        saveDay = new int[100];
        savePlayerLevel = new int[100];
        for (int i = 0; i < saveName.Length; i++)
        {
            if (i < name.Length)
            {
                saveName[i] = name[i];
                saveTime[i] = time[i];
                saveDay[i] = day[i];
                savePlayerLevel[i] = level[i];
            }
        }
    }
}
