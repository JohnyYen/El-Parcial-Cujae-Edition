---
title: Audio - Musica y Efectos de Sonido
---

## Historia de Usuario

Como jugador, quiero una experiencia auditiva inmersiva que refleje la tension y emocion de enfrentar parciales en la CUJAE, con musica que evoluciona y sonidos que confirman mis acciones.

## Descripcion

Este documento define toda la musica y efectos de sonido del juego. La musica cambia segun la fase del boss para aumentar la tension, y los SFX proporcionan feedback claro para cada accion del player y los enemigos.

## Elementos Requeridos

### Musica por Fase

#### Musica Fase 1: Aula de Clases (Intro del Parcial)

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Ambient acustico, tension suave |
| **Tempo** | 70-80 BPM |
| **Instrumentos** | Piano suave, cuerdas discretas |
| **Estado emocional** | Anticipacion, calma antes de la tormenta |
| **Duracion** | Loop infinito (2-4 minutos) |

##### Capas de la Musica

| Layer | Descripcion | Cuando suena |
|-------|-------------|--------------|
| Base | Piano arpeggio suave | Siempre |
| Armonia | Cuerdas en pianissimo | Durante combate |
| Tension | Percusion leve añadida | Boss activo |
| Climax | Brass stinger | Fase transition |

##### Estructura Musical

```
[Intro 16 bars] Piano solo
[Verse 16 bars] Piano + cuerdas sutiles
[Build 8 bars] Tension incremental
[Loop] Verso completo + build
[Transition] Cuerdas suben -> fade a silencio
```

##### Ejemplos de Referencia

- Persona 5: Track "Beneath the Mask" (version calma)
- Undertale: Track "Home"
- Celeste: Track "Resurrections"

---

#### Musica Fase 2: Biblioteca (Examen en Progreso)

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Electronica sutil, tension electronica |
| **Tempo** | 100-110 BPM |
| **Instrumentos** | Sintetizadores, piano electrico |
| **Estado emocional** | Focus, presion, concentracion extrema |
| **Duracion** | Loop infinito (3-5 minutos) |

##### Capas de la Musica

| Layer | Descripcion | Cuando suena |
|-------|-------------|--------------|
| Base | Sintetizador drone constante | Siempre |
| Ritmo | Beat electronico suave | Durante combate |
| Melodia | Piano electrico | Boss ataca |
| Tension | Arpegios rapidos | Salud boss < 50% |

##### Estructura Musical

```
[Drone 8 bars] Sintetizador bajo
[Beat entra] Ritmo suave
[Melodia 16 bars] Piano pattern
[Build 8 bars] Tempo acelera
[Loop] Seccion completa
[Transition] Glitch sound -> silencio
```

##### Ejemplos de Referencia

- Undertale: Track "Megalovania"
- Cuphead: Track "Threatenin' Zeppelin"
- Hades: Track "The House of Hades"

---

#### Musica Fase 3: Laboratorio (Desesperacion)

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Orchestral cinematico, accion maxima |
| **Tempo** | 130-150 BPM |
| **Instrumentos** | Orchestra completa, coros, percusion |
| **Estado emocional** | Desperacion, ultima chance, heroic journey |
| **Duracion** | Loop infinito (4-6 minutos) |

##### Capas de la Musica

| Layer | Descripcion | Cuando suena |
|-------|-------------|--------------|
| Base | Cuerdas ostinato | Siempre |
| Brass | Metales heroicos | Ataques boss |
| Percusion | Timpani, tambores | Batalla activa |
| Coro | Voces etereas | Salud boss < 25% |
| Full orchestra | Todo junto | Climax de combate |

##### Estructura Musical

```
[Intro 8 bars] Cuerdas tensas
[Explosion 8 bars] Orchestra completa entra
[Verse 16 bars] Ritmo completo
[Build 16 bars] Crescendo gradual
[Climax 8 bars] Todo el poder
[Loop] Verse completo + build + climax
[Transition] Big finish -> silencio
```

##### Ejemplos de Referencia

- Persona 5: Track "Rivers in the Desert"
- Nier: Automata: Track "Beneath the Mask" (version final)
- Hollow Knight: Track "Hornet"

---

### Musica Adicional

#### Pantalla de Titulo

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Melodia memorable, inspiradora |
| **Tempo** | 90-100 BPM |
| **Instrumentos** | Guitarra acustica, piano, cuerdas |
| **Caracter** | hook pegadizo, memorable |
| **Duracion** | Loop (1-2 minutos) |

#### Pantalla Game Over

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Melancolico, decepcionado |
| **Tempo** | 50-60 BPM |
| **Instrumentos** | Piano solo, cuerdas distantes |
| **Caracter** | Triste pero no dramatica |
| **Duracion** | 30 segundos -> fade out |

