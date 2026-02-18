using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections;
using UnityEngine.UI;

public class IntroNarrativaManager : MonoBehaviour
{
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
        "Hoy estudiaste. Bueno... al menos miraste los apuntes una vez.",
        "El café de esta mañana fue tu patrocinador oficial.",
        "La determinación en tus ojos es... aceptable.",
        "Respiras hondo. Sujetas tu lápiz como si fuera un arma.",
        "Es hora. El aula se abre. ¡El Parcial te espera!"
    };

    [Header("Timing")]
    [SerializeField] private float timePerSlide = 3.5f;
    [SerializeField] private float typewriterSpeed = 0.03f;
    [SerializeField] private float fadeDuration = 0.3f;

    [Header("Audio")]
    [SerializeField] private AudioClip ambientMusic;
    [SerializeField][Range(0f, 1f)] private float musicVolume = 0.4f;

    [Header("Scene")]
    [SerializeField] private string nextSceneName = "LevelSelect";

    [Header("UI References")]
    [SerializeField] private TMP_Text introText;
    [SerializeField] private TMP_Text hintText;
    [SerializeField] private Button skipButton;
    [SerializeField] private Slider progressBar;
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private int currentSlideIndex = 0;
    private bool isTypewriting = false;
    private Coroutine currentTypewriterCoroutine;
    private Coroutine autoAdvanceCoroutine;
    private AudioSource audioSource;

    private void Awake()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 1f;
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
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            AdvanceToNextSlide();
            Debug.Log("Pepe");

        }

        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SkipIntro();
        }
    }

    private IEnumerator PlayIntroSequence()
    {
        if (fadeCanvasGroup != null)
        {
            yield return StartCoroutine(FadeCanvas(1f, 0f, fadeDuration));
        }

        while (currentSlideIndex < slides.Length)
        {
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
            introText.alpha = 0f;
            introText.text = slides[index];
            currentTypewriterCoroutine = StartCoroutine(TypewriterEffect(introText));
            yield return StartCoroutine(FadeText(introText, 0f, 1f, fadeDuration));

        }
    }

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
        Debug.Log("Chorizo");

        currentSlideIndex++;

        if (currentSlideIndex >= slides.Length)
        {
            StartCoroutine(GoToNextScene());
        }
    }

    private void CompleteTypewriterImmediately()
    {
        Debug.Log("Queso");

        if (currentTypewriterCoroutine != null)
        {
            Debug.Log("Jamon");

            StopCoroutine(currentTypewriterCoroutine);
        }

        if (introText != null)
        {
            introText.maxVisibleCharacters = introText.textInfo.characterCount;
        }

        isTypewriting = false;
        ShowHint();
    }

    private void ShowHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(true);
        }
    }

    private void HideHint()
    {
        if (hintText != null)
        {
            hintText.gameObject.SetActive(false);
        }
    }

    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.maxValue = slides.Length;
            progressBar.value = currentSlideIndex;
        }
    }

    public void SkipIntro()
    {
        StopAllCoroutines();
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

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    private IEnumerator FadeCanvas(float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
    }

    private IEnumerator FadeText(TMP_Text text, float from, float to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            text.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }

        text.alpha = to;
    }

    private void OnDestroy()
    {
        if (skipButton != null)
        {
            skipButton.onClick.RemoveListener(SkipIntro);
        }
    }
}
