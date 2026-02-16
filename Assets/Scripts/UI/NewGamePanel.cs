using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class NewGamePanel : MonoBehaviour
{
    [Header("Slot Labels (asignar 3 elementos)")]
    [SerializeField] private TMP_Text[] slotLabels;

    [Header("Delete Buttons (asignar 3 elementos)")]
    [SerializeField] private GameObject[] deleteButtons;

    [Header("Gameplay Scene")]
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private const int MaxSlots = 3;
    private int selectedSlot = -1;

    private void OnEnable()
    {
        RefreshSlots();
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < MaxSlots; i++)
        {
            string data = PlayerPrefs.GetString($"SaveSlot_{i}", "");
            if (string.IsNullOrEmpty(data))
            {
                slotLabels[i].text = $"Slot {i + 1} - Vacío";
                if (deleteButtons != null && i < deleteButtons.Length)
                    deleteButtons[i].SetActive(false);
            }
            else
            {
                slotLabels[i].text = $"Slot {i + 1} - {data}";
                if (deleteButtons != null && i < deleteButtons.Length)
                    deleteButtons[i].SetActive(true);
            }
        }
    }

    public void OnSlotSelected(int slotIndex)
    {
        selectedSlot = slotIndex;
        string existing = PlayerPrefs.GetString($"SaveSlot_{slotIndex}", "");

        if (string.IsNullOrEmpty(existing))
        {
            StartNewGame(slotIndex);
        }
        else
        {
            LoadGame(slotIndex);
        }
    }

    public void OnDeleteSlot(int slotIndex)
    {
        PlayerPrefs.DeleteKey($"SaveSlot_{slotIndex}");
        PlayerPrefs.Save();
        RefreshSlots();
    }

    private void StartNewGame(int slotIndex)
    {
        PlayerPrefs.SetString($"SaveSlot_{slotIndex}", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm"));
        PlayerPrefs.SetInt("CurrentSlot", slotIndex);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameplaySceneName);
    }

    private void LoadGame(int slotIndex)
    {
        PlayerPrefs.SetInt("CurrentSlot", slotIndex);
        SceneManager.LoadScene(gameplaySceneName);
    }
}
