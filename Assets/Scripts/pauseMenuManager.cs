using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class pauseMenuManager : MonoBehaviour
{
    public GameObject savePopup;
    public SaveLoadManager saveManager;
    public GameObject pauseMenu;
    public GameObject settingsWindow;
    public GameObject loadingScreen;
    public RectTransform loadingBar;
    public TextMeshProUGUI progressText;
    public CameraController camCont;
    public MovementController moveCont;
    public SettingsManager settingMng;
    [Space]
    public bool settingsWindowOpen;
    [Space]
    public Transform targetPos;
    public Transform hiddenPos;
    public float transformSpeed;
    [Space]
    public bool canClick;
    public bool isMenuOppened;
    private void Start()
    {
        settingsWindow.SetActive(true);
        loadingScreen.SetActive(false);
    }
    private void FixedUpdate()
    {
        if (settingsWindowOpen)
            settingsWindow.transform.localPosition = Vector3.Lerp(settingsWindow.transform.localPosition, targetPos.localPosition, transformSpeed * Time.fixedDeltaTime);
        else
            settingsWindow.transform.localPosition = Vector3.Lerp(settingsWindow.transform.localPosition, hiddenPos.localPosition, transformSpeed * Time.fixedDeltaTime);
    }
    public void OpenMenu()
    {
        pauseMenu.SetActive(true);
        isMenuOppened = true;
        camCont.enabled = false;
        moveCont.canMove = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
    public void Resume()
    {
        CloseWindow();
        pauseMenu.SetActive(false);
        isMenuOppened = false;
        camCont.enabled = true;
        moveCont.canMove = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camCont.initSettings();
    }
    public void CloseWindow()
    {
        settingsWindowOpen = false;
        canClick = true;
    }
    public void OpenSettingsWindow()
    {
        if (canClick)
        {
            settingsWindowOpen = true;
            canClick = false;
        }
    }
    public void Save()
    {
        saveManager.Save();
        savePopup.SetActive(false);
        savePopup.SetActive(true);
    }
    public void SaveAndExit()
    {
        Save();
        ExitToMenu(); 
    }
    public void ExitToMenu()
    {
        Destroy(settingMng.fpsCount);
        Destroy(saveManager.loadMng.gameObject);
        loadingScreen.SetActive(true);
        StartCoroutine(LoadScene());
    }
    public IEnumerator LoadScene()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
        while (!operation.isDone)
        {
            progressText.text = (int)(operation.progress * 100) + "%";
            loadingBar.sizeDelta = new Vector2(730 * operation.progress, 26);
            yield return null;
        }
    }
}
