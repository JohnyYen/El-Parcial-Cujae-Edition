---
title: Game Over Screen
assignee: fernando
---

## Historia de Usuario

Como jugador, quiero recibir feedback claro cuando pierdo para entender mi estado actual y decidir si quiero reintentar o volver al menú.

## Descripción

Pantalla que se muestra cuando el jugador pierde todas sus vidas/estrés. Muestra un mensaje simple indicando el estado del jugador ("Tienes 2") y ofrece opciones para continuar o salir.

## Elementos Requeridos

### Mensaje Principal
- **Tipo:** Text
- **Contenido:** "Tienes 2"
- **Estilo:** Grande, centrado, visible inmediatamente
- **Color:** Blanco sobre fondo oscuro o contraste alto
- **Significado:** Indica número de vidas/continues restantes

### Botón "Reintentar"
- **Tipo:** Button
- **Comportamiento:** Reinicia la partida desde el último checkpoint o inicio del nivel
- **Keyboard shortcut:** Enter, R

### Botón "Menú Principal"
- **Tipo:** Button
- **Comportamiento:** Regresa a Title Screen (guardando progreso si aplica)
- **Keyboard shortcut:** Esc, M

### Botón "Salir del Juego"
- **Tipo:** Button (opcional)
- **Comportamiento:** Cierra la aplicación
- **Keyboard shortcut:** Q, Alt+F4

## Comportamiento General

### Estado de Vidas
- "Tienes 2" significa: 2 vidas/continues restantes
- Si tiene 0: No mostrar opción de reintentar, solo menú o salir
- El número debe ser dinámico basado en variable de estado

### Audio
- Sonido de game over al entrar
- Música de fondo opcional (triste/melancólica)
- Sonido de UI al navegar botones

### Animaciones
- Fade in del mensaje principal
- Entrada secuencial de botones
- Efecto de shake en mensaje si se pierde última vida

## Notas de Diseño

### Significado de "Tienes 2"
- Interpretación actual: El jugador tiene 2 oportunidades adicionales de continuar
- Alternativa a definir: ¿Sistema de vidas tradicional (3 vidas) o sistema de continues?
- **Decision pendiente con game designer**

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS
- Comunicación con Game Manager para obtener número de vidas
- Guardado automático del estado antes de mostrar pantalla
