using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Sistema de fade reutilizable para transiciones entre escenas.
/// Se mantiene persistente entre escenas con DontDestroyOnLoad.
/// </summary>
[RequireComponent(typeof(CanvasGroup))]
public class SceneFader : MonoBehaviour
{
    // ========== SINGLETON ==========

    public static SceneFader Instance { get; private set; }

    // ========== CONFIGURACIÓN ==========

    [Header("Settings")]
    [SerializeField] private float defaultFadeDuration = 0.5f;
    [SerializeField] private bool fadeOnStart = true;
    [SerializeField] private float startFadeInDuration = 0.5f;

    // ========== COMPONENTES ==========

    private CanvasGroup canvasGroup;
    private bool isFading;
    private Coroutine currentFadeCoroutine;

    // ========== EVENTOS ==========

    public event Action OnFadeInComplete;
    public event Action OnFadeOutComplete;
    public event Action OnFadeInOutComplete;

    // ========== PROPIEDADES ==========

    public bool IsFading => isFading;
    public CanvasGroup CanvasGroup => canvasGroup;

    // ========== INICIALIZACIÓN ==========

    private void Awake()
    {
        Debug.Log("=== SCENE FADER AWAKE ===");

        // Singleton
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning($"SceneFader duplicado detectado, destruyendo: {gameObject.name}");
            Destroy(gameObject);
            return;
        }

        Instance = this;
        Debug.Log($"SceneFader Instance asignado: {gameObject.name}");

        // No destruir entre escenas
        DontDestroyOnLoad(gameObject);
        Debug.Log("DontDestroyOnLoad aplicado");

        // Obtener o añadir CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // Configurar inicialmente
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0f;  // Iniciar transparente

        Debug.Log($"CanvasGroup configurado - Alpha: {canvasGroup.alpha}, BlocksRaycasts: {canvasGroup.blocksRaycasts}");

