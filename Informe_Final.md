# Informe Final - El Parcial Final: CUJAE Edition

## 1. Breve Descripción del Proyecto

**El Parcial Final: CUJAE Edition** es un videojuego de tipo *boss-fight* desarrollado en el motor Unity, inspirado en títulos como Cuphead, con una estética visual 2D y mecánicas de combate desafiantes. El juego fue concebido como un proyecto para una GameJam de 48 horas, con el objetivo de crear una experiencia completamente jugable que capture la identidad y el ambiente de la vida universitaria del CUJAE (Centro Universitario de Tecnologías y Artes Digitales).

El núcleo del juego consiste en un enfrentamiento épico contra un jefe final único: "El Parcial Final", una hoja de examen viviente, sellada y agresiva que representa la temida evaluación final del semestre. El jugador debe demostrar habilidad, reflejos y estrategia para "aprobar" el examen, sobreviviendo a través de tres fases de dificultad progresivamente creciente.

El proyecto se caracteriza por su identidad visual distintiva, con assets artísticos generados mediante herramientas de inteligencia artificial, lo que le confiere un estilo artístico único y coherente. La experiencia de juego está diseñada para ser intensa pero justa, desafiante pero gratificante, capturando la tensión y la adrenalina de enfrentar un examen universitario decisivo.

---

## 2. Listado de Integrantes y Funciones

El desarrollo del proyecto fue llevado a cabo por un equipo multidisciplinario de seis integrantes, cada uno especializado en áreas específicas del desarrollo de videojuegos:

### **Fernando** - Desarrollador de UI
- **Responsabilidades principales:**
  - Diseño e implementación de todas las interfaces de usuario del juego
  - Desarrollo del sistema de menús (pantalla de título, menú de opciones, pantalla de créditos)
  - Creación del HUD (Heads-Up Display) durante el gameplay
  - Implementación de transiciones entre escenas
  - Gestión de la navegación mediante teclado y gamepad
  - Desarrollo de pantallas de resultado (Victoria/Derrota)

### **Yanni** - Diseñador de Audio y UI
- **Responsabilidades principales:**
  - Composición y producción de la banda sonora del juego
  - Creación de efectos de sonido (SFX) para todos los elementos interactivos
  - Diseño de audio ambiental y atmosférico
  - Implementación del sistema de audio en Unity
  - Colaboración en el desarrollo de elementos de UI

### **Johny** - Programador de Gameplay
- **Responsabilidades principales:**
  - Implementación de las mecánicas básicas del jugador (movimiento, salto, dash)
  - Desarrollo del sistema de combate (ataques suaves y fuertes)
  - Programación de los sistemas de recursos (vida/estrés y concentración/enfoque)
  - Implementación de power-ups y buffs
  - Desarrollo de la lógica de físicas y colisiones
  - Balanceo de parámetros de gameplay

### **Sandro** - Artista Principal
- **Responsabilidades principales:**
  - Dirección artística del proyecto
  - Supervisión del estilo visual coherente
  - Creación y edición de sprites y animaciones
  - Diseño de assets visuales para el escenario
  - Creación de efectos visuales y partículas
  - Diseño de la interfaz visual
  - Establecimiento de la paleta de colores

### **Dashiel** - Artista
- **Responsabilidades principales:**
  - Colaboración en la creación de assets visuales
  - Desarrollo de sprites y animaciones de personajes
  - Apoyo en el diseño de efectos visuales

### **Carlos** - Programador de Gameplay
- **Responsabilidades principales:**
  - Implementación del sistema de inteligencia artificial del jefe
  - Desarrollo de las tres fases del boss con sus respectivos comportamientos
  - Programación de los diferentes tipos de ataques del jefe
  - Implementación del sistema de spawning de minions
  - Desarrollo de la máquina de estados del boss
  - Integración de mecánicas de combate del jefe

---

## 3. Historia y Temática del Juego

### Contexto Narrativo

