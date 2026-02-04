---
title: Backgrounds - Fondos por Escena y Fase
---

## Historia de Usuario

Como jugador, quiero entornos visuales tematicos que me transports al ambiente academico de la CUJAE mientras enfrento los parciales.

## Descripcion

Los fondos del juego representan diferentes entornos academicos donde se desarrollan los parciales. Cada fase del boss tiene su propio ambiente visual que refleja la intensidad creciente del examen.

## Elementos Requeridos

### Fondo Fase 1: Aula de Clases (Intro del Parcial)

#### Descripcion Visual

| Aspecto | Detalle |
|---------|---------|
| **Ambiente** | Aula universitaria estandar |
| **Elementos** | Pizarra, bancos, escritorios, ventanas |
| **Iluminacion** | Luz fluorescente, dia afuera |
| **Atmosfera** | Tranquila, antes del examen |

#### Componentes del Fondo

##### Plano Fondo (Furthest)

```
- Cielo visible por ventanas
- Edificios CUJAE en distancia
- Arboles/campus
- Nubes sutiles animate slowly
```

##### Plano Medio

```
- Pizarra gigante con "EXAMEN FINAL" escrito
- Carteles motivacionales academicos
- Mapa de Cuba/Universidad
- Reloj de pared (ticktock sutil)
```

##### Plano Cercano

```
- Primera fila de bancos vacios
- Sillas desordenadas
- Mesa del profesor con examenes
- Luz de proyector si aplica
```

#### Estado del Fondo

- **Idle:** Luz fluorescente flicker sutil, afuera se ve dia
- **Durante combate:** Luz se vuelve mas intensa/ansiosa
- **Transicion:** Pan zoom hacia pizarra

#### Color Palette

| Elemento | Color HEX | RGB |
|----------|-----------|-----|
| Paredes | #BDC3C7 | 189, 195, 199 |
| Pizarra | #27AE60 | 39, 174, 96 |
| Bancos | #8B4513 | 139, 69, 19 |
| Ventanas | #AED6F1 | 174, 214, 241 |
| Cielo | #5DADE2 | 93, 173, 226 |

---

### Fondo Fase 2: Biblioteca (Examen en Progreso)

#### Descripcion Visual

| Aspecto | Detalle |
|---------|---------|
| **Ambiente** | Biblioteca universitaria |
| **Elementos** | Estantes de libros, escaleras, mesas |
| **Iluminacion** | Luz calida de lamparas, mas intima |
| **Atmosfera** | Tensa, silenciosa, focused |

#### Componentes del Fondo

##### Plano Fondo (Furthest)

```
- Estantes infinitos de libros
- Secciones marcadas: Ciencias, Ingenierias, Humanidades
- Escalera de biblioteca estilo clasico
```

##### Plano Medio

```
- Mesas de estudio con libros abiertos
- Papelera con examenes arrugados
- Carteles de "Silencio por favor"
- Reloj marcando el tiempo
```

##### Plano Cercano

```
- Vista entre estantes
- Libros de texto flotando sutilmente
- Polvo en el aire (motas)
- Luz de ventana lateral
```

#### Estado del Fondo

- **Idle:** Polvo animate slow, paginas pasando sutil
- **Durante combate:** Libros empiezan a volar
- **Transicion:** Estantes se desordenan

#### Color Palette

| Elemento | Color HEX | RGB |
|----------|-----------|-----|
| Estantes | #5D4037 | 93, 64, 55 |
| Libros | #D35400 | 211, 84, 0 |
| Mesas | #F5B041 | 245, 176, 65 |
| Luz calida | #F8C471 | 248, 196, 113 |
| Sombras | #17202A | 23, 32, 42 |

---

### Fondo Fase 3: Laboratorio/Auditorio (Desesperacion)

#### Descripcion Visual

| Aspecto | Detalle |
|---------|---------|
| **Ambiente** | Laboratorio de computacion o Auditorio |
| **Elementos** | Computadoras, cables, proyector, escenario |
| **Iluminacion** | Luces rojas/emergencia, glitch effects |
| **Atmosfera** | Caotica, tecnologica, final |

#### Componentes del Fondo

##### Plano Fondo (Furthest)

```
- Si laboratorio: Monitores con codigo/error
- Si auditorio: Asientos vacios, escenario
- Luces de emergencia parpadeando
```

##### Plano Medio

```
- Computadoras mostrando: "COMPILATION ERROR"
- Cables desordenados por el suelo
- Proyectores mostrando preguntas del examen
- Contadores/timer gigante
```

##### Plano Cercano

```
- Escenario donde esta el boss
- Pool de proyecciones
- Pizarra digital con cuenta regresiva
```

#### Efectos Especiales

| Efecto | Descripcion |
|--------|-------------|
| Glitch | Artefactos visuales en fondos |
| Static | Ruido blanco en areas |
| Red alert | Luces rojas pulsando |
| Timer | Cuenta regresiva visible |
| Error | Screens BSOD/errores en monitors |

#### Color Palette

| Elemento | Color HEX | RGB |
|----------|-----------|-----|
| Luces emergencia | #C0392B | 192, 57, 43 |
| Monitores | #2E86C1 | 46, 134, 193 |
| Cables | #1C2833 | 28, 40, 51 |
| Error screens | #000000 | 0, 0, 0 |
| Proyeccion | #F4D03F | 244, 208, 63 |

---

### Fondos Adicionales

#### Pantalla de Titulo

| Elemento | Descripcion |
|----------|-------------|
| **Vista** | Entrada principal CUJAE de noche |
| **Elementos** | Logo universitario, faroles |
| **Animacion** | Luces titilando en faroles |

#### Pantalla Game Over

| Elemento | Descripcion |
|----------|-------------|
| **Vista** | Aula vacia despues del examen |
| **Elementos** | Examenes en el suelo, reloj roto |
| **Animacion** | Nada, silencio total |
| **Atmosfera** | Derrota, frustracion |

#### Pantalla Victoria

| Elemento | Descripcion |
|----------|-------------|
| **Vista** | Aula con luz del amanecer |
| **Elementos** | Examenes aprobados corregidos con check verde |
| **Animacion** | Confetti/particulas de celebracion |

---

### Especificaciones Tecnicas

#### Formato de Archivos

| Uso | Formato | Resolucion | Notas |
|-----|---------|------------|-------|
| Fondo principal | PNG con alpha | 1920x1080 | Sin alpha en areas covered |
| Elementos parallax | PNG con alpha | 1920x1080 | Separados por layers |
| UI backgrounds | PNG sin alpha | 1920x1080 | Tiled pattern |
| Thumbnail | JPG | 640x360 | Para pause/menus |

#### Nomenclatura

```
bg_title_cujae.png
bg_classroom_idle.png
bg_classroom_combat.png
bg_classroom_transition.png
bg_library_idle.png
bg_library_combat.png
bg_laboratory_idle.png
bg_laboratory_combat.png
bg_gameover.png
bg_victory.png
```

#### Capas Parallax

| Layer | Velocidad | Descripcion |
|-------|-----------|-------------|
| Background | 0.2x | Cielo, edificios lejanos |
| Mid-ground | 0.5x | Estantes, ventanas |
| Foreground | 1.0x | Bancos, objetos primeros |
