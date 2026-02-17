# InGameMenu - Guía de Configuración

Este sistema permite controlar el volumen de música y efectos de sonido durante el gameplay sin pausar completamente el juego.

## Archivos Creados

- `InGameMenu.cs` - Script principal del menú con controles de volumen
- `InGameMenuController.cs` - Controlador de input para el menú
- `Audio/GameAudioMixer.mixer` - AudioMixer para controlar volúmenes

## Configuración en Unity

### 1. Crear AudioMixer

1. Ve a **Assets > Create > Audio Mixer**
2. Nómbralo `GameAudioMixer`
3. Crea dos grupos:
   - **Music** (para música de fondo)
   - **SFX** (para efectos de sonido)

### 2. Configurar el Menú UI

Asume que ya tienes los elementos UI creados. Necesitas:

- **Canvas** con el menú
- **Slider** para volumen de música
- **Slider** para volumen de SFX
- **Button** para "Controles"
- **Button** para "Salir al Menú"
- **Button** para "Reanudar"
- **GameObject** para popup de controles
- **GameObject** para diálogo de confirmación de salida

### 3. Configurar el Script InGameMenu

1. Agrega el script `InGameMenu` a tu Canvas del menú
2. Asigna las referencias en el Inspector:
   - **Music Volume Slider**: Tu slider de música
   - **SFX Volume Slider**: Tu slider de SFX
   - **Audio Mixer**: El AudioMixer creado
   - **Music Volume Parameter**: "MusicVolume"
   - **SFX Volume Parameter**: "SFXVolume"
   - Asigna los demás botones y popups

### 4. Configurar Input System

1. Abre **Edit > Project Settings > Input System Package > Input Actions**
2. Crea acciones para:
   - **ToggleMenu** (Escape, P, Start button)
   - **IncreaseMusicVolume** / **DecreaseMusicVolume**
   - **IncreaseSFXVolume** / **DecreaseSFXVolume**

### 5. Configurar Audio Sources

1. Para música: Asigna el AudioSource al grupo **Music** del AudioMixer
2. Para SFX: Asigna los AudioSources al grupo **SFX** del AudioMixer

## Métodos Públicos Disponibles

### Control de Volumen
- `IncreaseMusicVolume()` - Sube volumen de música +10%
- `DecreaseMusicVolume()` - Baja volumen de música -10%
- `IncreaseSFXVolume()` - Sube volumen de SFX +10%
- `DecreaseSFXVolume()` - Baja volumen de SFX -10%

### Control del Menú
- `ToggleMenu()` - Muestra/oculta el menú
- `ResumeGame()` - Cierra el menú y reanuda
- `ConfirmExitToMenu()` - Sale al menú principal
- `CancelExitToMenu()` - Cancela salida

## Valores por Defecto

- **Música**: 70% (0.7f)
- **SFX**: 80% (0.8f)

Los valores se guardan automáticamente en PlayerPrefs.

## Notas Importantes

- El menú NO pausa el juego por defecto (Time.timeScale no se modifica)
- Los cambios de volumen se aplican inmediatamente
- Los valores se convierten de lineal (0-1) a dB para una atenuación natural
- El script incluye limpieza automática de listeners para evitar memory leaks

## Uso en Gamepad

Los métodos de Increase/Decrease permiten controlar el volumen con botones del gamepad sin necesidad de usar los sliders visuales.