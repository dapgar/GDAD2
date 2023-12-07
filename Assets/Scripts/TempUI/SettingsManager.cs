using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject pauseMenu;

    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    public Dropdown qualityDropdown;
    public Slider volumeSlider;
    float currentVolume;
    Resolution[] resolutions;

    // Start is called before the first frame update
    void Start()
    {
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;


        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " +
                     resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                  && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,
                  resolution.height, Screen.fullScreen);
    }

    public void SetQuality(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);

        qualityDropdown.value = qualityIndex;
    }

    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
        {
            qualityDropdown.value = PlayerPrefs.GetInt("QualitySettingPreference");
        }
        else
        {
            qualityDropdown.value = 3;
        }
            
        if (PlayerPrefs.HasKey("ResolutionPreference"))
        {
            resolutionDropdown.value = PlayerPrefs.GetInt("ResolutionPreference");
        }   
        else
        {
            resolutionDropdown.value = currentResolutionIndex;
        }
            
            
        if (PlayerPrefs.HasKey("FullscreenPreference"))
        {
            Screen.fullScreen = Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        }   
        else
        {
            Screen.fullScreen = true;
        }
          
        if (PlayerPrefs.HasKey("VolumePreference"))
        {
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }    
        else
        {
            volumeSlider.value = PlayerPrefs.GetFloat("VolumePreference");
        }
            
    }


    public void Exit()
    {
        pauseMenu.SetActive(true);
        settingsMenu.SetActive(false);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", resolutionDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", currentVolume);
    }
}