#### Pantalla Victoria

| Aspecto | Detalle |
|---------|---------|
| **Genero** | Celebratorio, triunfante |
| **Tempo** | 120-130 BPM |
| **Instrumentos** | Orchestra completa, campanas |
| **Caracter** | Victory fanfare |
| **Duracion** | 1-2 minutos + fade |

---

### Efectos de Sonido (SFX) - Player

#### Movimiento

| Accion | SFX | Descripcion | Volumen |
|--------|-----|-------------|---------|
| Walk | Pasos en suelo duro | 2-3 variations | -20dB |
| Run | Pasos rapidos | 3-4 variations | -18dB |
| Jump | Salto con aire | 1 variation | -22dB |
| Land | Aterrizaje | 2 variations | -16dB |
| Dash | Whoosh rapido | 1 variation | -15dB |

#### Ataques

| Accion | SFX | Descripcion | Volumen |
|--------|-----|-------------|---------|
| Light Attack 1 | Golpe rapido "swish" | 3 variations | -12dB |
| Light Attack 2 | Golpe rapido "swish" | 3 variations | -12dB |
| Light Attack 3 | Impacto "thud" | 2 variations | -10dB |
| Heavy Charge | Carga de energia | Loop mientras carga | -18dB |
| Heavy Release | Ataque pesado "whoosh" | 2 variations | -8dB |
| Attack Hit | Sonido de dano a enemigo | 4 variations | -6dB |
| Attack Miss | Golpe al aire | 2 variations | -14dB |

#### Estado del Player

| Accion | SFX | Descripcion | Volumen |
|--------|-----|-------------|---------|
| Take Damage | Grunto de dolor + impacto | 3 variations | -8dB |
| Low Health | Heartbeat + alarma sutil | Loop | -20dB |
| Death | Caida + ultimo suspiro | 1 variation | -6dB |
| Focus Charge | Energia concentrandose | Loop | -18dB |
| Focus Full | Campanita "ding" | 1 variation | -12dB |
| Heal | Sonido de curacion | 1 variation | -14dB |

---

### Efectos de Sonido (SFX) - Enemigos (Examenes)

#### Examen Escrito

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Idle | Papel arrugandose sutilmente | Loop |
| Move | Flotacion con viento suave | Loop |
| Attack | Hoja girando + "swish" | 1 variation |
| Hit | Sonido de papel rompiendo | 2 variations |
| Death | Fuego/chispas + suspiro | 1 variation |

#### Examen Multiple Choice

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Idle | "Tick tick" de reloj | Loop |
| Move | Rebote elastico | Loop |
| Attack | Burbuja explotando | 2 variations |
| Correct | "Ding" positivo | 1 variation |
| Wrong | "Buzz" negativo | 1 variation |

#### Examen Oral

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Idle | Murmullo/palabras distantes | Loop |
| Move | Voz acercandose | Loop |
| Attack | Onda de voz "WOOO" | 2 variations |
| Hit | Voz entrecortada | 2 variations |

#### Examen Laboratorio

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Idle | Burbujeo suave | Loop |
| Move | Arrastre con liquidos | Loop |
| Attack | Derrame/explosion pequena | 2 variations |
| Reaction | Cambio de color "splash" | 1 variation |

#### Minions (Todos)

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Spawn | Aparicion con "pop" | 1 variation |
| Move | Segun tipo (ver arriba) | - |
| Attack | Segun tipo (ver arriba) | - |
| Hit | Segun tipo | - |
| Death | Desaparicion con "poof" | 1 variation |

---

### Efectos de Sonido (SFX) - Boss (El Parcial Final)

#### Estados del Boss

| Accion | SFX | Descripcion | Volumen |
|--------|-----|-------------|---------|
| Idle | Respiracion pesada, amenazas | Loop | -20dB |
| Intro | "El examen va a comenzar" | 1 variation | -6dB |
| Phase 2 Transition | "Esto apenas empieza" | 1 variation | -6dB |
| Phase 3 Transition | "No... no puede ser" | 1 variation | -6dB |
| Attack Charge | Energia acumulandose | Loop | -16dB |
| Attack Execute | Ataque completo | 1 variation | -4dB |
| Take Damage | Gruñido de dano | 3 variations | -8dB |
| Low Health | Taunts desesperados | Loop | -12dB |
| Death | Colapso + silencio | 1 variation | -4dB |

#### Ataques Especificos del Boss

