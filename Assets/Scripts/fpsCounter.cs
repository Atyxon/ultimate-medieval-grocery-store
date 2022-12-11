using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fpsCounter : MonoBehaviour
{
    public int avgFrameRate;
    public TextMeshProUGUI displayText;
    int fpsGroup;
    int fpsGroupCount;
    float timer;
    public float fpsCountRefrestTime;
    public void Update()
    {
        timer += Time.deltaTime;
        float current = 0;
        current = (int)(1f / Time.unscaledDeltaTime);
        fpsGroup += (int)current;
        fpsGroupCount++;
        if (timer >= fpsCountRefrestTime)
        {
            avgFrameRate = fpsGroup/fpsGroupCount;
            displayText.text = avgFrameRate.ToString() + " FPS";
            fpsGroup = 0;
            fpsGroupCount = 0;
            timer = 0;
        }
    }
}
