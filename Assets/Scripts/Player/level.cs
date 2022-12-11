using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class level
{
    public string name;
    [Space]
    public int levelInt;
    public int exp;
    public int expMax;
    [Space]
    public RectTransform expSlider;
    public TextMeshProUGUI levelText;
}
