# Documento de Arquitectura del Proyecto

## Descripción general

El proyecto consiste en un videojuego 2D top-down desarrollado en Unity, basado en un sistema modular donde cada componente del juego está controlado por scripts especializados.

La arquitectura está centrada en la separación de responsabilidades, permitiendo que cada sistema (jugador, enemigos, UI, spawners) funcione de manera independiente pero coordinada.

---

## Sistemas principales

### 1. Sistema del jugador

**PlayerStateManager**  
Controla movimiento, input y animaciones.

**PlayerHealth**  
Gestiona vida, daño, animaciones de impacto y muerte.

---

### 2. Sistema de enemigos

**EnemyStateManager2D**  
Implementa FSM (Patrol, Chase, Attack).

**ChaserEnemy**  
Enemigos dinámicos que persiguen constantemente al jugador.

---

### 3. Sistema de combate

**NinjaStar**  
Proyectiles del jugador. Detectan colisiones y aplican daño.

**Arrow**  
Proyectiles enemigos que dañan al jugador.

---

### 4. Sistema de spawn

**SpawnManager**  
Controla la aparición de enemigos con un límite máximo.

**ArrowSpawner**  
Genera flechas de manera aleatoria dirigidas al jugador.

---

### 5. Sistema de flujo del juego

**GameFlowManager**  
Controla estados globales como gameplay, pausa y Game Over.

---

## Flujo general del juego

1. El jugador se mueve libremente por el mapa.  
2. Los enemigos aparecen mediante el sistema de spawn.  
3. Los enemigos persiguen o atacan al jugador.  
4. El jugador puede defenderse usando proyectiles.  
5. Las flechas aparecen como amenazas adicionales.  
6. El jugador recibe daño mediante PlayerHealth.  
7. Si la vida llega a 0, se activa el estado de Game Over.  

---

## Escenas principales

- Escena principal de gameplay (núcleo del juego).  
- Escenas o paneles de UI (Game Over, HUD).  

---

## Interacción entre componentes

**PlayerStateManager → NinjaStar**  
Genera proyectiles del jugador.

**NinjaStar → ChaserEnemy**  
Aplica daño a enemigos.

**Arrow → PlayerHealth**  
Aplica daño al jugador.

**SpawnManager → ChaserEnemy**  
Instancia enemigos.

**ArrowSpawner → Arrow**  
Instancia flechas dirigidas al jugador.

**GameFlowManager**  
Coordina los estados globales del juego.

---

## Conclusión

La arquitectura del proyecto sigue un enfoque modular, facilitando la escalabilidad y el mantenimiento del código. Cada sistema está desacoplado, permitiendo agregar nuevas mecánicas sin afectar significativamente las existentes.