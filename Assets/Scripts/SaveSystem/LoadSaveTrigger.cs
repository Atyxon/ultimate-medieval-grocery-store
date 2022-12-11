using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadSaveTrigger : MonoBehaviour
{
    public string saveName;
    public bool loadSaveBool;
    [Space]
    public string[] arrayOfStrings;
    public int[] saveTimeArray;
    public int[] saveDayArray;
    public int[] savePlayerLevelArray;
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        LoadData();
    }
    public void setLoadMode()
    {
        loadSaveBool = true;
    }
    public void LoadData()
    {
        SaveSlotData data = SavesWatcher.LoadSave();
        arrayOfStrings = data.saveName;
        saveTimeArray = data.saveTime;
        saveDayArray = data.saveDay;
        savePlayerLevelArray = data.savePlayerLevel;
    }
    public void SaveData(int time, int day, int playerLevel)
    {
        for (int i = 0; i < arrayOfStrings.Length; i++)
        {
            if (arrayOfStrings[i] == saveName)
            {
                saveTimeArray[i] = time;
                saveDayArray[i] = day;
                savePlayerLevelArray[i] = playerLevel;
            }
        }
        SavesWatcher.SaveSlot(arrayOfStrings, saveTimeArray, saveDayArray, savePlayerLevelArray);
    }
}
