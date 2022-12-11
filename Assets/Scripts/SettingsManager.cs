using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mixer;
    public TextMeshProUGUI fovText;
    public TextMeshProUGUI renderScaleText;
    public TextMeshProUGUI sensitivityText;
    public TextMeshProUGUI musicText;
    public TextMeshProUGUI sfxText;
    public Slider fovSlider;
    public Slider renderScaleSlider;
    public Slider sensitivitySlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Toggle fullscreenToggle;
    public Toggle vsyncToggle;
    public Toggle showFpsToggle;
    [Space]
    public GameObject videoSettings;
    public GameObject controlsSettings;
    public GameObject volumeSettings;
    [Space]
    public TMP_Dropdown resDropdown;
    public TMP_Dropdown qualityDropdown;
    public UniversalRenderPipelineAsset[] qualityAssets;
    public Resolution[] res;
    public GameObject fpsCount;
    int currentRes;
    private void Start()
    {
        if (fpsCount != null)
            DontDestroyOnLoad(fpsCount);
        else
        {
            fpsCount = FindInActiveObjectByName("FPSCounter");
        }
        openVideoSettings();
        res = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < res.Length; i++)
        {
            string option = res[i].width + " x " + res[i].height + " " + res[i].refreshRate + "Hz";
            options.Add(option);
            if (res[i].width == Screen.currentResolution.width && res[i].height == Screen.currentResolution.height)
            {
                currentRes = i;
            }
        }
        resDropdown.AddOptions(options);
        InitializeSettings();
    }
    public void InitializeSettings()
    {
        if (PlayerPrefs.HasKey("resolution"))
        {
            Screen.SetResolution(res[PlayerPrefs.GetInt("resolution")].width, res[PlayerPrefs.GetInt("resolution")].height, Screen.fullScreen);
            resDropdown.value = PlayerPrefs.GetInt("resolution");
            resDropdown.RefreshShownValue();
        }
        else
        {
            resDropdown.value = currentRes;
            resDropdown.RefreshShownValue();
        }

        if (PlayerPrefs.HasKey("fullscreen"))
        {
            if (PlayerPrefs.GetInt("fullscreen") == 1)
            {
                Screen.fullScreen = true;
                fullscreenToggle.isOn = true;
            }
            else
            {
                Screen.fullScreen = false;
                fullscreenToggle.isOn = false;
            }
        }
        else
        {
            Screen.fullScreen = true;
            fullscreenToggle.isOn = true;
        }
        if (PlayerPrefs.HasKey("vsync"))
        {
            if (PlayerPrefs.GetInt("vsync") == 1)
            {
                QualitySettings.vSyncCount = 1;
                vsyncToggle.isOn = true;
            }
            else
            {
                QualitySettings.vSyncCount = 0;
                vsyncToggle.isOn = false;
            }
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            vsyncToggle.isOn = false;
        }
        if (PlayerPrefs.HasKey("show_fps") && fpsCount != null)
        {
            if (PlayerPrefs.GetInt("show_fps") == 1)
            {
                fpsCount.SetActive(true);
                showFpsToggle.isOn = true;
            }
            else
            {
                fpsCount.SetActive(false);
                showFpsToggle.isOn = false;
            }
        }
        else
        {
            if (fpsCount != null)
            {
                fpsCount.SetActive(false);
                showFpsToggle.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("quality"));
            qualityDropdown.value = PlayerPrefs.GetInt("quality");
        }
        else
        {
            QualitySettings.SetQualityLevel(2);
            qualityDropdown.value = 2;
        }
        if (PlayerPrefs.HasKey("fov"))
        {
            SetFovSliderValue(PlayerPrefs.GetInt("fov"));
            fovSlider.value = PlayerPrefs.GetInt("fov");
        }
        else
        {
            SetFovSliderValue(80);
            fovSlider.value = 80;
        }
        if (PlayerPrefs.HasKey("render_scale"))
        {
            SetRenderScaleSliderValue(PlayerPrefs.GetInt("render_scale"));
            renderScaleSlider.value = PlayerPrefs.GetInt("render_scale");
        }
        else
        {
            SetRenderScaleSliderValue(100);
            renderScaleSlider.value = 100;
        }
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            SetSensitivitySliderValue(PlayerPrefs.GetInt("sensitivity"));
            sensitivitySlider.value = PlayerPrefs.GetInt("sensitivity");
        }
        else
        {
            SetSensitivitySliderValue(50);
            sensitivitySlider.value = 50;
        }
        if (PlayerPrefs.HasKey("music"))
        {
            SetMusicSliderValue(PlayerPrefs.GetInt("music"));
            musicSlider.value = PlayerPrefs.GetInt("music");
        }
        else
        {
            SetMusicSliderValue(0);
            musicSlider.value = 0;
        }
        if (PlayerPrefs.HasKey("sfx"))
        {
            SetSfxSliderValue(PlayerPrefs.GetInt("sfx"));
            sfxSlider.value = PlayerPrefs.GetInt("sfx");
        }
        else
        {
            SetSfxSliderValue(0);
            sfxSlider.value = 0;
        }
    }
    public void SetFovSliderValue(float value)
    {
        fovText.text = "FOV: <color=orange>" + fovSlider.value;
        PlayerPrefs.SetInt("fov", (int)value);
    }
    public void SetRenderScaleSliderValue(float value)
    {
        renderScaleText.text = "Render Scale: <color=orange>" + renderScaleSlider.value + "%";

        for (int i = 0; i < qualityAssets.Length; i++)
        {
            qualityAssets[i].renderScale = (value / 100);
        }
        PlayerPrefs.SetInt("render_scale", (int)value);
    }
    public void SetSensitivitySliderValue(float value)
    {
        sensitivityText.text = "Sensitivity: <color=orange>" + sensitivitySlider.value;
        PlayerPrefs.SetInt("sensitivity", (int)value);
    }
    public void SetMusicSliderValue(float value)
    {
        musicText.text = "Music: <color=orange>" + (musicSlider.value + 80);
        PlayerPrefs.SetInt("music", (int)value);
    }
    public void SetSfxSliderValue(float value)
    {
        sfxText.text = "SFX: <color=orange>" + (sfxSlider.value + 80);
        PlayerPrefs.SetInt("sfx", (int)value);
        mixer.SetFloat("sfx_volume", value); 
    }
    public void SetQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
        PlayerPrefs.SetInt("quality", level);
    }
    public void SetFullscreenMode(bool mode)
    {
        Screen.fullScreen = mode;
        if(mode)
            PlayerPrefs.SetInt("fullscreen", 1);
        else
            PlayerPrefs.SetInt("fullscreen", 0);
    }
    public void SetVsyncMode(bool mode)
    {
        if (mode)
        {
            PlayerPrefs.SetInt("vsync", 1);

            int currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.SetQualityLevel(0);
            QualitySettings.vSyncCount = 1;
            QualitySettings.SetQualityLevel(1);
            QualitySettings.vSyncCount = 1;
            QualitySettings.SetQualityLevel(2);
            QualitySettings.vSyncCount = 1;
            QualitySettings.SetQualityLevel(currentLevel);
        }
        else
        {
            PlayerPrefs.SetInt("vsync", 0);

            int currentLevel = QualitySettings.GetQualityLevel();
            QualitySettings.SetQualityLevel(0);
            QualitySettings.vSyncCount = 0;
            QualitySettings.SetQualityLevel(1);
            QualitySettings.vSyncCount = 0;
            QualitySettings.SetQualityLevel(2);
            QualitySettings.vSyncCount = 0;
            QualitySettings.SetQualityLevel(currentLevel);
        }
    }
    public void SetShowFps(bool mode)
    {
        if (mode)
        {
            fpsCount.SetActive(true);
            PlayerPrefs.SetInt("show_fps", 1);
        }
        else
        {
            fpsCount.SetActive(false);
            PlayerPrefs.SetInt("show_fps", 0);
        }
    }
    public void SetResolution(int index)
    {
        PlayerPrefs.SetInt("resolution", index);
        Screen.SetResolution(res[index].width, res[index].height, Screen.fullScreen);
    }
    public void openVideoSettings()
    {
        videoSettings.SetActive(true);
        controlsSettings.SetActive(false);
        volumeSettings.SetActive(false);
    }
    public void openControlsSettings()
    {
        videoSettings.SetActive(false);
        controlsSettings.SetActive(true);
        volumeSettings.SetActive(false);
    }
    public void openVolumeSettings()
    {
        videoSettings.SetActive(false);
        controlsSettings.SetActive(false);
        volumeSettings.SetActive(true);
    }
    GameObject FindInActiveObjectByName(string name)
    {
        Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < objs.Length; i++)
        {
            if (objs[i].hideFlags == HideFlags.None)
            {
                if (objs[i].name == name)
                {
                    return objs[i].gameObject;
                }
            }
        }
        return null;
    }
}
