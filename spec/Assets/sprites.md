---
title: Sprites - Personajes, Enemigos y Proyectiles
---

## Historia de Usuario

Como jugador, quiero ver personajes, enemigos y proyectiles con temática de exámenes CUJAE para sumergirme en la experiencia académica del juego.

## Descripción

Este documento define los sprites del juego con temática de parciales CUJAE. Los enemigos son exámenes literales, los proyectiles varían según la materia del parcial, y el player es un estudiante de la CUJAE.

## Elementos Requeridos

### Player Principal: Estudiante CUJAE

#### Características de Diseño

- **Apariencia:** Joven universitario con rasgos latinoamericanos
- **Vestimenta:** Bata de laboratorio o ropa casual estudiantil
- **Accesorios:** Mochila/backpack, possibly calculadora científica visible
- **Expresiones:** Determinado, enfocado, frustración al recibir daño

#### Estados de Animación

| Estado | Descripción | Frames estimados |
|--------|-------------|------------------|
| Idle | De pie, respiración sutil | 4-8 |
| Walk | Caminata normal | 8-12 |
| Run | Corriendo hacia acción | 8-12 |
| Jump | Salto hacia arriba | 6-8 |
| Fall | Cayendo | 4-6 |
| Dash | Stretch motion + velocidad | 6-8 |
| Light Attack 1 | Golpe rápido #1 | 8-10 |
| Light Attack 2 | Golpe rápido #2 | 8-10 |
| Light Attack 3 | Golpe rápido #3 (fin) | 10-12 |
| Heavy Attack | Ataque cargado | 12-16 |
| Hit | Reacción a daño | 4-6 |
| Death | Caída final | 8-12 |
| Focus Charge | Cargando enfoque | 6-8 |

#### Paleta de Colores

- **Skin:** Tono carne cálido (latino)
- **Ropa principal:** Verde o blanco (colors universitarios)
- **Acentos:** Amarillo/dorado (detalles)
- **Bata:** Blanco si es científico

---

### Enemigos: Exámenes Literales

#### Tipos de Exámenes

##### 1. Examen Escrito (Tipo Paper)

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Hoja de papel A4 arrugada con texto |
| **Dimensiones** | 1x1.5 units (humanoid) |
| **Cara** | Expresión stressed/ansiosa |
| **Extremidades** | Brazos de tinta/mano tulisan |
| **Movimiento** | Flota, se desliza |
| **Sonido** | Papel arrugado al moverse |

**Sprites requeridos:**
- Idle (paper flotando)
- Attack: Lanzar preguntas escrita
- Hit (paper dañado)
- Death (paper quemándose)

##### 2. Examen Multiple Choice (Tipo Test)

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Cuadrado/rectángulo tipo sheet de respuestas |
| **Cara** | Opciones A, B, C, D como ojos |
| **Características** | Marcas de check/X por todo el cuerpo |
| **Movimiento** | Rebota erraticamente |
| **Ataque** | Lanza burbujas con opciones |

**Sprites requeridos:**
- Idle (flotando con opciones visibles)
- Attack: Burbuja con A/B/C/D
- Transformación: Si player acierta, cambia a check

##### 3. Examen Oral (Tipo Speak)

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Cabeza flotante con boca gigante |
| **Cara** | Boca enorme hablando |
| **Expresión** | Profesor estricto |
| **Movimiento** | Se acerca al player, cara grande |
| **Ataque** | Ondas de voz/preguntas |

**Sprites requeridos:**
- Idle (boca moviéndose)
- Attack: Onda de sonido
- Hit (voz entrecortada)

##### 4. Examen Práctico/Laboratorio

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Mesa de laboratorio con instrumentos |
| **Cara** | Tubos de ensayo como ojos |
| **Accesorios** | Probetas, matraces, bunsen |
| **Movimiento** | Se arrastra con patas de support |
| **Ataque** | Lanza sustancias químicas |

**Sprites requeridos:**
- Idle (preparando experimento)
- Attack: Proyectil de líquido
- Reacción: Cambio de color según sustancia

##### 5. Parcial de Matemáticas (Boss Minion)

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Pizarra flotante con ecuaciones |
| **Dimensiones** | 2x2 units (más grande) |
| **Cara** | Símbolos matemáticos como expresión |
| **Características** | Números flotando alrededor |
| **Movimiento** | Teleport short distances |

**Sprites requeridos:**
- Idle (ecuaciones rotando)
- Attack: Lanzar números como proyectiles
- Phase Change: Nueva ecuación aparece

---

### Proyectiles (Por Materia)

#### Parcial de Cálculo