        // Asegurar que el canvas esté configurado correctamente
        SetupCanvas();
    }

    private void Start()
    {
        Debug.Log($"=== SCENE FADER START === fadeOnStart: {fadeOnStart}");
        
        if (fadeOnStart)
        {
            // Iniciar con fade in desde negro
            Debug.Log($"Iniciando FadeIn con duración: {startFadeInDuration}s");
            FadeIn(startFadeInDuration);
        }
    }

    private void SetupCanvas()
    {
        Canvas canvas = GetComponent<Canvas>();
        if (canvas == null)
        {
            canvas = gameObject.AddComponent<Canvas>();
        }

        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999; // Siempre arriba de todo

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = gameObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;
        }

        // Asegurar que hay una imagen de fondo
        Transform panelTransform = transform.GetChild(0);
        if (panelTransform != null)
        {
            UnityEngine.UI.Image panelImage = panelTransform.GetComponent<UnityEngine.UI.Image>();
            if (panelImage != null)
            {
                panelImage.color = Color.black;
                panelImage.fillCenter = true;
            }
        }
    }

    // ========== MÉTODOS PÚBLICOS ==========

    /// <summary>
    /// Fade in: de negro (alpha 1) a transparente (alpha 0).
    /// </summary>
    /// <param name="duration">Duración del fade. Si es -1, usa defaultFadeDuration.</param>
    public void FadeIn(float duration = -1f)
    {
        float actualDuration = duration < 0 ? defaultFadeDuration : duration;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        // Asegurar que start desde 1 (negro)
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        currentFadeCoroutine = StartCoroutine(FadeRoutine(1f, 0f, actualDuration, () =>
        {
            OnFadeInComplete?.Invoke();
            canvasGroup.blocksRaycasts = false;
        }));
    }

    /// <summary>
    /// Fade out: de transparente (alpha 0) a negro (alpha 1).
    /// </summary>
    /// <param name="duration">Duración del fade. Si es -1, usa defaultFadeDuration.</param>
    public void FadeOut(float duration = -1f)
    {
        float actualDuration = duration < 0 ? defaultFadeDuration : duration;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        // Asegurar que start desde 0 (transparente)
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = true;

        currentFadeCoroutine = StartCoroutine(FadeRoutine(0f, 1f, actualDuration, () =>
        {
            OnFadeOutComplete?.Invoke();
        }));
    }

    /// <summary>
    /// Secuencia completa: fade out → acción → fade in.
    /// </summary>
    /// <param name="fadeOutDuration">Duración del fade out.</param>
    /// <param name="fadeInDuration">Duración del fade in.</param>
    /// <param name="onMiddle">Acción a ejecutar entre ambos fades.</param>
    public void FadeInOut(float fadeOutDuration, float fadeInDuration, Action onMiddle = null)
    {
        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeInOutRoutine(fadeOutDuration, fadeInDuration, onMiddle));
    }

    /// <summary>
    /// Fade out y carga una nueva escena.
    /// </summary>
    /// <param name="sceneName">Nombre de la escena a cargar.</param>
    /// <param name="fadeOutDuration">Duración del fade out. Si es -1, usa defaultFadeDuration.</param>
    public void FadeAndLoadScene(string sceneName, float fadeOutDuration = -1f)
    {
        float actualDuration = fadeOutDuration < 0 ? defaultFadeDuration : fadeOutDuration;

        if (currentFadeCoroutine != null)
        {
            StopCoroutine(currentFadeCoroutine);
        }

        currentFadeCoroutine = StartCoroutine(FadeAndLoadSceneRoutine(sceneName, actualDuration));
    }

    /// <summary>
    /// Fade out, espera, carga escena, fade in.
    /// </summary>
    public void FadeTransition(string sceneName, float fadeOutDuration = -1f, float fadeInDuration = -1f)
    {
        float outDuration = fadeOutDuration < 0 ? defaultFadeDuration : fadeOutDuration;
        float inDuration = fadeInDuration < 0 ? defaultFadeDuration : fadeInDuration;

        StartCoroutine(FadeTransitionRoutine(sceneName, outDuration, inDuration));
    }

    /// <summary>
    /// Configura la duración por defecto del fade.
    /// </summary>
    public void SetDefaultDuration(float duration)
    {
        defaultFadeDuration = duration;
    }

    /// <summary>
    /// Establece el alpha del canvas directamente (sin animación).
    /// </summary>
    public void SetAlpha(float alpha)
    {
        canvasGroup.alpha = Mathf.Clamp01(alpha);
    }

    // ========== CORRUTINAS ==========

    private IEnumerator FadeRoutine(float from, float to, float duration, Action onComplete = null)
    {
        isFading = true;
        float elapsed = 0f;

        canvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out quadratic
            t = 1f - (1f - t) * (1f - t);

            canvasGroup.alpha = Mathf.Lerp(from, to, t);

            yield return null;
        }

        canvasGroup.alpha = to;
        isFading = false;
        onComplete?.Invoke();
    }

    private IEnumerator FadeInOutRoutine(float fadeOutDuration, float fadeInDuration, Action onMiddle)
    {
        isFading = true;

        // Fade out (a negro)
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Ejecutar acción del medio
        onMiddle?.Invoke();

        // Fade in (desde negro)
        elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        isFading = false;

        OnFadeInOutComplete?.Invoke();
    }

    private IEnumerator FadeAndLoadSceneRoutine(string sceneName, float fadeOutDuration)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true;

        // Fade out a negro
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Cargar escena
        SceneManager.LoadScene(sceneName);
        
        // Esperar un frame para que la nueva escena se inicialice
        yield return null;

        // Fade in desde negro (manual porque DontDestroyOnLoad no ejecuta Awake de nuevo)
        elapsed = 0f;
        float fadeInDuration = defaultFadeDuration;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        isFading = false;
    }

    private IEnumerator FadeTransitionRoutine(string sceneName, float fadeOutDuration, float fadeInDuration)
    {
        isFading = true;
        canvasGroup.blocksRaycasts = true;

        // Fade out a negro
        float elapsed = 0f;
        while (elapsed < fadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeOutDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        // Cargar escena
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Fade in desde negro
        elapsed = 0f;
        while (elapsed < fadeInDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeInDuration;
            t = 1f - (1f - t) * (1f - t);
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        isFading = false;

        OnFadeInComplete?.Invoke();
    }
}
