---
title: Pantalla Créditos
assignee: @content-team
---

## Historia de Usuario

Como jugador, quiero ver los créditos del juego para reconocer al equipo de desarrollo y conocer las fuentes de assets utilizados.

## Descripción

Pantalla informativa que muestra el equipo de desarrollo, colaboradores, y atribuciones de assets externos. Puede ser navegable o con scroll automático.

## Elementos Requeridos

### Encabezado "Créditos"
- **Tipo:** Text / Header
- **Estilo:** Grande, destacado, alineado al centro
- **Animación:** Fade in al entrar

### Sección Equipo de Desarrollo
- **Tipo:** Lista o Grid
- **Contenido por miembro:**
  - Rol/Nombre del contributor
  - Área de contribución (Programación, Arte, Audio, Diseño)
  - Links opcionales (GitHub, Portfolio)

#### Roles típicos:
- Director de Proyecto
- Lead Programmer
- Game Designer
- Artist 2D / 3D
- Sound Designer / Composer
- QA Lead

### Sección Assets Externos
- **Tipo:** Lista con atribución
- **Contenido:**
  - Nombre del asset
  - Licencia (MIT, CC0, etc.)
  - Link al asset original
  - Sección "Made with" si aplica (Unity, Godot, Blender, etc.)

### Sección Agradecimientos
- **Tipo:** Text block
- **Contenido:** Beta testers, inspiradores, recursos educativos

### Botón "Volver"
- **Tipo:** Button
- **Comportamiento:** Regresa a Title Screen
- **Posición:** Esquina inferior derecha o izquierda
- **Keyboard shortcut:** Esc, Back button

## Comportamiento General

### Navegación
- Scroll vertical para ver todo el contenido
- Mouse wheel y touch scroll habilitados
- Gamepad: Stick izquierdo para scroll

### Animaciones
- Entrada progresiva de cada sección
- Transición suave al salir

### Modo de presentación
- Scroll manual: Usuario controla el scroll
- Auto-scroll: Opción para scroll automático con velocidad ajustable

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS
- Responsive design para diferentes resoluciones
- Links clickables en versión desktop
