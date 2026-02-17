# Configuración de Input Actions para InGameMenu

Para configurar los controles del menú, sigue estos pasos:

## 1. Abrir Input Actions

1. Ve a **Edit > Project Settings > Input System Package > Input Actions**
2. Abre tu archivo `.inputactions` (probablemente `InputSystem_Actions.inputactions`)

## 2. Crear Action Map

Crea un nuevo **Action Map** llamado `InGameMenu` (o usa uno existente).

## 3. Crear Actions

### Toggle Menu
- **Action Name**: `ToggleMenu`
- **Action Type**: Button
- **Bindings**:
  - Keyboard: `Escape`
  - Keyboard: `P`
  - Gamepad: `Start` (Button South)

### Volumen Música
- **Action Name**: `IncreaseMusicVolume`
- **Action Type**: Button
- **Bindings**: Gamepad D-Pad Up

- **Action Name**: `DecreaseMusicVolume`
- **Action Type**: Button
- **Bindings**: Gamepad D-Pad Down

### Volumen SFX
- **Action Name**: `IncreaseSFXVolume`
- **Action Type**: Button
- **Bindings**: Gamepad D-Pad Right

- **Action Name**: `DecreaseSFXVolume`
- **Action Type**: Button
- **Bindings**: Gamepad D-Pad Left

## 4. Configurar el Controller

1. Agrega el script `InGameMenuController` a un GameObject en tu escena
2. En el Inspector, asigna la referencia al `InGameMenu`
3. En el componente **Player Input**, selecciona:
   - **Actions**: Tu Input Actions asset
   - **Default Map**: El Action Map que contiene las acciones del menú

## 5. Eventos de Input

Los métodos del `InGameMenuController` se llamarán automáticamente cuando se activen las acciones configuradas.

## Notas

- Asegúrate de que el `InGameMenuController` tenga un componente `PlayerInput`
- Las acciones se activan solo cuando `context.performed` es true
- Puedes agregar más bindings según necesites (teclado numérico, etc.)