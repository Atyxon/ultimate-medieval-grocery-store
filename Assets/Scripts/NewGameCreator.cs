using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGameCreator : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;
    public float buttonPositionOn;
    public float buttonPositionOff;
    public float buttonMoveTime;
    public GameObject contentWindowLevels;
    public GameObject contentWindowAppearance;
    public GameObject contentWindowClothing;
    public RectTransform buttonLevels;
    public RectTransform buttonAppearance;
    public RectTransform buttonClothing;

    // Levels Window
    [Header("Levels Window")]
    public int pointsLeft;
    public int[] levelInts = new int[4];
    public TextMeshProUGUI pointsLeftText;
    public TextMeshProUGUI plantingLevelText;
    public TextMeshProUGUI serviceLevelText;
    public TextMeshProUGUI managementLevelText;
    public TextMeshProUGUI buildingLevelText;
    public TextMeshProUGUI[] levelTexts = new TextMeshProUGUI[4];

    // Appearance Window
    //[Header("Appearance Window")]

    // Clothings Window
    //[Header("Clothings Window")]

    private void Start()
    {
        pointsLeftText.text = "Points left: <color=orange>" + pointsLeft;
        setContentWindow(0);
        levelTexts[0] = plantingLevelText;
        levelTexts[1] = serviceLevelText;
        levelTexts[2] = managementLevelText;
        levelTexts[3] = buildingLevelText;
    }
    public void increaseLevel(int index)
    {
        if(pointsLeft > 0)
        {
            pointsLeft--;
            levelInts[index]++;
            levelTexts[index].text = levelInts[index] + "";
            pointsLeftText.text = "Points left: <color=orange>" + pointsLeft;
            levelTexts[index].color = activeColor;
        }
    }
    public void decreaseLevel(int index)
    {
        if(levelInts[index] > 0)
        {
            pointsLeft++;
            levelInts[index]--;
            levelTexts  [index].text = levelInts[index] + "";
            pointsLeftText.text = "Points left: <color=orange>" + pointsLeft;
            if (levelInts[index] == 0)
                levelTexts[index].color = inactiveColor;
        }
    }

    public void setContentWindow(int contentIndex)
    {
        contentWindowLevels.SetActive(false);
        contentWindowAppearance.SetActive(false);
        contentWindowClothing.SetActive(false);
        if(contentIndex == 0)
        {
            contentWindowLevels.SetActive(true);
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonLevels, true));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonAppearance, false));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonClothing, false));
        }
        else if (contentIndex == 1)
        {
            contentWindowAppearance.SetActive(true);
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonLevels, false));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonAppearance, true));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonClothing, false));
        }
        else if (contentIndex == 2)
        {
            contentWindowClothing.SetActive(true);
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonLevels, false));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonAppearance, false));
            StartCoroutine (SmoothLerp (buttonMoveTime, buttonClothing, true));
        }
    }

    private IEnumerator SmoothLerp (float time, RectTransform button, bool setOn)
    {
        Vector3 startingPos  = button.anchoredPosition;
        float finalX = 0;
        if(setOn)
            finalX = buttonPositionOn;
        else
            finalX = buttonPositionOff;
        Vector3 finalPos = new Vector3(finalX, button.anchoredPosition.y, 0);
        float elapsedTime = 0;
        while (elapsedTime < time)
        {
            button.anchoredPosition = Vector3.Lerp(startingPos, finalPos, (elapsedTime / time));
            elapsedTime += Time.fixedDeltaTime;
            yield return null;
        }
    }
}
