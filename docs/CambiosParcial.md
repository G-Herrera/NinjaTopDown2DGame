# Documento de Cambios – Tercer Parcial

## Descripción general

Durante el tercer parcial se implementaron nuevas mecánicas enfocadas en mejorar el dinamismo del juego, incrementar la dificultad y enriquecer la experiencia del jugador.

---

## Mecánicas añadidas

### 1. Enemigos tipo Chaser

Enemigos que persiguen constantemente al jugador.  
No requieren detección previa.  
Se generan dinámicamente en el mapa.

**Impacto:**
- Aumenta la presión constante.  
- Elimina zonas seguras.  

---

### 2. Sistema de disparo (Ninja Stars)

El jugador puede lanzar proyectiles.  
La dirección del disparo está basada en el input del jugador.  
Permite eliminar enemigos.

**Impacto:**
- Introduce una mecánica activa de defensa.  
- Mejora la interacción jugador–entorno.  

---

### 3. Sistema de flechas (ArrowSpawner)

Las flechas aparecen de manera aleatoria en el mapa.  
Se dirigen hacia el jugador.  
Funcionan como trampas dinámicas.

**Impacto:**
- Incrementa la dificultad.  
- Obliga al jugador a mantenerse en movimiento.  

---

## Scripts nuevos creados

- ChaserEnemy.cs  
- NinjaStar.cs  
- Arrow.cs  
- ArrowSpawner.cs  

---

## Scripts modificados

**PlayerStateManager**  
Se añadió el sistema de disparo.

**SpawnManager**  
Se mejoró el control de la cantidad de enemigos.

**PlayerHealth**  
Se integraron nuevas fuentes de daño.

---

## Problemas encontrados

- Dirección incorrecta en vectores de movimiento.  
- Configuración del sistema de Input (Input System vs Legacy).  
- Control de instancias activas en el spawner.  
- Colisiones no detectadas correctamente por falta de configuración de triggers.  

---

## Decisiones importantes

- Uso de `.normalized` para mantener velocidad constante.  
- Implementación de FSM para el comportamiento de enemigos.  
- Separación de responsabilidades en scripts.  
- Uso de comunicación simple entre sistemas (por ejemplo, enemigo–spawner).  

---

## Resultados obtenidos

- Juego más dinámico y desafiante.  
- Integración de múltiples sistemas interactivos.  
- Mejor comprensión de arquitectura en Unity.  
- Implementación funcional de combate y amenazas múltiples.  

---

## Conclusión

El tercer parcial permitió expandir significativamente el proyecto, pasando de un sistema base a un juego con múltiples mecánicas activas que interactúan entre sí de forma coherente.