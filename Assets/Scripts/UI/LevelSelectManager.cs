using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using TMPro;
using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public string sceneName;
}

public class LevelSelectManager : MonoBehaviour
{
    [Header("Niveles (configurable en Inspector)")]
    [SerializeField] private LevelData[] levels = new LevelData[]
    {
        new LevelData { levelName = "Cálculo I", sceneName = "Gameplay_Calculo" },
        new LevelData { levelName = "Álgebra", sceneName = "Gameplay_Algebra" },
        new LevelData { levelName = "Física I", sceneName = "Gameplay_Fisica" },
        new LevelData { levelName = "Programación", sceneName = "Gameplay_Programacion" }
    };

    [Header("UI References")]
    [SerializeField] private GameObject levelButtonPrefab;
    [SerializeField] private Transform buttonsContainer;
    [SerializeField] private Button backButton;
    [SerializeField] private TMP_Text headerText;

    [Header("Scene Names")]
    [SerializeField] private string titleScreenSceneName = "TitleScreen";

    private List<GameObject> spawnedButtons = new List<GameObject>();
    private List<Button> buttonComponents = new List<Button>();
    private int currentIndex = 0;

    private void Start()
    {
        GenerateLevelButtons();
        SetupBackButton();

        if (spawnedButtons.Count > 0)
        {
            SelectButton(0);
        }
    }

    private void Update()
    {
        HandleKeyboardNavigation();
    }

    private void GenerateLevelButtons()
    {
        ClearExistingButtons();

        for (int i = 0; i < levels.Length; i++)
        {
            GameObject buttonObj = Instantiate(levelButtonPrefab, buttonsContainer);
            spawnedButtons.Add(buttonObj);

            LevelData level = levels[i];
            int index = i;

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
            {
                buttonText.text = level.levelName;
            }

            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                buttonComponents.Add(button);
                button.onClick.AddListener(() => OnLevelSelected(index));
            }
        }
    }

    private void ClearExistingButtons()
    {
        foreach (GameObject btn in spawnedButtons)
        {
            if (btn != null)
            {
                Destroy(btn);
            }
        }
        spawnedButtons.Clear();
        buttonComponents.Clear();
    }

    private void SetupBackButton()
    {
        if (backButton != null)
        {
            backButton.onClick.AddListener(GoBackToTitle);
        }
    }

    private void HandleKeyboardNavigation()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.upArrowKey.wasPressedThisFrame)
        {
            NavigateUp();
        }
        else if (kb.downArrowKey.wasPressedThisFrame)
        {
            NavigateDown();
        }
        else if (kb.enterKey.wasPressedThisFrame || kb.spaceKey.wasPressedThisFrame)
        {
            ConfirmSelection();
        }
        else if (kb.escapeKey.wasPressedThisFrame)
        {
            GoBackToTitle();
        }
    }

    private void NavigateUp()
    {
        if (buttonComponents.Count == 0) return;

        int newIndex = currentIndex - 1;
        if (newIndex < 0)
        {
            newIndex = buttonComponents.Count - 1;
        }

        SelectButton(newIndex);
    }

    private void NavigateDown()
    {
        if (buttonComponents.Count == 0) return;

        int newIndex = currentIndex + 1;
        if (newIndex >= buttonComponents.Count)
        {
            newIndex = 0;
        }

        SelectButton(newIndex);
    }

    private void SelectButton(int index)
    {
        if (index < 0 || index >= buttonComponents.Count) return;

        currentIndex = index;

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(spawnedButtons[index]);
    }

    private void ConfirmSelection()
    {
        if (currentIndex >= 0 && currentIndex < buttonComponents.Count)
        {
            OnLevelSelected(currentIndex);
        }
    }

    public void OnLevelSelected(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex >= levels.Length)
        {
            Debug.LogError($"Índice de nivel inválido: {levelIndex}");
            return;
        }

        LevelData selectedLevel = levels[levelIndex];
        Debug.Log($"Nivel seleccionado: {selectedLevel.levelName} → Escena: {selectedLevel.sceneName}");

        if (!string.IsNullOrEmpty(selectedLevel.sceneName))
        {
            // Usar SceneFader para transición
            SceneFader fader = FindFirstObjectByType<SceneFader>();
            if (fader != null)
            {
                fader.FadeTransition(selectedLevel.sceneName, 0.5f, 0.5f);
            }
            else
            {
                // Fallback si no hay fader
                SceneManager.LoadScene(selectedLevel.sceneName);
            }
        }
        else
        {
            Debug.LogError($"El nivel '{selectedLevel.levelName}' no tiene escena asignada.");
        }
    }

    public void GoBackToTitle()
    {
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

    private void OnDestroy()
    {
        if (backButton != null)
        {
            backButton.onClick.RemoveListener(GoBackToTitle);
        }
    }
}