La historia de **El Parcial Final: CUJAE Edition** se sitúa en el contexto de la vida universitaria cubana, específicamente en el ambiente del CUJAE. El juego utiliza metáforas viscerales y mecánicas de juego para representar la experiencia de enfrentar un examen final decisivo.

El protagonista es un estudiante que debe enfrentarse a "El Parcial Final", una entidad antropomórfica que representa la manifestación física del estrés, la presión y el desafío académico. Esta entidad no es simplemente un enemigo común, sino una fuerza casi sobrenatural que ha cobrado vida a través de la ansiedad colectiva de generaciones de estudiantes.

### Las Tres Fases del Examen

La narrativa del juego se desarrolla a través de tres fases distintas, cada una representando una etapa diferente del enfrentamiento contra el parcial:

#### **Fase 1: "Tranquilo, era fácil"**
En esta primera fase, el jefe adopta una actitud engañosamente simple. Representa ese momento inicial del examen donde todo parece manejable, donde las preguntas parecen directas y el tiempo abunda. El jefe lanza ataques básicos como hojas voladoras y bolígrafos en arco, creando un ambiente de falsa confianza. Los mensajes del jefe son provocadores pero no amenazantes: *"Esto es solo el comienzo"*, *"¡Prepárate para sufrir!"*

Esta fase sirve como tutorial táctico, permitiendo al jugador aprender los patrones de ataque y familiarizarse con los controles antes de que la verdadera dificultad comience.

#### **Fase 2: "Esto no lo dimos"**
La transición a la segunda fase representa ese momento crítico en cualquier examen cuando el estudiante se da cuenta de que la evaluación incluye material que no se cubrió en clase o que era más complejo de lo esperado. El jefe aumenta drásticamente su agresividad.

El ambiente cambia: aparecen relojes cayendo desde arriba (representando la presión del tiempo) y reglas giratorias (simbolizando las estrictas normas académicas). Los ataques se vuelven más complejos y se combinan entre sí, creando patrones que requieren mayor atención y habilidad. Los minions que aparecen son más agresivos y resistentes.

Los mensajes del jefe reflejan su frustración: *"¡Esto apenas empieza!"*, *"¡Ahora verás mi verdadero poder!"*. Esta fase representa el momento de crisis donde el estudiante debe demostrar su verdadera preparación.

#### **Fase 3: "El Integrador"**
La fase final es el clímax de la experiencia. El jefe entra en modo de desesperación total, lanzando todos sus recursos restantes. Esta fase simboliza el examen integrador, esa evaluación comprehensiva que pone a prueba todo el conocimiento acumulado durante el semestre.

Los ataques son ahora devastadores y múltiples: lluvia de proyectiles, embestidas furiosas, y ataques masivos que cubren grandes porciones de la pantalla. El ritmo se acelera al máximo, dejando poco margen para el error. El jefe ya no solo ataca; desata todo su poder académico acumulado.

Los mensajes finales del jefe reflejan su desesperación: *"¡Imposible! ¡¿Cómo puedes ser tan fuerte?!"*, *"¡No... no puede ser!"*. Esta fase representa la batalla final por la aprobación, donde cada movimiento cuenta.

### Simbolismo y Metáforas

El juego está lleno de simbolismo relacionado con la experiencia universitaria:

- **El Café**: Representa el combustible del estudiante durante las noches de estudio. En el juego otorga velocidad y cadencia de disparo aumentada.
- **Los Apuntes**: Simbolizan la preparación y el conocimiento. En el juego funcionan como escudo protector.
- **Los Minions (Bugs)**: Representan los errores y problemas técnicos que siempre aparecen en los momentos más críticos. Cada tipo de bug tiene características diferentes, desde simples molestias hasta obstáculos serios.
- **El Estrés (Vida)**: El recurso de vida del jugador se denomina "Estrés", reconociendo que en el contexto universitario, el estrés es una forma de "salud mental" que debe gestionarse.
- **La Concentración (Enfoque)**: El recurso para ataques especiales representa la capacidad de enfocarse que el estudiante debe mantener durante el examen.