| Ataque | SFX | Descripcion |
|--------|-----|-------------|
| Integral Attack | Simbolo magico + "whoosh" | 1 variation |
| Derivative Attack | Numeros deslizandose | 1 variation |
| Furia Total | Orchestra hits + caos | Loop |
| Super Combo | Combo de sonidos rapidos | 5 hits |

#### Mensajes del Boss

| Fase | Mensaje | SFX |
|------|---------|-----|
| Fase 1 | "¡No podras vencerme!" | Voz profunda |
| Fase 1 | "Esto es solo el comienzo" | Voz amenazante |
| Fase 2 | "¡Ahora veras mi verdadero poder!" | Voz enfadada |
| Fase 2 | "¡No me vas a derrotar tan facilmente!" | Voz frustrada |
| Fase 3 | "¡Imposible! ¿Como puedes ser tan fuerte?" | Voz sorprendida |
| Fase 3 | "¡Juntos caeremos!" | Voz desesperada |

---

### UI Sounds

| Accion | SFX | Descripcion |
|--------|-----|-------------|
| Menu Hover | Click suave "blip" | 1 variation |
| Menu Select | Confirmacion "ding" | 1 variation |
| Menu Back | Cancelacion "bloop" | 1 variation |
| Pause Open | Sonido de pause | 1 variation |
| Pause Close | Sonido de resume | 1 variation |
| Victory Fanfare | Campanas + orquesta | 15 segundos |
| Game Over | Descenso dramatica | 10 segundos |

---

### Especificaciones Tecnicas

#### Formato de Archivos

| Uso | Formato | Sample Rate | Canales | Bit Depth |
|-----|---------|-------------|---------|-----------|
| Musica | WAV, OGG | 44.1 kHz | Stereo | 16-bit |
| SFX principales | WAV | 44.1 kHz | Mono/Stereo | 16-bit |
| SFX loop | WAV | 44.1 kHz | Mono | 16-bit |
| Voice lines | WAV | 48 kHz | Mono | 16-bit |

#### Nomenclatura de Archivos

```
// Musica
mus_title_loop.wav
mus_classroom_loop.wav
mus_library_loop.wav
mus_laboratory_loop.wav
mus_gameover.wav
mus_victory.wav

// SFX Player
sfx_player_walk_01.wav
sfx_player_walk_02.wav
sfx_player_run_01.wav
sfx_player_jump.wav
sfx_player_dash.wav
sfx_player_light_atk_01.wav
sfx_player_light_atk_02.wav
sfx_player_heavy_charge.wav
sfx_player_heavy_release.wav
sfx_player_hit_01.wav
sfx_player_death.wav
sfx_player_focus_charge.wav
sfx_player_focus_full.wav

// SFX Enemigos
sfx_exam_paper_idle.wav
sfx_exam_paper_attack.wav
sfx_exam_paper_hit.wav
sfx_exam_paper_death.wav
sfx_exam_multchoice_idle.wav
sfx_exam_multchoice_attack.wav
sfx_minion_quick_spawn.wav
sfx_minion_quick_death.wav

// SFX Boss
sfx_boss_intro.wav
sfx_boss_phase2_trans.wav
sfx_boss_phase3_trans.wav
sfx_boss_attack_charge.wav
sfx_boss_attack_execute.wav
sfx_boss_hit_01.wav
sfx_boss_death.wav
sfx_boss_taunt_phase1_01.wav
sfx_boss_taunt_phase2_01.wav
sfx_boss_taunt_phase3_01.wav

// SFX UI
sfx_ui_hover.wav
sfx_ui_select.wav
sfx_ui_back.wav
sfx_ui_pause_open.wav
sfx_ui_victory_fanfare.wav
sfx_ui_gameover.wav
```

#### Volumenes Referencia

| Categoria | Volumen Normalizado |
|----------|---------------------|
| Musica | -6dB a -3dB |
| SFX Player | 0dB (referencia) |
| SFX Enemigos | -3dB a 0dB |
| SFX Boss | +3dB a +6dB |
| Voz Boss | 0dB a +3dB |
| SFX UI | -6dB a -3dB |

#### Audio Mixing

| Escena | Musica | SFX | Voz |
|--------|--------|-----|-----|
| Titulo | 70% | 50% | N/A |
| Fase 1 | 80% | 100% | 100% (taunts) |
| Fase 2 | 75% | 100% | 100% (taunts) |
| Fase 3 | 70% | 100% | 100% (taunts) |
| Game Over | 50% | 100% | N/A |
| Victoria | 80% | 100% | N/A |

#### Formato de entrega

- Archivos organizados en carpetas por categoria
- CSV/spreadsheet con metadatos de cada archivo
- Documentacion de volumenes y timing
- Licencia de uso para todos los assets
