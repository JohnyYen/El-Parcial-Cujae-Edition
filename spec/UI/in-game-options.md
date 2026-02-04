---
title: Menú Opciones Durante el Juego
assignee: yanni
---

## Historia de Usuario

Como jugador, quiero acceder a un menú de opciones reducido durante la partida para ajustar volumen sin pausar completamente mi experiencia de juego.

## Descripción

Menú de opciones accesible desde el HUD durante el gameplay. Es una versión simplificada del menú de opciones principal, enfocada en ajustes rápidos que el jugador puede necesitar sin interrumpir completamente el flujo del juego.

## Elementos Requeridos

### Slider SFX
- **Tipo:** Slider (0-100)
- **Comportamiento:** Ajusta el volumen de efectos de sonido en tiempo real
- **Valor por defecto:** 80 (heredado de opciones globales)
- **Preview:** Reproducir sound effect de prueba
- **Comportamiento en juego:** Change aplica inmediatamente

### Slider Audio / Música
- **Tipo:** Slider (0-100)
- **Comportamiento:** Ajusta el volumen de música ambiente
- **Valor por defecto:** 70 (heredado de opciones globales)
- **Preview:** Cambiar volumen de música actual
- **Comportamiento en juego:** Change aplica inmediatamente

### Botón "Controles"
- **Tipo:** Button
- **Comportamiento:** Abre popup modal con mapa de controles
- **Contenido:** Lista de controles actuales (igual que opciones globales)
- **Cierre:** Click fuera del modal, Escape, o botón cerrar

### Botón "Salir al Menú"
- **Tipo:** Button (danger style)
- **Comportamiento:** Regresa a Title Screen
- **Confirmación:** Mostrar diálogo: "¿Estás seguro de que quieres salir al menú? Se guardará tu progreso."

### Botón "Reanudar"
- **Tipo:** Button
- **Comportamiento:** Cierra el menú y reanuda el juego
- **Keyboard shortcut:** Esc, Start button, P

## Comportamiento General

### Estado del Juego
- El juego NO se pausa al abrir este menú
- El jugador puede seguir moviéndose/enemigos atacando mientras ajusta opciones
- Alternativa: Pausar el juego (definir en meeting de diseño)

### Acceso
- Accessible desde HUD mediante botón de opciones
- Keyboard shortcut: Escape, P, Start
- Gamepad: Start button

### Visuales
- Semi-transparente (overlay) sobre el gameplay
- No bloquear completamente la vista del juego
- Estilo consistente con el menú de opciones principal

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS (menú no debe impactar rendimiento)
- Audio mixer con snapshots para transiciones suaves
- Guardado automático de cambios en opciones