El escenario, un aula del CUJAE estilizada con pizarra, ventanas altas, columnas y un ventilador roto, ancla la experiencia en un entorno familiar para cualquier estudiante cubano.

---

## 4. Herramientas Utilizadas

El desarrollo del proyecto hizo uso de un stack tecnológico moderno y especializado:

### Motor de Juego
- **Unity 6000.x (Unity 6)**: Motor de juego principal utilizado para el desarrollo. Proporciona un entorno robusto para el desarrollo 2D, con soporte para físicas, animaciones, audio y renderizado optimizado.

### Herramientas de Desarrollo de Software
- **Visual Studio / VS Code**: Entornos de desarrollo integrado para la programación en C#
- **Git**: Sistema de control de versiones para gestionar el código fuente
- **GitHub**: Plataforma de hospedaje del repositorio y colaboración

### Herramientas de Inteligencia Artificial para Generación de Assets Artísticos

El proyecto utilizó herramientas de IA específicamente para la **creación de contenido artístico** (sprites, animaciones, música y audio), no para programación ni diseño de mecánicas:

- **Ludo.ai**: Plataforma de IA especializada en la generación de assets de videojuegos. Utilizada para crear las animaciones de los personajes, sprites y elementos visuales del juego, permitiendo un desarrollo rápido de contenido artístico de alta calidad.

- **Google Gemini**: Modelo de lenguaje grande utilizado para la generación de imágenes estáticas, concept art y assets visuales adicionales que complementaron el estilo artístico del juego.

- **Suno.ai**: Plataforma de IA para generación de música y audio. Utilizada para componer la banda sonora original del juego y crear efectos de sonido personalizados, permitiendo una banda sonora única sin necesidad de contratar compositores externos.

### Documentación
- **Markdown**: Formato utilizado para la documentación de especificaciones, historias de usuario y requerimientos técnicos

### Nota sobre Assets Externos
Es importante destacar que el proyecto **no utilizó ningún asset externo** de mercados como Unity Asset Store u otras fuentes. Todo el contenido artístico, musical y de diseño fue generado específicamente para este proyecto mediante las herramientas de IA mencionadas, o creado directamente por los artistas del equipo (Sandro y Dashiel) utilizando dichas herramientas como apoyo en el flujo de trabajo creativo.

---

## 5. Mecánicas y Reglas del Juego

### 5.1 Controles del Jugador

El sistema de control fue diseñado para ser intuitivo pero con suficiente profundidad para permitir expresión de habilidad:

| Acción | Teclado | Gamepad | Mouse |
|--------|---------|---------|-------|
| Movimiento Lateral | ← / → | Left Stick | - |
| Salto | ↑ | Button A | - |
| Dash | Shift | Button X | Right Click |
| Disparo Normal (Ataque Suave) | Z / C | Button Y | Left Click |
| Ataque Especial (Ataque Fuerte) | X / V | Button B | - |

#### Sistema de Movimiento

**Movimiento Lateral:**
El jugador puede moverse horizontalmente a velocidad constante. El sistema implementa aceleración y desaceleración suaves para evitar movimientos abruptos. La dirección del movimiento también determina la orientación del sprite del personaje.

**Salto:**
- Ejecutable solo cuando el personaje está en contacto con el suelo
- Impulso vertical inmediato con gravedad aplicada durante la caída
- Altura de salto configurable mediante parámetros de física
- Sistema de detección de suelo mediante raycasts para determinar cuándo se puede saltar

**Dash:**
- Desplazamiento rápido en la dirección actual del movimiento
- Proporciona **invulnerabilidad total** durante la ejecución (0.2-0.5 segundos)
- Cooldown de 2-3 segundos entre usos
- Velocidad 2x-3x superior a la velocidad normal de movimiento
- Trail visual que indica el uso de la habilidad
- No consume recursos, pero requiere timing estratégico

### 5.2 Sistema de Recursos

El juego implementa un sistema de doble recurso que añade profundidad estratégica:

