---
title: Interfaces del Sistema de Combate
assignee: johny
---

## Historia de Usuario

Como desarrollador, quiero un conjunto de interfaces bien definidas para estandarizar el comportamiento de los personajes y entidades del juego, permitiendo polymorphism y código mantenible.

## Descripción

Este documento define las interfaces base del sistema de combate que serán implementadas por Player, Boss, Minion y cualquier otra entidad del juego. Las interfaces establecen contratos claros para ataques, movimiento, vida y spawning.

## Interfaces Requeridas

### IBoss

Contrato principal para el enemigo boss que encapsula ataques, gestión de fases y spawning.

```typescript
interface IBoss {
  // Verificaciones de salud
  VerificarSalud(): FaseBoss;
  
  // Transición de fases
  CambiarFase(nuevaFase: FaseBoss): void;
  
  // Ataques por fase
  EjecutarAtaqueFase1(): void;
  EjecutarAtaqueFase2(): void;
  EjecutarAtaqueFase3(): void;
  
  // Spawning de enemigos
  SpawnEnemigo(tipoEnemigo: TipoMinion): void;
  
  // Estado del boss
  ObtenerVidaActual(): number;
  ObtenerVidaMaxima(): number;
  EstaVivo(): boolean;
}
```

### IAtaqueBoss

Define los patrones de ataque específicos que el boss puede ejecutar.

```typescript
interface IAtaqueBoss {
  // Identificación del ataque
  ObtenerNombreAtaque(): string;
  ObtenerDamage(): number;
  
  // Ejecución
  Ejecutar(): void;
  EstaEnProgreso(): boolean;
  
  // Cooldown
  ObtenerCooldown(): number;
  ObtenerTiempoUltimoAtaque(): number;
  
  // Configuración por fase
  EsValidoParaFase(fase: FaseBoss): boolean;
}
```

### ISpawnEnemigo

Contrato para el spawning de enemigos en el campo de batalla.

```typescript
interface ISpawnEnemigo {
  // Spawning básico
  Spawn(tipo: TipoMinion, posicion: Vector2): IMinion;
  
  // Spawning en oleadas
  IniciarOleada(cantidad: number, tipo: TipoMinion, intervalo: number): void;
  FinalizarOleada(): void;
  
  // Gestión de pool
  ObtenerEnemigosActivos(): IMinion[];
  ObtenerEnemigosRestantes(): number;
  
  // Configuración
  ObtenerPosicionSpawn(): Vector2[];
}
```

### IMinion

Contrato base para todos los minions del juego.

```typescript
interface IMinion {
  // Identificación
  ObtenerTipo(): TipoMinion;
  ObtenerNivel(): number;
  
  // Vida
  ObtenerVida(): number;
  ObtenerVidaMaxima(): number;
  RecibirDamage(cantidad: number): void;
  
  // Movimiento
  Mover(direccion: Vector2): void;
  ObtenerVelocidad(): number;
  
  // Ataque
  Atacar(objetivo: ITargetable): void;
  ObtenerRangoAtaque(): number;
  ObtenerDamage(): number;
  
  // Estado
  EstaVivo(): boolean;
  Morir(): void;
}
```

### IPlayer

Contrato para el jugador principal, adaptable según el tipo de player.

```typescript
interface IPlayer {
  // Identificación
  ObtenerTipoPlayer(): TipoPlayer;
  
  // Movimiento
  Mover(direccion: Vector2): void;
  Saltar(): void;
  Dash(direccion: Vector2): void;
  ObtenerVelocidadMovimiento(): number;
  ObtenerVelocidadDash(): number;
  
  // Vida
  ObtenerVida(): number;
  ObtenerVidaMaxima(): number;
  RecibirDamage(cantidad: number): void;
  
  // Ataques
  AtaqueSuave(): void;
  AtaqueFuerte(): void;
  ObtenerDamageSuave(): number;
  ObtenerDamageFuerte(): number;
  
  // Estado
  EstaVivo(): boolean;
  ObtenerEnfoque(): number;
  ModificarEnfoque(cantidad: number): void;
}
```

## Enumeraciones de Soporte

```typescript
enum FaseBoss {
  Fase1 = 1,
  Fase2 = 2,
  Fase3 = 3
}

enum TipoMinion {
  Facil = 1,
  Medio = 2,
  Dificil = 3
}

enum TipoPlayer {
  Estandar = 0,
  Personalizado = 1
}
```

## Comportamiento General

### Herencia e Implementación

- Todas las interfaces son contratos puros (sin implementación)
- Las clases concretas implementan las interfaces necesarias
- IBoss puede compositionar IAtaqueBoss para patrones de ataque específicos
- IMinion es implementado por MinionFacil, MinionMedio, MinionDificil

### Polimorfismo

- El sistema de combate referencia interfaces, no implementaciones concretas
- Permite agregar nuevos tipos de minions sin modificar código existente
- Facilita testing con mocks/stubs de las interfaces

### Extensibilidad

- Nuevas interfaces pueden agregar funcionalidad sin romper existentes
- Extension methods pueden agregar comportamiento adicional

## Requisitos Técnicos

- Lenguaje: TypeScript con strict mode
- Todas las interfaces deben estar en `src/gameplay/interfaces/`
- Documentación JSDoc/TSDoc para cada método
- Tests unitarios para cada implementación de interfaz
- Integración con el sistema de tipos de Unity (si aplica)
- Compatibilidad con el sistema de inyección de dependencias
