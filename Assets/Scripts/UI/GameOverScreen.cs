using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text livesText;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject menuButton;
    [SerializeField] private GameObject quitButton;

    [Header("Scenes")]
    [SerializeField] private string gameplaySceneName = "Gameplay";
    [SerializeField] private string titleScreenSceneName = "TitleScreen";

    [Header("Animation")]
    [SerializeField] private CanvasGroup messageGroup;
    [SerializeField] private CanvasGroup buttonsGroup;
    [SerializeField] private float fadeSpeed = 2f;

    private int lives;
    private bool buttonsReady;
    private float messageAlpha;
    private float buttonsAlpha;
    private float buttonsDelay = 1f;
    private float timer;

    private void Start()
    {
        Application.targetFrameRate = 60;

        lives = PlayerPrefs.GetInt("PlayerLives", 2);
        livesText.text = $"Tienes {lives}";

        if (lives <= 0)
        {
            livesText.text = "No te quedan vidas";
            retryButton.SetActive(false);
        }

        // Start fade-in animation
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

        if ((kb.enterKey.wasPressedThisFrame || kb.rKey.wasPressedThisFrame) && lives > 0)
            OnRetry();
        else if (kb.escapeKey.wasPressedThisFrame || kb.mKey.wasPressedThisFrame)
            OnMainMenu();
        else if (kb.qKey.wasPressedThisFrame)
            OnQuit();
    }

    public void OnRetry()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnMainMenu()
    {
        SceneManager.LoadScene(titleScreenSceneName);
    }

    public void OnQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void SelectFirstButton()
    {
        if (lives > 0)
            EventSystem.current.SetSelectedGameObject(retryButton);
        else
            EventSystem.current.SetSelectedGameObject(menuButton);
    }
}