#### Estrés (Vida / HP)
- **Valor inicial:** 100%
- **Función:** Representa la salud mental del estudiante. Cuando llega a 0%, el jugador "suspende" (Game Over).
- **Daño recibido:** Depende del tipo de ataque del enemigo (10-60 puntos según el ataque)
- **Regeneración:** No hay regeneración natural; la única forma de recuperar estrés es mediante power-ups o mecánicas específicas.
- **Visualización:** Barra de vida en el HUD con cambio de color según el nivel (verde → amarillo → rojo).

#### Concentración (Enfoque / Focus)
- **Valor inicial:** 0%
- **Capacidad máxima:** 100%
- **Función:** Recurso necesario para ejecutar ataques especiales
- **Ganancia:** 
  - Derrotar minions otorga enfoque (10-35 puntos según el tipo)
  - Esquivas exitosas (usar dash para evitar daño)
  - Golpear al jefe con ataques normales
- **Consumo:** Cada ataque especial consume una cantidad específica de concentración
- **Visualización:** Barra secundaria en el HUD, generalmente de color azul o cyan.

### 5.3 Sistema de Ataques del Jugador

#### Ataque Suave (Disparo Normal)
- **Input:** Z o C
- **Daño:** 10-15 puntos por impacto
- **Velocidad:** Ejecución rápida (0.3 segundos)
- **Recovery:** Casi inmediato (0.1 segundos)
- **Rango:** Medio (proyectiles que viajan en línea recta)
- **Características:**
  - Puede realizarse repetidamente (spam permitido)
  - No consume recursos
  - Ideal para daño continuo y acumulativo
  - Proyectiles pueden destruirse al impactar con ataques del jefe

#### Ataque Especial (Ataque Fuerte)
- **Input:** X o V
- **Daño:** 25-60 puntos según el tipo
- **Velocidad:** Ejecución lenta (0.6 segundos)
- **Recovery:** Significativo si falla (0.4 segundos)
- **Rango:** Variable según el tipo de ataque especial
- **Características:**
  - Consume concentración (enfoque)
  - Puede romper ciertos ataques del jefe
  - Causa stun breve en enemigos
  - Mayor daño por golpe
  - Requiere timing y posicionamiento cuidadosos

### 5.4 Sistema de Power-ups

El juego incluye power-ups temáticos que representan elementos típicos de la vida estudiantil:

#### Café ☕
- **Efecto:** Aumenta temporalmente la velocidad de movimiento y la cadencia de disparo
- **Duración:** 5 segundos
- **Representación:** Taza de café animada
- **Estrategia:** Ideal para momentos de alta presión donde se necesita movilidad extra

#### Apuntes 📚
- **Efecto:** Proporciona un escudo que bloquea completamente el siguiente golpe recibido
- **Duración:** Persistente hasta recibir daño
- **Visual:** El personaje muestra un indicador visual cuando está protegido
- **Estrategia:** Permite cometer un error sin consecuencias; útil para aprender patrones de ataque

### 5.5 Sistema de Minions

Los minions son enemigos secundarios spawnados por el jefe para aumentar la complejidad del combate. Existen tres tipos, cada uno con comportamientos y estadísticas diferenciadas:

#### Minion Básico (Tipo 1)
- **Vida:** 50 HP
- **Daño:** 10 puntos por contacto
- **Velocidad:** 2.0 unidades/segundo (lento)
- **Comportamiento:** 
  - Movimiento directo hacia el jugador
  - Ataque de contacto simple
  - Fácil de esquivar pero puede bloquear movimientos
- **Enfoque al derrotar:** 10 puntos
- **Uso por el jefe:** Fase 1 (100%), Fase 2 (50%)

#### Minion Medio (Tipo 2)
- **Vida:** 70 HP
- **Daño:** 20 puntos por contacto
- **Velocidad:** 3.5 unidades/segundo (rápido)
- **Comportamiento:**
  - Persecución agresiva
  - Mayor prioridad de atención requerida
  - Puede alcanzar al jugador rápidamente
