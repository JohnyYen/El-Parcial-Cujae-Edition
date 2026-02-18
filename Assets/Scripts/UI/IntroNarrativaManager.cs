using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class IntroNarrativaManager : MonoBehaviour
{
    // ========== SLIDES DE TEXTO ==========

    [Header("Slides de Texto (editables)")]
    [TextArea(3, 5)]
    [SerializeField]
    private string[] slides = new string[]
    {
        "Facultad de Informática. Universidad Cujae.",
        "Es lunes. 7:00 AM. El sol apenas asoma por las ventanas del edificio.",
        "Comienza la semana de parciales.",
        "El pasillo del segundo piso está extrañamente silencioso.",
        "Solo se escucha el zumbido de los proyectores y el trote de estudiantes desesperados.",
        "Ves a un compañero dormido sobre su laptop. El código sigue compilando.",
        "En la cafetería, el café se acabó hace media hora.",
        "Alguien grita: '¡PROFE, LAS NOTAS DEL SEMESTRE PASADO!'",
        "Silencio. Nadie responde. Nadie quiere recordar.",
        "Tu mente comienza a divagar...",
        "* flashbacks de punteros * '¿& o *? ¿Cuál era?'",
        "* flashbacks de recursividad * '¿Por qué me llamé a mí mismo infinitamente?'",
        "* flashbacks de ese segment fault que nunca entendiste *",
        "Esa vez que dijiste 'ya casi termino' a las 2 AM...",
        "...y seguías ahí a las 5 AM, preguntándote por qué elegiste esta carrera.",
        "El semáforo de compile error. El rojo de la muerte.",
        "Esa sensación cuando el código funciona... pero no sabes por qué.",
        "Y peor: cuando deja de funcionar... y TAMPOCO sabes por qué.",
        "'Solo cambia una cosita', dijiste. '¿Qué podría salir mal?', dijiste.",
        "Todo. Todo podía salir mal.",
        "Pero hoy es diferente.",
        "Hoy studiaste. Bueno... al menos miraste los apuntes una vez.",
        "El café de esta mañana fue tu patrocinador oficial.",
        "La determinación en tus ojos es... aceptable.",
        "Respiras hondo. Sujetas tu lápiz como si fuera un arma.",
        "Es hora. El aula se abre. ¡El Parcial te espera!"
    };

    // ========== TIMING ==========

    [Header("Timing")]
    [SerializeField] private float timePerSlide = 4f;
    [SerializeField] private float typewriterSpeed = 0.04f;
    [SerializeField] private float fadeDuration = 0.5f;

    // ========== ENHANCED VISUALS ==========

    [Header("Enhanced Visuals")]
    [SerializeField] private bool useSlideUpAnimation = true;
    [SerializeField] private float slideUpDistance = 40f;
    [SerializeField] private float slideUpDuration = 0.5f;
    [SerializeField] private AnimationCurve slideUpCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));
    [SerializeField] private bool useBlinkIndicator = true;
    [SerializeField] private GameObject continueIndicator;
    [SerializeField] private float blinkInterval = 0.5f;
    [SerializeField] private bool useCrossfade = true;
    [SerializeField] private float crossfadeDuration = 0.3f;

    // ========== AUDIO ==========

    [Header("Audio")]
    [SerializeField] private AudioClip ambientMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.4f;

    // ========== SCENE ==========

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "LevelSelect";

    // ========== UI REFERENCES ==========

    [Header("UI References")]
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private Button skipButton;
    [SerializeField] private Slider progressBar;
    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private CanvasGroup textCanvasGroup;

    // ========== ESTADO PRIVADO ==========

    private int currentSlideIndex = 0;
    private bool isTypewriting = false;
    private Coroutine currentTypewriterCoroutine;
    private Coroutine autoAdvanceCoroutine;
    private Coroutine blinkCoroutine;
    private AudioSource audioSource;
    private Vector2 originalTextPosition;

    // ========== INICIALIZACIÓN ==========

    private void Awake()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
        }

        // Guardar posición original del texto
        if (introText != null)
        {
            originalTextPosition = introText.rectTransform.anchoredPosition;
        }
    }

    private void Start()
    {
        SetupAudio();
        SetupUI();
        UpdateProgressBar();
        StartCoroutine(PlayIntroSequence());
    }

    private void SetupAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ambientMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (ambientMusic != null)
        {
            audioSource.Play();
        }
    }

    private void SetupUI()
    {
        if (skipButton != null)
        {
            skipButton.onClick.AddListener(SkipIntro);
        }

        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }

        if (continueIndicator != null)
        {
            continueIndicator.SetActive(false);
        }

        // Configurar textCanvasGroup si existe
        if (textCanvasGroup == null && introText != null)
        {
            textCanvasGroup = introText.GetComponent<CanvasGroup>();
            if (textCanvasGroup == null)
            {
                textCanvasGroup = introText.gameObject.AddComponent<CanvasGroup>();
            }
        }
    }

    private void Update()
    {
        HandleInput();
    }

    // ========== INPUT ==========

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            AdvanceToNextSlide();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SkipIntro();
        }
    }

    // ========== SECUENCIA PRINCIPAL ==========

    private IEnumerator PlayIntroSequence()
    {
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));
        }

        while (currentSlideIndex < slides.Length)
        {
            // Crossfade del slide anterior si es necesario
            if (useCrossfade && currentSlideIndex > 0 && textCanvasGroup != null)
            {
                yield return StartCoroutine(CrossfadeOutPrevious());
            }

            yield return StartCoroutine(ShowSlide(currentSlideIndex));

            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceTimer());
            yield return autoAdvanceCoroutine;
        }

        yield return StartCoroutine(GoToNextScene());
    }

    private IEnumerator ShowSlide(int index)
    {
        if (introText != null)
        {
            // Resetear posición y alpha
            introText.alpha = 0f;
            introText.text = slides[index];
            introText.maxVisibleCharacters = 0;

            // Slide-up animation
            if (useSlideUpAnimation)
            {
                introText.rectTransform.anchoredPosition = originalTextPosition + new Vector2(0, slideUpDistance);
            }

            // Ocultar indicador
            HideBlinkIndicator();

            // Ejecutar animación de entrada (slide-up + fade-in)
            if (useSlideUpAnimation)
            {
                StartCoroutine(SlideUpFadeIn());
            }
            else
            {
                yield return StartCoroutine(FadeText(introText, 0f, 1f, fadeDuration));
            }

            // Iniciar typewriter
            currentTypewriterCoroutine = StartCoroutine(TypewriterEffect(introText));
        }
    }

    // ========== TYPEWRITER ==========

    private IEnumerator TypewriterEffect(TMP_Text textComponent)
    {
        isTypewriting = true;
        textComponent.ForceMeshUpdate();

        int totalVisibleCharacters = textComponent.textInfo.characterCount;
        int counter = 0;

        while (counter < totalVisibleCharacters)
        {
            counter++;
            textComponent.maxVisibleCharacters = counter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTypewriting = false;
        ShowHint();
    }

    // ========== SLIDE-UP ANIMATION ==========

    private IEnumerator SlideUpFadeIn()
    {
        float elapsed = 0f;
        float fadeElapsed = 0f;
        Vector2 startPos = originalTextPosition + new Vector2(0, slideUpDistance);
        Vector2 endPos = originalTextPosition;

        //同时播放位置动画和淡入动画
        while (elapsed < slideUpDuration || fadeElapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeElapsed += Time.deltaTime;

            // 位置插值 (使用曲线)
            float t = slideUpCurve.Evaluate(Mathf.Clamp01(elapsed / slideUpDuration));
            introText.rectTransform.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            // Alpha淡入
            if (fadeElapsed < fadeDuration)
            {
                introText.alpha = Mathf.Clamp01(fadeElapsed / fadeDuration);
            }

            yield return null;
        }

        // 确保最终状态正确
        introText.rectTransform.anchoredPosition = endPos;
        introText.alpha = 1f;
    }

    // ========== CROSSFADE ==========

    private IEnumerator CrossfadeOutPrevious()
    {
        if (textCanvasGroup == null) yield break;

        float elapsed = 0f;
        float startAlpha = textCanvasGroup.alpha;

        while (elapsed < crossfadeDuration)
        {
            elapsed += Time.deltaTime;
            textCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / crossfadeDuration);
            yield return null;
        }

        textCanvasGroup.alpha = 0f;
    }

    // ========== BLINK INDICATOR ==========

    private void ShowHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(true);
        }

        if (useBlinkIndicator && continueIndicator != null)
        {
            continueIndicator.SetActive(true);
            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkAnimation());
        }
    }

    private void HideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }

        HideBlinkIndicator();
    }

    private void HideBlinkIndicator()
    {
        if (continueIndicator != null)
        {
            continueIndicator.SetActive(false);
        }
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
        }
    }

    private IEnumerator BlinkAnimation()
    {
        CanvasGroup cg = continueIndicator.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = continueIndicator.AddComponent<CanvasGroup>();
        }

        while (true)
        {
            // Fade in
            float elapsed = 0f;
            while (elapsed < blinkInterval / 2f)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Lerp(0f, 1f, elapsed / (blinkInterval / 2f));
                yield return null;
            }

            // Fade out
            elapsed = 0f;
            while (elapsed < blinkInterval / 2f)
            {
                elapsed += Time.deltaTime;
                cg.alpha = Mathf.Lerp(1f, 0f, elapsed / (blinkInterval / 2f));
                yield return null;
            }
        }
    }

    // ========== AUTO ADVANCE ==========

    private IEnumerator AutoAdvanceTimer()
    {
        float timer = 0f;

        while (timer < timePerSlide)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        currentSlideIndex++;
        UpdateProgressBar();
    }

    // ========== NAVEGACIÓN ==========

    private void AdvanceToNextSlide()
    {
        if (isTypewriting)
        {
            CompleteTypewriterImmediately();
            return;
        }

        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
        }

        currentSlideIndex++;

        if (currentSlideIndex >= slides.Length)
        {
            StartCoroutine(GoToNextScene());
        }
    }

    private void CompleteTypewriterImmediately()
    {
        if (currentTypewriterCoroutine != null)
        {
            StopCoroutine(currentTypewriterCoroutine);
        }

        if (introText != null)
        {
            introText.maxVisibleCharacters = introText.textInfo.characterCount;
            introText.alpha = 1f;
            
            // 确保位置正确
            if (useSlideUpAnimation)
            {
                introText.rectTransform.anchoredPosition = originalTextPosition;
            }
        }

        isTypewriting = false;
        ShowHint();
    }

    // ========== PROGRESS BAR ==========

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = slides.Length;
            progressBar.value = currentSlideIndex;
        }
    }

    // ========== SKIP ==========

    public void SkipIntro()
    {
        StopAllCoroutines();
        HideBlinkIndicator();
        StartCoroutine(GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        HideHint();

        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));
        }
        else
        {
            yield return new WaitForSeconds(0.2f);
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Usar SceneFader si está disponible
        SceneFader fader = FindFirstObjectByType<SceneFader>();
        if (fader != null)
        {
            fader.FadeAndLoadScene(nextSceneName, 0.5f);
        }
        else if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    // ========== FADE HELPERS ==========

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        if (fadeCanvasGroup == null) yield break;

        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out quadratic
            t = 1f - (1f - t) * (1f - t);
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }

    private IEnumerator FadeText(TMP_Text text, float from, float to, float duration)
    {
        float elapsed = 0f;
        text.alpha = from;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            // Ease out quadratic
            t = 1f - (1f - t) * (1f - t);
            text.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        text.alpha = to;
    }

    // ========== CLEANUP ==========

    private void OnDestroy()
    {
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(SkipIntro);
        }

        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
    }
}