| Proyectil | Visual | Efecto |
|-----------|--------|--------|
| Integral | ∫ flotante con valor | Daño área si no esquiva |
| Derivada | d/dx symbol | Reduces velocidad del player |
| Límite | lim(x→∞) | Traba movimiento brevemente |
| Número Pi | π gigante flotando | Daño constante en área |
| Raíz Cuadrada | √(número) | Daño moderado, rápido |
| Seno/Coseno | Onda sinusoidal | Trayectoria curva difícil |

#### Parcial de Física

| Proyectil | Visual | Efecto |
|-----------|--------|--------|
| Gravedad | g ↓ symbol | Player cae más rápido |
| Fuerza | F = m*a | Empuja al player hacia atrás |
| Momentum | p = mv | Daño por colisión propia |
| Velocidad | v = Δx/Δt | Proyectil muy rápido |
| Energía | E = mc² | Explosión en área |

#### Parcial de Química

| Proyectil | Visual | Efecto |
|-----------|--------|--------|
| Reacción | H2O + ... → | Daño por área + slow |
| Tabla Periódica | Elemento flotante | Debuff específico |
| Ácido | Gota verde brillante | Daño continuo (DoT) |
| Neutralizar | pH 7 | Cura al boss, daña player |

#### Parcial de Historia

| Proyectil | Visual | Efecto |
|-----------|--------|--------|
| Fecha | Año flotante (1492) | Stun por confusión |
| Línea Temporal | Línea con eventos | Player ve flashbacks |
| Documento | Pergamino antiguo | Daño + slow por peso |

---

### Minions (Por Nivel)

#### Minion Facil - "El Quiz"

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Papel pequeño, 3-4 preguntas |
| **Dimensiones** | 0.5x0.5 units |
| **Comportamiento** | Flota lento hacia el player |
| **Ataque** | Una pregunta simple |
| **Salud** | 1 hit para destruir |

#### Minion Medio - "El Taller"

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Pack de hojas con problemas |
| **Dimensiones** | 0.8x0.8 units |
| **Comportamiento** | Camino directo con slight evasion |
| **Ataque** | Serie de 2-3 preguntas |
| **Salud** | 2-3 hits |

#### Minion Dificil - "El Proyecto"

| Aspecto | Descripción |
|---------|-------------|
| **Forma** | Folder gigante con muchos docs |
| **Dimensiones** | 1.2x1.2 units |
| **Comportamiento** | Persigue activamente, flanks |
| **Ataque** | Presentación completa + defensa |
| **Salud** | 4-5 hits |

---

### Especificaciones Técnicas

#### Formato de Archivos

| Category | Formato | Resolución Base | Profundidad Color |
|----------|---------|-----------------|-------------------|
| Player | PNG con alpha | 64x64 px | 32-bit |
| Enemigos | PNG con alpha | 48x48 px (base) | 32-bit |
| Proyectiles | PNG con alpha | 16x16 - 32x32 px | 32-bit |
| UI Icons | PNG sin alpha | 32x32 px | 8-bit (indexed) |

#### Nomenclatura

```
// Player
player_idle_01.png
player_idle_02.png
...
player_walk_01.png
player_run_01.png
player_jump_start.png
player_jump_fall.png
player_light_atk_01.png
player_heavy_atk_charge.png
player_heavy_atk_release.png
player_hit.png
player_death.png

// Enemigos (Exámenes)
exam_paper_idle.png
exam_paper_attack.png
exam_paper_hit.png
exam_paper_death.png
exam_multchoice_idle.png
exam_oral_idle.png
exam_lab_idle.png

// Proyectiles
proj_integral.png
proj_derivative.png
proj_gravity.png
proj_acid.png
```

#### Spritesheets

- **Player:** 512x512 px max por sheet
- **Enemigos:** 256x256 px max por sheet
- **Organización:** Por personaje, no por tipo de frame

---

### Paleta de Colores General

| Uso | Color HEX | RGB |
|-----|-----------|-----|
| Player principal | #3498DB | 52, 152, 219 |
| Examen papel | #ECF0F1 | 236, 240, 241 |
| Examen multiple choice | #F1C40F | 241, 196, 15 |
| Examen oral | #E74C3C | 231, 76, 60 |
| Examen laboratorio | #2ECC71 | 46, 204, 113 |
| Proyectil cálculo | #9B59B6 | 155, 89, 182 |
| Proyectil física | #E67E22 | 230, 126, 34 |
| Proyectil química | #1ABC9C | 26, 188, 156 |
| UI/texto | #2C3E50 | 44, 62, 80 |