- **Enfoque al derrotar:** 20 puntos
- **Uso por el jefe:** Fase 2 (50%), Fase 3 (100%)

#### Minion Difícil (Tipo 3)
- **Vida:** 120 HP
- **Daño:** 30 puntos por contacto
- **Velocidad:** 1.8 unidades/segundo (lento pero resistente)
- **Comportamiento:**
  - Tanque con 25% de reducción de daño
  - Requiere múltiples ataques para derrotar
  - Puede bloquear rutas de escape
- **Enfoque al derrotar:** 35 puntos
- **Uso por el jefe:** Reservado para uso especial (no spawneado normalmente)

**Sistema de Spawning:**
- Máximo 5-8 minions activos simultáneamente
- Spawn en 4-6 puntos fijos alrededor del área de juego
- Frecuencia y tipos dependen de la fase actual del jefe
- Los minions otorgan enfoque al ser derrotados, incentivando su eliminación

### 5.6 Sistema del Boss: "El Parcial Final"

El boss es el elemento central del juego, diseñado como un enfrentamiento épico de tres fases con comportamientos y ataques distintivos en cada una.

#### Arquitectura del Sistema de Fases

El jefe implementa un sistema de tres fases que se activan según su porcentaje de vida restante:

**Fase 1 (100% - 66% de vida): "Introducción y Aprendizaje"**
- **Comportamiento:** Patrones básicos, velocidad normal (1.0x)
- **Objetivo didáctico:** Permitir al jugador aprender los controles y patrones básicos
- **Frecuencia de ataques:** Media (cada 2-3 segundos)
- **Spawning de minions:** 100% Básicos

**Ataques disponibles en Fase 1:**

| Ataque | Daño | Rango | Cooldown | Descripción Mecánica |
|--------|------|-------|----------|---------------------|
| **Golpe Básico** | 10-15 | Cuerpo a cuerpo | 2s | Ataque simple en arco corto frente al boss. Señalizado por animación de preparación. |
| **Embestida** | 15-20 | Medio | 3s | Carga rápida en línea recta hacia la posición del jugador. Deja al boss vulnerable brevemente después. |
| **Combo Doble** | 20-25 | Cuerpo a cuerpo | 4s | Dos golpes consecutivos. El segundo golpe tiene mayor alcance que el primero. |

**Fase 2 (66% - 33% de vida): "Escalada de Dificultad"**
- **Comportamiento:** Aumento significativo de agresividad, velocidad aumentada (1.25x)
- **Transición:** Animación de furia con mensaje de diálogo
- **Frecuencia de ataques:** Alta (cada 1.5-2 segundos)
- **Spawning de minions:** 50% Básicos, 50% Medios

**Ataques disponibles en Fase 2 (incluye los de Fase 1 mejorados + nuevos):**

| Ataque | Daño | Rango | Cooldown | Descripción Mecánica |
|--------|------|-------|----------|---------------------|
| **Golpe Básico+** | 15-20 | Cuerpo a cuerpo | 1.5s | Versión mejorada del golpe básico. Animación más rápida, menos telegrafiado. |
| **Embestida** | 20-25 | Medio | 2.5s | Versión más rápida de la embestida. Recuperación más corta. |
| **Onda de Choque** | 25-30 | Largo | 5s | Proyectil que viaja en arco parabólico desde el boss hacia el jugador. Debe esquivarse saltando o usando dash. |
| **Ataque Giratorio** | 30-35 | Área circular | 6s | El boss gira 360° emitiendo daño en área alrededor de sí mismo. Requiere mantener distancia. |

**Fase 3 (33% - 0% de vida): "Desesperación Total"**
- **Comportamiento:** Modo desesperación - ataques máximos, velocidad máxima (1.5x)
- **Transición:** Animación dramática de transformación final
- **Frecuencia de ataques:** Muy alta (cada 1 segundo)
- **Spawning de minions:** 100% Medios (más peligrosos)
- **Característica especial:** Los ataques se combinan, creando patrones complejos

