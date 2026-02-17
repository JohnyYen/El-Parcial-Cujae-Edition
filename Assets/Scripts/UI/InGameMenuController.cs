using UnityEngine;
using UnityEngine.InputSystem;

public class InGameMenuController : MonoBehaviour
{
    [SerializeField] private InGameMenu inGameMenu;

    private void Awake()
    {
        if (inGameMenu == null)
        {
            inGameMenu = FindObjectOfType<InGameMenu>();
        }
    }

    // Método llamado desde Input System (asigna en Input Actions)
    public void OnToggleMenu(InputAction.CallbackContext context)
    {
        if (context.performed && inGameMenu != null)
        {
            inGameMenu.ToggleMenu();
        }
    }

    // Métodos para botones de subir/bajar volumen (útil para gamepad)
    public void OnIncreaseMusicVolume(InputAction.CallbackContext context)
    {
        if (context.performed && inGameMenu != null)
        {
            inGameMenu.IncreaseMusicVolume();
        }
    }

    public void OnDecreaseMusicVolume(InputAction.CallbackContext context)
    {
        if (context.performed && inGameMenu != null)
        {
            inGameMenu.DecreaseMusicVolume();
        }
    }

    public void OnIncreaseSFXVolume(InputAction.CallbackContext context)
    {
        if (context.performed && inGameMenu != null)
        {
            inGameMenu.IncreaseSFXVolume();
        }
    }

    public void OnDecreaseSFXVolume(InputAction.CallbackContext context)
    {
        if (context.performed && inGameMenu != null)
        {
            inGameMenu.DecreaseSFXVolume();
        }
    }
}