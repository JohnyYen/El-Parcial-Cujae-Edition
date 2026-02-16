using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameHUD : MonoBehaviour
{
    [Header("Stress Bar (Health)")]
    [SerializeField] private Slider stressBar;
    [SerializeField] private Image stressBarFill;
    [SerializeField] private Color normalColor = new Color(0.2f, 0.8f, 0.2f);
    [SerializeField] private Color criticalColor = new Color(0.9f, 0.1f, 0.1f);
    [SerializeField] private float criticalThreshold = 0.8f;

    [Header("Focus Bar")]
    [SerializeField] private Slider focusBar;
    [SerializeField] private Image focusBarFill;
    [SerializeField] private Color focusColor = new Color(0.2f, 0.6f, 1f);
    [SerializeField] private Color focusFullColor = new Color(1f, 0.9f, 0.2f);

    [Header("Damage Flash")]
    [SerializeField] private Image damageOverlay;
    [SerializeField] private float flashDuration = 0.3f;

    [Header("Options")]
    [SerializeField] private GameObject pausePanel;

    private float currentStress;
    private float targetStress;
    private float currentFocus;
    private float targetFocus;
    private float flashTimer;
    private bool isPaused;
    private float smoothSpeed = 5f;

    private void Start()
    {
        stressBar.value = 0f;
        focusBar.value = 0f;
        currentStress = 0f;
        targetStress = 0f;
        currentFocus = 0f;
        targetFocus = 0f;

        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = 0f;
            damageOverlay.color = c;
        }

        if (pausePanel != null) pausePanel.SetActive(false);
        isPaused = false;
    }

    private void Update()
    {
        UpdateStressBar();
        UpdateFocusBar();
        UpdateDamageFlash();
        HandlePauseInput();
    }

    // --- Public methods for Gameplay scripts to call ---

    public void SetStress(float value01)
    {
        targetStress = Mathf.Clamp01(value01);
    }

    public void AddStress(float amount)
    {
        targetStress = Mathf.Clamp01(targetStress + amount);
        TriggerDamageFlash();
    }

    public void SetFocus(float value01)
    {
        targetFocus = Mathf.Clamp01(value01);
    }

    public void AddFocus(float amount)
    {
        targetFocus = Mathf.Clamp01(targetFocus + amount);
    }

    public void UseFocus(float amount)
    {
        targetFocus = Mathf.Clamp01(targetFocus - amount);
    }

    public float GetStress() => currentStress;
    public float GetFocus() => currentFocus;

    public void SetVisible(bool visible)
    {
        gameObject.SetActive(visible);
    }

    // --- Internal updates ---

    private void UpdateStressBar()
    {
        currentStress = Mathf.Lerp(currentStress, targetStress, smoothSpeed * Time.deltaTime);
        stressBar.value = currentStress;

        if (stressBarFill != null)
        {
            bool isCritical = currentStress >= criticalThreshold;
            Color target = isCritical ? criticalColor : normalColor;
            stressBarFill.color = Color.Lerp(stressBarFill.color, target, smoothSpeed * Time.deltaTime);

            // Pulsing effect when critical
            if (isCritical)
            {
                float pulse = Mathf.PingPong(Time.time * 3f, 1f) * 0.3f + 0.7f;
                Color c = stressBarFill.color;
                c.a = pulse;
                stressBarFill.color = c;
            }
        }
    }

    private void UpdateFocusBar()
    {
        currentFocus = Mathf.Lerp(currentFocus, targetFocus, smoothSpeed * Time.deltaTime);
        focusBar.value = currentFocus;

        if (focusBarFill != null)
        {
            bool isFull = currentFocus >= 0.99f;
            Color target = isFull ? focusFullColor : focusColor;
            focusBarFill.color = Color.Lerp(focusBarFill.color, target, smoothSpeed * Time.deltaTime);
        }
    }

    private void TriggerDamageFlash()
    {
        flashTimer = flashDuration;
    }

    private void UpdateDamageFlash()
    {
        if (damageOverlay == null) return;

        if (flashTimer > 0f)
        {
            flashTimer -= Time.deltaTime;
            Color c = damageOverlay.color;
            c.a = flashTimer / flashDuration * 0.4f;
            damageOverlay.color = c;
        }
    }

    private void HandlePauseInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.escapeKey.wasPressedThisFrame || kb.pKey.wasPressedThisFrame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (pausePanel != null) pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0f : 1f;
    }

    public void OnResume()
    {
        isPaused = false;
        if (pausePanel != null) pausePanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("TitleScreen");
    }
}