**Ataques disponibles en Fase 3 (incluye todos los anteriores mejorados + súper ataques):**

| Ataque | Daño | Rango | Cooldown | Descripción Mecánica |
|--------|------|-------|----------|---------------------|
| **Furia Total** | 35-45 | Todo el escenario | 8s | Ataque masivo que cubre grandes áreas de la pantalla. Requiere posicionamiento preciso o dash. |
| **Lluvia de Proyectiles** | 10-15 c/u | Largo | 7s | Múltiples proyectiles cayendo desde arriba en patrón. Simula "preguntas difíciles" cayendo. |
| **Embestida Furiosa** | 40-50 | Largo | 4s | Carga extremadamente rápida con rango aumentado. El boss puede cambiar de dirección una vez durante la carga. |
| **Super Combo** | 50-60 | Cuerpo a cuerpo | 10s | Combo de 4-5 golpes consecutivos. Último golpe causa stun. |
| **Desesperación** | 45-55 | Área grande | 12s | Explosión de energía en área amplia alrededor del boss. Usado como último recurso cuando tiene poca vida. |

#### Máquina de Estados del Boss

El jefe opera mediante una máquina de estados compleja:

```
IDLE → (Timer expira) → SELECCIONAR_ATAQUE → EJECUTAR_ATAQUE → IDLE
       → (Jugador cerca) → ATAQUE_CERCANO → IDLE
       → (Vida < 66%) → TRANSICIÓN_FASE_2 → FASE_2 (nuevos comportamientos)
       → (Vida < 33%) → TRANSICIÓN_FASE_3 → FASE_3 (nuevos comportamientos)
       → (Vida <= 0%) → MUERTE → PANTALLA_VICTORIA
```

**Lógica de Decisión (IA):**
- El boss selecciona ataques basándose en:
  - Distancia actual al jugador
  - Fase actual del combate
  - Cooldowns disponibles
  - Comportamiento pseudo-aleatorio ponderado por fase
- Siempre mantiene agresión hacia el jugador (aggro)
- Ataques de área preferidos cuando el jugador mantiene distancia
- Ataques cuerpo a cuerpo preferidos cuando el jugador está cerca

#### Sistema de Mensajes del Boss

El jefe comunica su estado emocional mediante mensajes de diálogo que aparecen en pantalla durante las transiciones de fase:

**Fase 1 - Mensajes de Introducción:**
- "¡No podrás vencerme!"
- "Esto es solo el comienzo"
- "¡Prepárate para sufrir!"

**Fase 2 - Mensajes de Frustración:**
- "¡Esto apenas empieza!"
- "¡Ahora verás mi verdadero poder!"
- "¡No me vas a derrotar tan fácilmente!"

**Fase 3 - Mensajes de Desesperación:**
- "¡Imposible! ¡¿Cómo puedes ser tan fuerte?!"
- "¡No... no puede ser!"
- "¡Juntos caeremos!"

### 5.7 Condiciones de Victoria y Derrota

#### Victoria (APROBADO)
- Se logra reduciendo la vida del jefe a 0%
- Aparece pantalla de victoria con:
  - Texto grande: "APROBADO"
  - Sello animado (visual de éxito)
  - Mensaje de felicitación
  - Opciones: "Volver al Menú" o "Reintentar"

#### Derrota (REPROBADO)
- Ocurre cuando el estrés del jugador llega a 0%
- Aparece pantalla de derrota con:
  - Texto grande: "REPROBADO"
  - Sonido grave y efectos visuales de fracaso
  - Estadísticas de la partida (tiempo, daño causado, etc.)
  - Opciones: "Reintentar" o "Volver al Menú"

### Capturas de pantalla

![[Battle1.png]]

![[Battle2.png]]

![[Battle3.png]]

![[GameOver.png]]

![[Text1.png]]

![[Title.png]]
  

---

## 6. Referencias

### Inspiraciones Principales

