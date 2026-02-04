---
title: Pantalla Nuevo Juego
assignee: @ui-team
---

## Historia de Usuario

Como jugador, quiero ver y seleccionar entre todas las partidas guardadas para poder continuar mi progreso o iniciar una nueva partida.

## Descripción

Pantalla de gestión de partidas guardadas que permite al jugador visualizar todas las saves existentes, ver información de progreso de cada una, y elegir si quiere continuar o iniciar una nueva partida.

## Elementos Requeridos

### Lista de Saves
- **Tipo:** Scrollable List / Grid
- **Comportamiento:** Muestra todas las partidas guardadas disponibles
- **Información mostrada por save:**
  - Nombre de la partida
  - Fecha y hora del último guardado
  - Nivel/Alcance del progreso
  - Tiempo total de juego
- **Interacción:** Click para seleccionar, Enter para cargar

### Botón "Nueva Partida"
- **Tipo:** Button
- **Comportamiento:** Inicia una nueva partida después de confirmar que no hay slot libre o seleccionando slot vacío
- **Confirmación:** Mostrar diálogo de confirmación si ya existen saves máximos

### Botón "Eliminar Partida"
- **Tipo:** Button
- **Comportamiento:** Borra la partida seleccionada (requiere confirmación)
- **Estado:** Deshabilitado si no hay save seleccionado

### Botón "Volver"
- **Tipo:** Button
- **Comportamiento:** Regresa a la Title Screen
- **Keyboard shortcut:** Esc

### Slots de Save
- **Cantidad máxima:** 3-5 slots (definir en configuración)
- **Estados:**
  - Vacío: Muestra "+" o "Nuevo"
  - Ocupado: Muestra thumbnail del estado del juego
  - Corrupto: Mostrar indicador de error

## Comportamiento General

### Orden de Saves
- Por defecto: Ordenado por fecha de último guardado (más reciente primero)
- Opción de ordenar por: Fecha, Nivel, Tiempo de juego

### Thumbnails
- Cada save debe tener un screenshot del momento del guardado
- Actualizar thumbnail en cada auto-save

## Requisitos Técnicos

- Frame rate objetivo: 60 FPS
- Carga de saves: < 1 segundo
- Thumbnail resolution: 320x180
- Formato de metadata JSON embebido en archivo .save
