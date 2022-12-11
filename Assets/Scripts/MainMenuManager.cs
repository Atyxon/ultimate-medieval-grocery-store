using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuManager : MonoBehaviour
{
    public Transform logoBackground;
    public float spinSpeed;
    public LoadSaveTrigger lsTrigger;
    public GameObject loadSaveWindow;
    public GameObject newGameWindow;
    public GameObject settingsWindow;
    public GameObject creditsWindow;
    public Image Background;
    public Color bgColor;
    public GameObject loadingScreen;
    public RectTransform loadingBar;
    public TextMeshProUGUI progressText;
    public TMP_InputField saveNameInputField;
    public GameObject nothingToShowHereText;
    public GameObject deleteSave;
    public MainMenuSaveSlot saveToDelete;
    public TextMeshProUGUI deleteText;
    [Space]
    public bool loadSaveWindowOpen;
    public bool newGameWindowOpen;
    public bool settingsWindowOpen;
    public bool creditsWindowOpen;
    public bool backgroundEnabled;
    [Space]
    public Transform targetPos;
    public Transform hiddenPos;
    public float transformSpeed;
    [Space]
    public bool canClick;
    [Space]
    public Transform savesContent;
    public GameObject savePrefab;
    public string[] arrayOfStrings;
    public int[] saveTimeArray;
    public int[] saveDayArray;
    public int[] savePlayerLevelArray;
    private void Start()
    {
        loadSaveWindow.SetActive(true);
        newGameWindow.SetActive(true);
        settingsWindow.SetActive(true);
        creditsWindow.SetActive(true);
        Background.gameObject.SetActive(true);
        loadingScreen.SetActive(false);
        LoadSaveSlots();
    }
    private void FixedUpdate()
    {
        logoBackground.Rotate(0,0,spinSpeed*Time.fixedDeltaTime);

        if (loadSaveWindowOpen)
            loadSaveWindow.transform.localPosition = Vector3.Lerp(loadSaveWindow.transform.localPosition, targetPos.localPosition, transformSpeed * Time.fixedDeltaTime);
        else
            loadSaveWindow.transform.localPosition = Vector3.Lerp(loadSaveWindow.transform.localPosition, hiddenPos.localPosition, transformSpeed * Time.fixedDeltaTime);

        if (newGameWindowOpen)
            newGameWindow.transform.localPosition = Vector3.Lerp(newGameWindow.transform.localPosition, targetPos.localPosition, transformSpeed * Time.fixedDeltaTime);
        else
            newGameWindow.transform.localPosition = Vector3.Lerp(newGameWindow.transform.localPosition, hiddenPos.localPosition, transformSpeed * Time.fixedDeltaTime);

        if (settingsWindowOpen)
            settingsWindow.transform.localPosition = Vector3.Lerp(settingsWindow.transform.localPosition, targetPos.localPosition, transformSpeed * Time.fixedDeltaTime);
        else
            settingsWindow.transform.localPosition = Vector3.Lerp(settingsWindow.transform.localPosition, hiddenPos.localPosition, transformSpeed * Time.fixedDeltaTime);

        if (creditsWindowOpen)
            creditsWindow.transform.localPosition = Vector3.Lerp(creditsWindow.transform.localPosition, targetPos.localPosition, transformSpeed * Time.fixedDeltaTime);
        else
            creditsWindow.transform.localPosition = Vector3.Lerp(creditsWindow.transform.localPosition, hiddenPos.localPosition, transformSpeed * Time.fixedDeltaTime);

        if (backgroundEnabled)
        {
            bgColor.a = Mathf.Lerp(bgColor.a, .9f, 15 * Time.fixedDeltaTime);
            Background.color = bgColor;
            Background.raycastTarget = true;
        }
        else
        {
            bgColor.a = Mathf.Lerp(bgColor.a, 0, 15 * Time.fixedDeltaTime);
            Background.color = bgColor;
            Background.raycastTarget = false;
        }
    }
    public void CloseWindow()
    {
        loadSaveWindowOpen = false;
        newGameWindowOpen = false;
        settingsWindowOpen = false;
        creditsWindowOpen = false;
        backgroundEnabled = false;
        canClick = true;
    }
    public void OpenLoadSaveWindow()
    {
        if (canClick)
        {
            loadSaveWindowOpen = true;
            backgroundEnabled = true;
            canClick = false;
        }
    }
    public void OpenNewGameWindow()
    {
        if (canClick)
        {
            newGameWindowOpen = true;
            backgroundEnabled = true;
            canClick = false;
        }
    }
    public void OpenSettingsWindow()
    {
        if (canClick)
        {
            settingsWindowOpen = true;
            backgroundEnabled = true;
            canClick = false;
        }
    }
    public void OpenCreditsWindow()
    {
        if (canClick)
        {
            creditsWindowOpen = true;
            backgroundEnabled = true;
            canClick = false;
        }
    }
    public void StartNewGame()
    {
        for (int i = 0; i < arrayOfStrings.Length; i++)
        {
            if (arrayOfStrings[i] == null)
            {
                arrayOfStrings[i] = saveNameInputField.text;
                saveDayArray[i] = 1;
                savePlayerLevelArray[i] = 1;
                lsTrigger.saveName = saveNameInputField.text;
                break;
            }
        }
        SavesWatcher.SaveSlot(arrayOfStrings,saveTimeArray,saveDayArray,savePlayerLevelArray);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene());
    }
    public void LoadGame(string saveName)
    {
        lsTrigger.saveName = saveName;
        loadingScreen.SetActive(true);
        lsTrigger.setLoadMode();
        StartCoroutine(LoadScene());
    }
    public IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Single);
        while (!operation.isDone)
        {
            progressText.text = (int)(operation.progress * 100) + "%";
            loadingBar.sizeDelta = new Vector2(730 * operation.progress, 26);
            yield return null;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void LoadSaveSlots()
    {
        SaveSlotData data = SavesWatcher.LoadSave();
        arrayOfStrings = data.saveName;
        saveTimeArray = data.saveTime;
        saveDayArray = data.saveDay;
        savePlayerLevelArray = data.savePlayerLevel;
        for (int i = 0; i < arrayOfStrings.Length; i++)
        {
            if (arrayOfStrings[i] != null)
            {
                GameObject SaveSlot = Instantiate(savePrefab, savesContent.transform.position, savesContent.transform.rotation);
                SaveSlot.transform.parent = savesContent;
                SaveSlot.transform.localScale = new Vector3(1, 1, 1);
                MainMenuSaveSlot slot = SaveSlot.GetComponent<MainMenuSaveSlot>();
                slot.saveName = arrayOfStrings[i];
                nothingToShowHereText.SetActive(false);
            }
        }
    }
    public void checkStrings()
    {
        nothingToShowHereText.SetActive(true);
        for (int i = 0; i < arrayOfStrings.Length; i++)
        {
            if (arrayOfStrings[i] != null)
            {
                nothingToShowHereText.SetActive(false);
            }
        }
    }
    public void deleteSlotNo()
    {
        deleteSave.SetActive(false);  
    }
    public void deleteSlotYes()
    {
        deleteSave.SetActive(false);
        saveToDelete.DeleteSaveConfirmed();
    }
}