1. **Cuphead (Studio MDHR, 2017)**
   - Principal inspiración para el estilo de boss-fight
   - Influencia en el diseño de múltiples fases con comportamientos distintivos
   - Referencia para el sistema de ataques desafiantes pero justos
   - Inspiración para la estética visual 2D con animaciones fluidas

2. **Experiencia Universitaria Cubana**
   - Contexto cultural y ambiental del CUJAE
   - Situaciones cotidianas de la vida estudiantil (exámenes, café, apuntes)
   - Humor y referencias locales a la experiencia académica cubana

### Recursos Utilizados

#### Assets Artísticos y Visuales
Los assets artísticos del juego fueron creados mediante un proceso colaborativo entre los artistas del equipo (Sandro y Dashiel) y herramientas de IA:
- **Ludo.ai**: Generación base de sprites y animaciones
- **Google Gemini**: Creación de imágenes estáticas y concept art
- **Edición manual por artistas**: Refinamiento, ajuste y optimización de assets generados

#### Audio y Música
La banda sonora y efectos de sonido fueron generados utilizando:
- **Suno.ai**: Composición musical y generación de pistas de audio
- **Edición y masterización por Yanni**: Adaptación e integración en el juego

#### Desarrollo de Software
- **Documentación oficial de Unity 6000.x**: Referencia técnica para programación
- **Patrones de diseño de juegos**: Máquinas de estado, Object Pooling, Component Pattern
- **Especificaciones internas del proyecto**: Archivos markdown en carpeta `/spec/`

### Nota sobre Originalidad
Todo el código fuente del juego fue programado desde cero por los desarrolladores (Johny y Carlos). Las herramientas de IA fueron utilizadas exclusivamente como apoyo en la **generación de contenido artístico**, no en la programación ni en el diseño de mecánicas de juego.

### Agradecimientos

- A la comunidad de desarrolladores de Unity por documentación y recursos educativos
- A los creadores de herramientas de IA (Ludo.ai, Suno.ai, Gemini) por democratizar la creación de contenido artístico
- A todos los beta testers que probaron el juego durante el desarrollo

---

## 7. Mejoras a Futuro

El proyecto, aunque completado como MVP para la GameJam, tiene un considerable potencial de expansión. El equipo ha identificado las siguientes áreas de mejora para futuras iteraciones:

### 7.1 Expansión de Contenido

**Nuevos Niveles y Bosses:**
- Implementar múltiples niveles con diferentes jefes finales, cada uno representando diferentes asignaturas o desafíos académicos (ej: "El Proyecto Final", "La Tesis", "El Examen de Ingreso")
- Cada nuevo boss tendría mecánicas únicas temáticas (ej: un boss de Matemática que use patrones geométricos, uno de Programación que spawnee "bugs" de software)

**Modo Historia:**
- Desarrollar una campaña completa donde el jugador progrese a través de diferentes "semestres"
- Incluir niveles intermedios con minions antes de llegar a cada boss final
- Sistema de progresión con desbloqueo de nuevas habilidades

### 7.2 Mejoras en Gameplay

**Fluididad y Respuesta:**
- Refinar el sistema de movimiento para hacerlo más preciso y satisfactorio
- Implementar *input buffering* (almacenamiento de inputs) para combos más fluidos
- Mejorar la detección de colisiones y hitboxes
- Optimizar el rendimiento para mantener 60 FPS constantes

**Profundidad Estratégica:**
- Agregar más tipos de ataques especiales con diferentes costos de concentración
- Implementar un sistema de combo más elaborado
- Agregar mecánicas de parry (contraataque) o defensa activa
- Sistema de upgrade donde el jugador puede mejorar sus estadísticas entre niveles

**Balanceo:**
- Ajustar la dificultad basándose en playtesting más extenso
- Implementar diferentes niveles de dificultad (Fácil, Normal, Difícil, Extremo)
- Sistema adaptativo que ajuste la dificultad según el desempeño del jugador

### 7.3 Exploración y Mundo Abierto

