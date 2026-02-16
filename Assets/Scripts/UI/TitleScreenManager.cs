using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TitleScreenManager : MonoBehaviour
{
    private enum TitlePanel { Main, NewGame, Options, Credits }

    [Header("Panels")]
    [SerializeField] private GameObject mainPanel;
    [SerializeField] private GameObject newGamePanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject creditsPanel;

    [Header("First Selected Button per Panel")]
    [SerializeField] private GameObject mainFirstSelected;
    [SerializeField] private GameObject newGameFirstSelected;
    [SerializeField] private GameObject optionsFirstSelected;
    [SerializeField] private GameObject creditsFirstSelected;

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private TitlePanel currentPanel = TitlePanel.Main;

    private void Start()
    {
        Application.targetFrameRate = 60;
        ShowPanel(TitlePanel.Main);
    }

    private void Update()
    {
        var kb = Keyboard.current;
        if (kb == null) return;

        if (currentPanel == TitlePanel.Main)
        {
            if (kb.enterKey.wasPressedThisFrame) OnJugar();
            else if (kb.nKey.wasPressedThisFrame) OnNuevoJuego();
            else if (kb.cKey.wasPressedThisFrame) OnCreditos();
            else if (kb.oKey.wasPressedThisFrame) OnOpciones();
            else if (kb.escapeKey.wasPressedThisFrame) OnSalir();
        }
        else
        {
            if (kb.escapeKey.wasPressedThisFrame || kb.backspaceKey.wasPressedThisFrame)
                GoBackToMain();
        }
    }

    public void OnJugar()
    {
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void OnNuevoJuego()
    {
        ShowPanel(TitlePanel.NewGame);
    }

    public void OnCreditos()
    {
        ShowPanel(TitlePanel.Credits);
    }

    public void OnOpciones()
    {
        ShowPanel(TitlePanel.Options);
    }

    public void OnSalir()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void GoBackToMain()
    {
        ShowPanel(TitlePanel.Main);
    }

    private void SetPanelActive(GameObject panel, bool active)
    {
        if (panel != null) panel.SetActive(active);
    }

    private void ShowPanel(TitlePanel panel)
    {
        currentPanel = panel;

        SetPanelActive(mainPanel, panel == TitlePanel.Main);
        SetPanelActive(newGamePanel, panel == TitlePanel.NewGame);
        SetPanelActive(optionsPanel, panel == TitlePanel.Options);
        SetPanelActive(creditsPanel, panel == TitlePanel.Credits);

        GameObject firstSelected = panel switch
        {
            TitlePanel.Main => mainFirstSelected,
            TitlePanel.NewGame => newGameFirstSelected,
            TitlePanel.Options => optionsFirstSelected,
            TitlePanel.Credits => creditsFirstSelected,
            _ => mainFirstSelected
        };

        if (firstSelected != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(firstSelected);
        }
    }
}
