---
title: Title Screen
assignee: @ui-team
---

## Historia de Usuario

Como jugador, quiero ver la pantalla de inicio del juego con las opciones principales para poder navegar hacia la acción o la información del juego.

## Descripción

Pantalla inicial del juego que se muestra al ejecutar la aplicación. Es el primer punto de contacto del jugador con la experiencia de juego y debe transmitir la identidad visual del proyecto.

## Elementos Requeridos

### Botón "Jugar"
- **Tipo:** Button
- **Comportamiento:** Inicia el juego cargando la última partida guardada (si existe) o dirige a la pantalla de selección de saved games
- **Estado inicial:** Habilitado
- **Keyboard shortcut:** Enter

### Botón "Nuevo Juego"
- **Tipo:** Button
- **Comportamiento:** Inicia una nueva partida,-reseteando todo el progreso y بدء estado inicial del juego
- **Estado inicial:** Habilitado
- **Keyboard shortcut:** N

### Botón "Créditos"
- **Tipo:** Button
- **Comportamiento:** Navega a la pantalla de créditos con información del equipo de desarrollo
- **Estado inicial:** Habilitado
- **Keyboard shortcut:** C

### Botón "Opciones"
- **Tipo:** Button
- **Comportamiento:** Navega al menú de opciones para ajustar volumen, resolución y controles
- **Estado inicial:** Habilitado
- **Keyboard shortcut:** O

### Botón "Salir"
- **Tipo:** Button
- **Comportamiento:** Cierra la aplicación de forma segura
- **Estado inicial:** Habilitado
- **Keyboard shortcut:** Esc

## Comportamiento General

### Navegación
- Los botones deben ser navegables con gamepad, teclado y mouse
- Focus visual debe estar siempre en el elemento seleccionado
- Navegación circular (arriba/abajo)

### Animaciones
- Transición suave entre botones (highlight)
- Efecto de hover en cada botón
- Sonido de UI al navegar y seleccionar

## Requisitos Técnicos

- Resolución base: 1920x1080 (escalable)
- Frame rate objetivo: 60 FPS
- Assets requeridos: Background, Logo del juego, 5 botones con estados (normal, hover, pressed, disabled)
