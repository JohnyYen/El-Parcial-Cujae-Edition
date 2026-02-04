---
title: Menú Opciones (Title Screen)
assignee: @ui-team
---

## Historia de Usuario

Como jugador, quiero configurar las opciones del juego para personalizar mi experiencia de audio, video y controles según mis preferencias y hardware.

## Descripción

Pantalla de configuración global del juego accesible desde la Title Screen. Permite ajustar parámetros de audio, video y controles que se guardan en persistencia.

## Elementos Requeridos

### Sección Volumen

#### Slider SFX
- **Tipo:** Slider (0-100)
- **Comportamiento:** Ajusta el volumen de efectos de sonido
- **Valor por defecto:** 80
- **Preview:** Reproducir sonido de prueba al cambiar
- **Persistence:** Guardar en PlayerPrefs/Settings

#### Slider Audio / Música
- **Tipo:** Slider (0-100)
- **Comportamiento:** Ajusta el volumen de música y audio ambiente
- **Valor por defecto:** 70
- **Preview:** Cambiar música de fondo a volumen objetivo
- **Persistence:** Guardar en PlayerPrefs/Settings

### Sección Resolución

#### Dropdown Resolución
- **Tipo:** Dropdown
- **Comportamiento:** Lista de resoluciones soportadas por el monitor
- **Resoluciones típicas:** 1280x720, 1920x1080, 2560x1440, 3840x2160
- **Valor por defecto:** Native resolution del monitor
- **Filtering:** Solo mostrar resoluciones >= 1280x720

#### Toggle Fullscreen
- **Tipo:** Toggle
- **Comportamiento:** Alterna entre modo ventana y pantalla completa
- **Valor por defecto:** Enabled (fullscreen)
- **Requier reinicio:** No (aplicar inmediatamente si es posible)

### Sección Controles

#### Mostrar Mapa de Controles
- **Tipo:** Button
- **Comportamiento:** Abre popup/modal con lista de controles actuales
- **Contenido:**
  - Mover: WASD / Flechas
  - Atacar: Espacio / Joystick
  - Defender: Shift
  - Menú: Escape
  - Saltar: W / Up

#### Rebind Controles
- **Tipo:** Button
- **Comportamiento:** Abre pantalla de reasignación de teclas
- **Interacción:** Click en acción → Presionar nueva tecla → Confirmar

### Botones de Acción

#### Botón "Aplicar"
- **Tipo:** Button
- **Comportamiento:** Aplica cambios realizados
- **Confirmation:** No requerida

#### Botón "Restaurar por Defecto"
- **Tipo:** Button
- **Comportamiento:** Resetea todas las opciones a valores por defecto
- **Confirmation:** Mostrar diálogo de confirmación

#### Botón "Volver"
- **Tipo:** Button
- **Comportamiento:** Regresa a Title Screen (aplicando cambios si no se rechazaron)
- **Keyboard shortcut:** Esc

## Comportamiento General

### Validación
- No permitir valores inválidos en sliders
- Resoluciones fuera de rango deben deshabilitarse

### Persistencia
- Guardar cambios automáticamente al salir de la pantalla
- Cargar valores guardados al entrar

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS
- Audio mixer unity para control de volúmenes
- Resolución de iconos de controles: 64x64
