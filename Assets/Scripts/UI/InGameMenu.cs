using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Header("UI References - Asigna desde el Inspector")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button controlsButton;
    [SerializeField] private Button exitToMenuButton;
    [SerializeField] private Button resumeButton;
    [SerializeField] private GameObject controlsPopup;
    [SerializeField] private GameObject exitConfirmationDialog;

    [Header("Audio Settings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private string musicVolumeParameter = "MusicVolume";
    [SerializeField] private string sfxVolumeParameter = "SFXVolume";

    [Header("Default Values (0-1)")]
    [SerializeField] private float defaultMusicVolume = 0.7f; // 70%
    [SerializeField] private float defaultSFXVolume = 0.8f; // 80%

    private bool isMenuOpen = false;

    private void Start()
    {
        InitializeUI();
        LoadAudioSettings();
    }

    private void InitializeUI()
    {
        // Configurar sliders de volumen
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            musicVolumeSlider.minValue = 0f;
            musicVolumeSlider.maxValue = 1f;
        }

        if (sfxVolumeSlider != null)
        {
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
            sfxVolumeSlider.minValue = 0f;
            sfxVolumeSlider.maxValue = 1f;
        }

        // Configurar botones
        if (controlsButton != null)
            controlsButton.onClick.AddListener(ShowControls);

        if (exitToMenuButton != null)
            exitToMenuButton.onClick.AddListener(ShowExitConfirmation);

        if (resumeButton != null)
            resumeButton.onClick.AddListener(ResumeGame);
    }

    private void LoadAudioSettings()
    {
        // Cargar configuración guardada o usar valores por defecto
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", defaultSFXVolume);

        // Aplicar volúmenes
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);

        // Actualizar sliders visuales
        if (musicVolumeSlider != null)
            musicVolumeSlider.value = musicVolume;
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = sfxVolume;
    }

    private void SaveAudioSettings()
    {
        if (musicVolumeSlider != null)
            PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
        if (sfxVolumeSlider != null)
            PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
        PlayerPrefs.Save();
    }

    // Método para subir volumen de música
    public void IncreaseMusicVolume()
    {
        if (musicVolumeSlider != null)
        {
            float newVolume = Mathf.Min(musicVolumeSlider.value + 0.1f, 1f);
            musicVolumeSlider.value = newVolume;
            SetMusicVolume(newVolume);
        }
    }

    // Método para bajar volumen de música
    public void DecreaseMusicVolume()
    {
        if (musicVolumeSlider != null)
        {
            float newVolume = Mathf.Max(musicVolumeSlider.value - 0.1f, 0f);
            musicVolumeSlider.value = newVolume;
            SetMusicVolume(newVolume);
        }
    }

    // Método para subir volumen de SFX
    public void IncreaseSFXVolume()
    {
        if (sfxVolumeSlider != null)
        {
            float newVolume = Mathf.Min(sfxVolumeSlider.value + 0.1f, 1f);
            sfxVolumeSlider.value = newVolume;
            SetSFXVolume(newVolume);
        }
    }

    // Método para bajar volumen de SFX
    public void DecreaseSFXVolume()
    {
        if (sfxVolumeSlider != null)
        {
            float newVolume = Mathf.Max(sfxVolumeSlider.value - 0.1f, 0f);
            sfxVolumeSlider.value = newVolume;
            SetSFXVolume(newVolume);
        }
    }

    private void SetMusicVolume(float volume)
    {
        // Convertir de 0-1 a dB (-80dB a 0dB para atenuación natural)
        float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        if (audioMixer != null)
        {
            audioMixer.SetFloat(musicVolumeParameter, dB);
        }
        SaveAudioSettings();
    }

    private void SetSFXVolume(float volume)
    {
        // Convertir de 0-1 a dB (-80dB a 0dB para atenuación natural)
        float dB = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
        if (audioMixer != null)
        {
            audioMixer.SetFloat(sfxVolumeParameter, dB);
        }
        SaveAudioSettings();
    }

    public void ToggleMenu()
    {
        isMenuOpen = !isMenuOpen;
        gameObject.SetActive(isMenuOpen);

        if (!isMenuOpen)
        {
            // Cerrar popups cuando se cierra el menú
            if (controlsPopup != null) controlsPopup.SetActive(false);
            if (exitConfirmationDialog != null) exitConfirmationDialog.SetActive(false);
        }
    }

    private void ShowControls()
    {
        if (controlsPopup != null)
        {
            controlsPopup.SetActive(true);
        }
    }

    private void ShowExitConfirmation()
    {
        if (exitConfirmationDialog != null)
        {
            exitConfirmationDialog.SetActive(true);
        }
    }

    public void ConfirmExitToMenu()
    {
        // Guardar configuración antes de salir
        SaveAudioSettings();

        // Cargar escena del menú principal (ajusta el nombre según tu proyecto)
        SceneManager.LoadScene("MainMenu");
    }

    public void CancelExitToMenu()
    {
        if (exitConfirmationDialog != null)
        {
            exitConfirmationDialog.SetActive(false);
        }
    }

    private void ResumeGame()
    {
        ToggleMenu();
    }

    private void OnDestroy()
    {
        // Limpiar listeners para evitar memory leaks
        if (musicVolumeSlider != null)
            musicVolumeSlider.onValueChanged.RemoveAllListeners();
        if (sfxVolumeSlider != null)
            sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        if (controlsButton != null)
            controlsButton.onClick.RemoveAllListeners();
        if (exitToMenuButton != null)
            exitToMenuButton.onClick.RemoveAllListeners();
        if (resumeButton != null)
            resumeButton.onClick.RemoveAllListeners();
    }
}