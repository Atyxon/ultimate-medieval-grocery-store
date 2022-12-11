using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class levelManager : MonoBehaviour
{
    public level[] levels;
    public Color levelTextColor;
    public LevelUpAnimation levelAnim;
    private void Start()
    {
        initLevels();
    }
    public void addExp(string levelName, int expAmount)
    {
        levels[0].exp += expAmount;
        if (levels[0].exp >= levels[0].expMax)
        {
            nextLevel("Player");
        }

        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].name == levelName)
            {
                levels[i].exp += expAmount;
                if (levels[i].exp >= levels[i].expMax)
                {
                    nextLevel(levelName); 
                }
            }
        }
        initLevels();
    }
    public void nextLevel(string levelName)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (levels[i].name == levelName)
            {
                levels[i].levelInt++;
                int expLeft = levels[i].exp - levels[i].expMax;
                levels[i].exp = 0;

                float nextExp = (float)levels[i].expMax * 1.137f + (4 * (Mathf.Pow((float)levels[i].levelInt, 3))) / 5;
                levels[i].expMax = (int)nextExp;

                addExp(levelName, expLeft);

                levelAnim.SetText(levelName + " level increased to <color=orange>" + levels[i].levelInt + "<color=white>!");
                levelAnim.gameObject.SetActive(true);
            }
        }
        initLevels();
    }
    public void initLevels()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].levelText.text = levels[i].name + " <color=orange>Lvl. " + levels[i].levelInt;
            if (levels[i].name == "Player")
            {
                levels[i].expSlider.sizeDelta = new Vector2(((float)levels[i].exp / (float)levels[i].expMax) * 400, 36);
            }
            else
            {
                levels[i].expSlider.sizeDelta = new Vector2(((float)levels[i].exp / (float)levels[i].expMax) * 360, 36);
            }
        }
    }
}
