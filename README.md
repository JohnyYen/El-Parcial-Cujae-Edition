# 🎮 MVP – PARCIAL FINAL: CUJAE EDITION (GameJam)

## 🧠 Objetivo del MVP

Construir un **boss-fight estilo Cuphead**, completamente jugable en una GameJam (48h), con **identidad CUJAE clara**, mecánicas simples y alto impacto visual.

---

## 🎯 CORE GAME LOOP

```
Pantalla Inicio → Seleccionar Jugar → Boss Fight → Fase 1 → Fase 2 → Fase 3 → Resultado (APROBADO / REPROBADO) → Reintentar / Salir
```

---

## 🎮 MECÁNICAS BASE (PLAYER)

### Movimiento
- `← / →` : mover
- `⬆️` : salto
- `Shift` : dash corto (invulnerabilidad breve)

### Ataque
- `Z` : disparo normal (spam permitido)
- `X` : ataque especial (consume Concentración)

### Recursos

| Recurso | Descripción |
|---------|-------------|
| Estrés (HP) | Vida del jugador |
| Concentración | Carga para ataque especial |

---

## 👹 BOSS FIGHT – "EL PARCIAL FINAL"

### Concepto
Una hoja de examen viva, sellada, firmada y agresiva.

### Vida
- HP total dividido en 3 fases
- Cada fase cambia ataques y ritmo

---

## 🧩 FASES DEL BOSS

### 🟢 FASE 1 – "Tranquilo, era fácil"
**Objetivo:** Aprender patrones

**Ataques:**
- 📄 Hojas volando (proyectiles rectos)
- 🖊️ Bolígrafos lanzados en arco

**Minions:**
- Bug pequeño
- 1 HP
- Movimiento lento

**Ritmo:**
- Lento
- Mucho espacio para esquivar

---

### 🟡 FASE 2 – "Esto no lo dimos"
**Objetivo:** Presión constante

**Ataques:**
- ⏰ Relojes cayendo desde arriba
- 📐 Reglas giratorias (patrones circulares)

**Minions:**
- Bug mediano
- 2–3 HP
- Persigue al jugador

**Cambios:**
- Ataques combinados
- Menos tiempo entre patrones

---

### 🔴 FASE 3 – "El Integrador"
**Objetivo:** Supervivencia total

**Ataques:**
- 📄 + ⏰ simultáneos
- Tinta derramada (zonas peligrosas)
- Texto rojo distorsionado

**Minions:**
- Bug grande
- No muere
- Controla espacio

**Ritmo:**
- Muy rápido
- Ataques encadenados

---

## 👾 MINIONS (MVP)

| Tipo | Vida | Función |
|------|------|---------|
| Bug pequeño | 1 | Molestar |
| Bug mediano | 2–3 | Presión |
| Bug grande | ∞ | Forzar movimiento |

---

## ☕ POWER-UPS CUJAE

| Power-up | Efecto | Duración |
|----------|--------|----------|
| Café | +Velocidad, +Cadencia de disparo | 5s |
| Apuntes | Escudo (bloquea 1 golpe) | Se rompe visualmente |

---

## 🎨 ESCENARIO (ÚNICO – MVP)

**Aula CUJAE estilizada:**
- Pizarra
- Ventanas altas
- Columnas
- Ventilador roto

**Elementos vivos:**
- Tiza cayendo
- Sonido ambiente bajo

---

## 🖥️ PANTALLAS REQUERIDAS

### 1️⃣ Pantalla Inicial
- Logo del juego
- Texto: "PARCIAL FINAL"
- Botón: JUGAR / SALIR

### 2️⃣ Pantalla de Controles
- Movimiento
- Disparo
- Dash
- Especial

### 3️⃣ Pantalla de Resultado

**Victoria:**
- Texto grande: APROBADO
- Sello animado

**Derrota:**
- Texto grande: REPROBADO
- Sonido grave
- Opciones: Reintentar / Volver al menú

---

## 🧪 MVP FEATURES (CHECKLIST)

### Imprescindible
- [ ] Movimiento + salto + dash
- [ ] Disparo normal
- [ ] Ataque especial
- [ ] 1 Boss con 3 fases
- [ ] 3 tipos de ataques
- [ ] 2 tipos de minions
- [ ] HUD simple
- [ ] Pantallas básicas

### Deseable (si hay tiempo)
- [ ] Animaciones exageradas
- [ ] Música dinámica
- [ ] Efectos de cámara
- [ ] Ranking local

---

## ⏱️ PLAN DE TRABAJO 48H

### Día 1
- Player movement + disparo
- Arena + cámara
- Boss fase 1

### Día 2
- Fases 2 y 3
- UI + pantallas
- Pulido visual
- Testeo

---

## 🏁 DEFINICIÓN DE ÉXITO (GAMEJAM)

- ✔ Se entiende sin explicación
- ✔ Se siente difícil pero justo
- ✔ Se reconoce CUJAE
- ✔ Es divertido en 3 minutos
- ✔ Se puede terminar

---

## 🚀 NOMBRE FINAL (OPCIONAL)

**Parcial Final: CUJAE Edition**

---

> 🧠 **Apruebas si sobrevivís.**
> 🎮 **Ganas si jugás bien.**
> 🎓 **Eres CUJAE.**