**Sistema de Exploración:**
- Transformar el juego de una experiencia de boss-fight lineal a un metroidvania ligero
- Permitir al jugador explorar el campus del CUJAE entre combates
- Incluir áreas secretas con power-ups o lore adicional

**NPCs y Quests:**
- Agregar personajes no jugables que den misiones secundarias
- Sistema de diálogo con opciones de respuesta
- Historias secundarias que expandan el lore del universo

**Hub Central:**
- Crear un área segura (cafetería o biblioteca) donde el jugador pueda:
  - Guardar progreso
  - Interactuar con NPCs
  - Acceder a tienda de mejoras
  - Revisar estadísticas y logros

### 7.4 Mejoras Técnicas

**Optimización:**
- Implementar Object Pooling para minions y proyectiles
- Optimizar renderizado 2D para dispositivos de bajos recursos
- Reducir tiempos de carga entre escenas

**Plataformas Adicionales:**
- Portar el juego a móviles (iOS/Android) con controles táctiles
- Adaptación para consolas (Nintendo Switch, PlayStation, Xbox)
- Soporte para múltiples resoluciones y aspect ratios

**Online Features:**
- Tabla de clasificación online (leaderboards)
- Modo speedrun con temporizador integrado
- Sistema de logros/trofeos
- Compartir repeticiones de partidas

### 7.5 Mejoras Artísticas y de Presentación

**Animaciones:**
- Agregar más frames de animación para transiciones más suaves
- Animaciones de reacción más expresivas para el jefe
- Efectos de partículas más elaborados para impactos y habilidades

**Narrativa:**
- Cinemáticas entre niveles
- Más diálogos y desarrollo de personajes
- Final alternativos según el desempeño

**Accesibilidad:**
- Opciones de accesibilidad para jugadores con discapacidades visuales/auditivas
- Modo de alto contraste
- Opciones de velocidad de juego ajustable
- Remapeo completo de controles

### 7.6 Contenido Post-Lanzamiento

**DLC Potenciales:**
- "El Semestre de Verano": Nuevo boss y nivel
- "La Práctica Profesional": Modo de juego con mecánicas diferentes
- Pack de skins alternativos para el personaje

**Actualizaciones Gratuitas:**
- Modo desafío semanal con modificadores
- Nuevos tipos de minions
- Boss rush mode (enfrentar todos los jefes seguidos)

---

## Conclusión

**El Parcial Final: CUJAE Edition** representa una realización exitosa de un videojuego de boss-fight en el contexto de una GameJam de 48 horas. El proyecto demuestra que con herramientas modernas de IA para generación de assets artísticos y un equipo multidisciplinario comprometido, es posible crear experiencias de juego completas, coherentes y disfrutables en tiempos limitados.

La decisión de utilizar herramientas de IA para la generación de contenido artístico no solo resolvió problemas de coherencia visual, sino que también permitió un desarrollo rápido sin sacrificar calidad. La temática universitaria cubana proporciona un contexto único y culturalmente relevante que distingue al juego de otros títulos similares.

Las mecánicas implementadas, especialmente el sistema de tres fases del boss, proporcionan una curva de dificultad satisfactoria que enseña al jugador progresivamente mientras aumenta el desafío. El sistema de doble recurso (estrés y concentración) añade profundidad estratégica sin complicar innecesariamente la experiencia.

El trabajo colaborativo del equipo, con roles claramente definidos entre programadores (Johny y Carlos), artistas (Sandro y Dashiel), y especialistas en UI y audio (Fernando y Yanni), permitió una división eficiente del trabajo y un producto final polido.

Con las mejoras futuras planificadas, el proyecto tiene el potencial de evolucionar de un MVP de GameJam a un producto comercial completo, expandiendo su alcance y duración mientras mantiene la esencia que lo hace único: la representación gamificada de la experiencia universal de enfrentar un examen final.

**"Apruebas si sobrevivís. Ganas si jugás bien. Eres CUJAE."**

---

*Informe Final elaborado por el equipo de desarrollo*  
*Fecha: Febrero 2026*  
*Versión: 1.0*
