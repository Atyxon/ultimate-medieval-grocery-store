using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainMenuSaveSlot : MonoBehaviour
{
    public RawImage ScreenshotImage;
    public MainMenuManager mainMenuMng;
    public string saveName;
    public TextMeshProUGUI saveNameText;
    public TextMeshProUGUI saveTimeText;
    public TextMeshProUGUI saveDayText;
    public TextMeshProUGUI saveLevelText;
    private void Start()
    {
        mainMenuMng = FindObjectOfType<MainMenuManager>();
        saveNameText.text = saveName;
        SaveSlotData data = SavesWatcher.LoadSave();
        for (int i = 0; i < data.saveName.Length; i++)
        {
            if (saveName == data.saveName[i])
            {
                saveDayText.text = "Day " + data.saveDay[i];
                saveLevelText.text = "Player Level <color=orange>" + data.savePlayerLevel[i];
                int time = data.saveTime[i];
                int hours = (time / 60);
                int minutes = time - (hours * 60);
                saveTimeText.text = hours.ToString("00") + ":" + minutes.ToString("00");
            }
        }
    }
    public void LoadGame()
    {
        mainMenuMng.LoadGame(saveName);
    }
    public void DeleteSave()
    {
        mainMenuMng.saveToDelete = this;
        mainMenuMng.deleteSave.SetActive(true);
        mainMenuMng.deleteText.text = "Are You sure You want to delete <color=orange>" + saveName +"</color> save slot?";
    }
    public void DeleteSaveConfirmed()
    {
        SaveSystem.DeleteSave(saveName);
        for (int i = 0; i < mainMenuMng.arrayOfStrings.Length; i++)
        {
            if (saveName == mainMenuMng.arrayOfStrings[i])
            {
                mainMenuMng.arrayOfStrings[i] = null;
                break;
            }
        }
        SavesWatcher.SaveSlot(mainMenuMng.arrayOfStrings, mainMenuMng.saveTimeArray, mainMenuMng.saveDayArray, mainMenuMng.savePlayerLevelArray);
        mainMenuMng.checkStrings();
        Destroy(this.gameObject);
    }
}
