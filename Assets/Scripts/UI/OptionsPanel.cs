using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class OptionsPanel : MonoBehaviour
{
    [Header("Audio Sliders (lógica de audio la conecta otro integrante)")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [Header("Display")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;

    private Resolution[] availableResolutions;
    private int currentResolutionIndex;

    private void OnEnable()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
        PopulateResolutions();

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }

    private void PopulateResolutions()
    {
        Resolution[] allResolutions = Screen.resolutions;
        var filtered = new List<Resolution>();
        var seen = new HashSet<string>();

        for (int i = 0; i < allResolutions.Length; i++)
        {
            string key = $"{allResolutions[i].width}x{allResolutions[i].height}";
            if (allResolutions[i].width >= 1280 && seen.Add(key))
            {
                filtered.Add(allResolutions[i]);
            }
        }

        availableResolutions = filtered.ToArray();
        resolutionDropdown.ClearOptions();

        var options = new List<string>();
        currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            options.Add($"{availableResolutions[i].width} x {availableResolutions[i].height}");
            if (availableResolutions[i].width == Screen.currentResolution.width &&
                availableResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void OnApply()
    {
        if (availableResolutions.Length > 0)
        {
            Resolution res = availableResolutions[resolutionDropdown.value];
            Screen.SetResolution(res.width, res.height, fullscreenToggle.isOn);
        }

        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    public void OnDefaults()
    {
        musicSlider.value = 0.7f;
        sfxSlider.value = 1f;
        fullscreenToggle.isOn = true;
    }
}
