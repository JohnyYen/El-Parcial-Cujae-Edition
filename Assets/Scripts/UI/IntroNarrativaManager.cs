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

    [Header("Typewriter Audio")]
    [SerializeField] private AudioClip typewriterSound;
    [SerializeField][Range(0f, 1f)] private float typewriterVolume = 0.5f;
    [SerializeField] private bool playTypewriterSound = true;

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
    private Coroutine currentSlideCoroutine;
    private Coroutine currentTypewriterCoroutine;
    private Coroutine autoAdvanceCoroutine;
    private Coroutine blinkCoroutine;
    private AudioSource audioSource;
    private AudioSource typewriterAudioSource;
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
        // Audio ambiente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = ambientMusic;
        audioSource.volume = musicVolume;
        audioSource.loop = true;
        audioSource.playOnAwake = false;

        if (ambientMusic != null)
        {
            audioSource.Play();
        }

        // Audio de typewriter
        if (typewriterSound != null)
        {
            typewriterAudioSource = gameObject.AddComponent<AudioSource>();
            typewriterAudioSource.clip = typewriterSound;
            typewriterAudioSource.volume = typewriterVolume;
            typewriterAudioSource.loop = true;
            typewriterAudioSource.playOnAwake = false;
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

        // Asegurar que el texto sea visible (sin CanvasGroup que pueda bloquear)
        if (introText != null)
        {
            introText.alpha = 1f;
            
            // Eliminar cualquier CanvasGroup que pueda estar bloquendo la visibilidad
            CanvasGroup existingCG = introText.GetComponent<CanvasGroup>();
            if (existingCG != null)
            {
                existingCG.alpha = 1f;
                existingCG.blocksRaycasts = false;
                existingCG.interactable = false;
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
            Debug.Log("Space/Return presionado - Avanzando slide");
            AdvanceToNextSlide();
        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("KeypadEnter presionado - Saltando intro");
            SkipIntro();
        }
    }

    // ========== SECUENCIA PRINCIPAL ==========

    private IEnumerator PlayIntroSequence()
    {
        // Fade in inicial
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));
        }

        // Asegurar que el texto sea visible al inicio
        if (introText != null)
        {
            introText.alpha = 1f;
        }

        while (currentSlideIndex < slides.Length)
        {
            // Mostrar el slide actual y guardar referencia
            currentSlideCoroutine = StartCoroutine(ShowSlide(currentSlideIndex));
            yield return currentSlideCoroutine;

            // Esperar el tiempo del slide O hasta que el usuario avance
            autoAdvanceCoroutine = StartCoroutine(AutoAdvanceTimer());
            yield return autoAdvanceCoroutine;

            // Avanzar al siguiente slide (auto advance)
            currentSlideIndex++;
            UpdateProgressBar();
        }

        yield return StartCoroutine(GoToNextScene());
    }

    private IEnumerator ShowSlide(int index)
    {
        Debug.Log($"ShowSlide llamado con índice: {index}");

        if (introText != null)
        {
            // Resetear texto y posición - FORZAR visibilidad
            introText.text = slides[index];
            introText.maxVisibleCharacters = 0;
            introText.alpha = 1f;
            
            // Asegurar que cualquier CanvasGroup tenga alpha = 1
            CanvasGroup cg = introText.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 1f;
                cg.blocksRaycasts = false;
            }

            Debug.Log($"Texto configurado: {slides[index]}, Alpha: {introText.alpha}");

            // Posición inicial
            if (useSlideUpAnimation)
            {
                introText.rectTransform.anchoredPosition = originalTextPosition + new Vector2(0, slideUpDistance);
            }

            // Ocultar indicador
            HideBlinkIndicator();

            // Animación de entrada
            if (useSlideUpAnimation)
            {
                yield return StartCoroutine(SlideUpFadeIn());
            }
            else
            {
                yield return StartCoroutine(FadeText(introText, 0f, 1f, fadeDuration));
            }

            Debug.Log($"Slide {index} mostrado, iniciando typewriter");

            // typewriter
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

        // INICIAR AUDIO de typewriter
        if (playTypewriterSound && typewriterAudioSource != null && typewriterSound != null)
        {
            typewriterAudioSource.time = 0f;
            typewriterAudioSource.Play();
        }

        while (counter < totalVisibleCharacters)
        {
            counter++;
            textComponent.maxVisibleCharacters = counter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        // DETENER AUDIO de typewriter
        if (typewriterAudioSource != null && typewriterAudioSource.isPlaying)
        {
            typewriterAudioSource.Stop();
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

        // El avance del índice ahora se maneja en PlayIntroSequence
    }

    // ========== NAVEGACIÓN ==========

    private void AdvanceToNextSlide()
    {
        Debug.Log($"AdvanceToNextSlide - isTypewriting: {isTypewriting}, currentSlideIndex: {currentSlideIndex}");

        // Detener cualquier corrutina de slide en progreso
        if (currentSlideCoroutine != null)
        {
            StopCoroutine(currentSlideCoroutine);
        }
        if (currentTypewriterCoroutine != null)
        {
            StopCoroutine(currentTypewriterCoroutine);
        }
        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
        }

        // Si está mostrando el slide actual, completar el typewriter primero
        if (isTypewriting)
        {
            CompleteTypewriterImmediately();
        }

        // Ocultar hint
        HideHint();

        // Incrementar índice
        currentSlideIndex++;
        UpdateProgressBar();

        Debug.Log($"Nuevo índice: {currentSlideIndex}, Total slides: {slides.Length}");

        // Si llegamos al final, ir a siguiente escena
        if (currentSlideIndex >= slides.Length)
        {
            StartCoroutine(GoToNextScene());
            return;
        }

        // MOSTRAR SIGUIENTE SLIDE INMEDIATAMENTE
        currentSlideCoroutine = StartCoroutine(ShowSlide(currentSlideIndex));
    }

    private void CompleteTypewriterImmediately()
    {
        if (currentTypewriterCoroutine != null)
        {
            StopCoroutine(currentTypewriterCoroutine);
        }

        // Detener audio de typewriter
        if (typewriterAudioSource != null && typewriterAudioSource.isPlaying)
        {
            typewriterAudioSource.Stop();
        }

        if (introText != null)
        {
            introText.maxVisibleCharacters = introText.textInfo.characterCount;
            introText.alpha = 1f;
            
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
        Debug.Log("=== SKIP INTRO INICIADO ===");
        StopAllCoroutines();
        HideBlinkIndicator();

        // Detener audio de typewriter
        if (typewriterAudioSource != null && typewriterAudioSource.isPlaying)
        {
            Debug.Log("Deteniendo audio typewriter");
            typewriterAudioSource.Stop();
        }

        Debug.Log("Iniciando GoToNextScene coroutine");
        StartCoroutine(GoToNextScene());
    }

    private IEnumerator GoToNextScene()
    {
        Debug.Log("=== GO TO NEXT SCENE INICIADO ===");
        Debug.Log($"Escena destino: {nextSceneName}");

        HideHint();

        // Detener todos los audios
        if (typewriterAudioSource != null && typewriterAudioSource.isPlaying)
        {
            Debug.Log("Deteniendo audio typewriter en GoToNextScene");
            typewriterAudioSource.Stop();
        }

        if (audioSource != null && audioSource.isPlaying)
        {
            Debug.Log("Deteniendo audio ambiente");
            audioSource.Stop();
        }

        // Usar SceneFader si está disponible (prioridad)
        SceneFader fader = FindFirstObjectByType<SceneFader>();
        if (fader != null)
        {
            Debug.Log($"SceneFader encontrado: {fader.gameObject.name}");
            Debug.Log($"Llamando FadeAndLoadScene con: {nextSceneName}, duración: 0.5s");
            fader.FadeAndLoadScene(nextSceneName, 0.5f);
            Debug.Log("SceneFader.FadeAndLoadScene llamado, saliendo de GoToNextScene");
            yield break;  // SceneFader maneja todo, salir aquí
        }
        else
        {
            Debug.LogWarning("SceneFader NO encontrado, usando fallback con fadeCanvasGroup");
        }

        // Fallback: usar fadeCanvasGroup nativo si no hay SceneFader
        if (fadeCanvasGroup != null)
        {
            Debug.Log($"Usando fadeCanvasGroup para fade a negro, duración: {fadeDuration}s");
            yield return StartCoroutine(FadeCanvas(0f, 1f, fadeDuration));
            Debug.Log("Fade a negro completado");
        }
        else
        {
            Debug.Log("fadeCanvasGroup no asignado, esperando 0.2s");
            yield return new WaitForSeconds(0.2f);
        }

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            Debug.Log($"Cargando escena directamente: {nextSceneName}");
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogError("nextSceneName está vacío o es null!");
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
