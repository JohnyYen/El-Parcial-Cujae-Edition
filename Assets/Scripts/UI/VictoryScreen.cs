using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Pantalla de victoria que se muestra al derrotar al boss.
/// Panel UI con fade-in animado, botones y atajos de teclado.
/// </summary>
public class VictoryScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text victoryText;
    [SerializeField] private GameObject continueButton;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject quitButton;

    [Header("Scenes")]
    [SerializeField] private string levelSelectSceneName = "LevelSelect";
    [SerializeField] private string titleScreenSceneName = "TitleScreen";

    [Header("Animation")]
    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private CanvasGroup buttonsGroup;
    [SerializeField] private float fadeSpeed = 2f;

    [Header("Victory Audio")]
    [SerializeField] private AudioClip victoryMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.7f;
    [SerializeField] private bool loopMusic = true;

    private AudioSource audioSource;
    private bool buttonsReady;
    private float messageAlpha;
    private float buttonsAlpha;
    private float buttonsDelay = 1f;
    private float timer;

    private void Start()
    {
        gameObject.SetActive(false);
        Application.targetFrameRate = 60;

        // Initialize hidden state
        messageAlpha = 0f;
        buttonsAlpha = 0f;
        buttonsReady = false;
        timer = 0f;

        if (messageGroup != null) messageGroup.alpha = 0f;
        if (buttonsGroup != null) buttonsGroup.alpha = 0f;
    }

    private void Update()
    {
        timer += Time.unscaledDeltaTime;

        // Fade in message
        if (messageGroup != null && messageAlpha < 1f)
        {
            messageAlpha = Mathf.MoveTowards(messageAlpha, 1f, fadeSpeed * Time.unscaledDeltaTime);
            messageGroup.alpha = messageAlpha;
        }

        // Fade in buttons after delay
        if (timer > buttonsDelay && buttonsGroup != null)
        {
            buttonsAlpha = Mathf.MoveTowards(buttonsAlpha, 1f, fadeSpeed * Time.unscaledDeltaTime);
            buttonsGroup.alpha = buttonsAlpha;

            if (!buttonsReady && buttonsAlpha >= 1f)
            {
                buttonsReady = true;
                buttonsGroup.interactable = true;
                SelectFirstButton();
            }
        }

        // Keyboard shortcuts (only when buttons are visible)
        if (!buttonsReady) return;

        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.enterKey.wasPressedThisFrame || kb.cKey.wasPressedThisFrame)
            OnContinue();
        else if (kb.escapeKey.wasPressedThisFrame || kb.mKey.wasPressedThisFrame)
            OnMainMenu();
        else if (kb.qKey.wasPressedThisFrame)
            OnQuit();
    }

    /// <summary>
    /// Muestra la pantalla de victoria.
    /// Llamar desde Boss.cs cuando el boss muere.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        
        // Reproducir música de victoria
        PlayVictoryMusic();
    }

    /// <summary>
    /// Reproduce la música de victoria.
    /// </summary>
    private void PlayVictoryMusic()
    {
        if (victoryMusic != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = victoryMusic;
            audioSource.volume = musicVolume;
            audioSource.loop = loopMusic;
            audioSource.playOnAwake = false;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("VictoryMusic no está asignado en el Inspector!");
        }
    }

    /// <summary>
    /// Oculta la pantalla de victoria.
    /// </summary>
    public void Hide()
    {
        // Detener música si está sonando
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnContinue()
    {
        // Detener música de victoria
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        Time.timeScale = 1f;
        
        SceneFader fader = FindFirstObjectByType<SceneFader>();
        if (fader != null)
        {
            fader.FadeAndLoadScene(levelSelectSceneName, 0.5f);
        }
        else
        {
            SceneManager.LoadScene(levelSelectSceneName);
        }
    }

    public void OnMainMenu()
    {
        // Detener música de victoria
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
        Time.timeScale = 1f;

        SceneFader fader = FindFirstObjectByType<SceneFader>();
        if (fader != null)
        {
            fader.FadeAndLoadScene(titleScreenSceneName, 0.5f);
        }
        else
        {
            SceneManager.LoadScene(titleScreenSceneName);
        }
    }

    public void OnQuit()
    {
        // Detener música de victoria
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SelectFirstButton()
    {
        if (continueButton != null)
            EventSystem.current.SetSelectedGameObject(continueButton);
        else if (menuButton != null)
            EventSystem.current.SetSelectedGameObject(menuButton);
    }
}
