# Proyecto Integrador. Juego 2D Top-Down "NinjaQuest"

## 1. Descripción general del juego

**NINJA QUEST** es un videojuego 2D con vista top-down desarrollado en Unity, donde el jugador controla a un ninja que debe recolectar objetos para ganar, sobreviviendo el mayor tiempo posible mientras evita amenazas constantes en el entorno.

El objetivo principal del jugador es mantenerse con vida intentando conseguir todos los objetos a recoletar, esquivando enemigos y proyectiles mientras utiliza habilidades de ataque para defenderse. El juego busca ofrecer una experiencia dinámica, con presión constante, toma de decisiones rápidas y control preciso del personaje.

## 2. Objetivo del proyecto

El propósito de este proyecto es aplicar los conocimientos adquiridos durante el curso en el desarrollo de un videojuego funcional en Unity, integrando:

- Programación en C#
- Arquitectura basada en scripts
- Implementación de sistemas interactivos
- Uso de máquinas de estados (FSM)
- Integración de nuevas mecánicas sobre un proyecto base

Además, se busca reforzar el uso de herramientas de control de versiones como Git y GitHub dentro de un flujo de trabajo profesional.

## 3. Mecánicas principales del juego
- **Movimiento del jugador**
  
El jugador puede desplazarse en las cuatro direcciones usando teclado, con control fluido y normalización de velocidad.
- **Sistema de ataque**

El jugador tiene la habilidad de disparar estrellas ninja al puslar una tecla. Permite instanciar objetos y defenderse de enemigos.
- **Sistema de enemigos (FSM)**
  
Enemigos con comportamiento basado en estados: Patrol, Chase y Attack.
- **Sistema de vida (PlayerHealth)**
  
El jugador tiene puntos de vida, recibe daño y puede morir, activando animaciones y estado de Game Over.
- **Sistema de ataque enemigo**

Los enemigos pueden dañar al jugador al entrar en rango.
- **Sistema de score**
  
Se lleva un registro del desempeño del jugador.
- **Coleccionables**
  
Elementos dentro del mapa que el jugador puede recoger.
- **Sistema de victoria y derrota**
  
El juego termina al cumplir condiciones específicas o al morir.
- **UI y paneles**
  
Interfaz que muestra información relevante como vida y estado del juego.
- **Cámara con freeze**
La cámara se congela en eventos importantes como el Game Over.

## 4. Mecánicas o mejoras implementadas en este parcial

Durante este parcial se implementaron tres nuevas mecánicas que incrementan la dificultad y dinamismo del juego:

**1. Enemigos tipo “Chaser”**

Se añadieron enemigos que aparecen de forma dinámica y persiguen constantemente al jugador sin necesidad de detección previa.
Mejora: incrementan la presión sobre el jugador y reducen zonas seguras.

**2. Sistema de disparo (estrellas ninja)**

El jugador puede lanzar proyectiles en la dirección de movimiento para eliminar enemigos.
Mejora: introduce una mecánica activa de defensa y aumenta la interacción del jugador con el entorno.

**3. Sistema de flechas (trampas dinámicas)**

Se implementó un sistema donde flechas aparecen aleatoriamente en el mapa y se dirigen hacia el jugador.
Mejora: añade amenazas externas constantes, obligando al jugador a mantenerse en movimiento.

## 5. Tecnologías utilizadas
- **Unity:** Motor principal para el desarrollo del videojuego.
- **C#:** Lenguaje de programación utilizado para la lógica del juego.
- **Visual Studio:** Entorno de desarrollo para escribir y depurar código.
- **Git:** Control de versiones para gestionar cambios en el proyecto.
- **GitHub:** Plataforma para alojar el repositorio y colaborar en equipo.

## 6. Instrucciones básicas de ejecución
1. Abrir el proyecto en Unity (versión recomendada: [6000.2.6f2 o posterior]).
2. Cargar la escena principal del juego (Menu).
3. Presionar el botón Play en el editor.
4. Controles:
    + Movimiento: WASD
    + Ataque: Barra espaciadora
  
## 7. Scripts o sistemas principales
+ **PlayerStateManager**
  
Controla el movimiento del jugador, entrada de usuario y animaciones.
+ **PlayerHealth**

Gestiona la vida del jugador, daño recibido y muerte.
+ **EnemyStateManager2D**

Implementa la máquina de estados del enemigo principal.
- **ChaserEnemy**

Controla enemigos que persiguen directamente al jugador.
+ **SpawnManager**

Administra la aparición de enemigos en el mapa.
+ **ArrowSpawner**

Genera flechas que atacan al jugador desde distintas posiciones.
+ **NinjaStar**

Maneja el comportamiento de los proyectiles del jugador.
+ **GameFlowManager**

Controla los estados generales del juego (gameplay, game over, etc.).

## 8. Capturas o evidencias visuales


<img width="1151" height="646" alt="Screenshot 2026-04-14 120105" src="https://github.com/user-attachments/assets/bb5ac35c-8533-45cd-ae23-928a671fd2ef" />
<img width="1143" height="644" alt="Screenshot 2026-04-14 120116" src="https://github.com/user-attachments/assets/1519a3a5-6450-4191-829d-b2d255831b2f" />
<img width="1149" height="647" alt="Screenshot 2026-04-14 120127" src="https://github.com/user-attachments/assets/4acc1535-6bb5-48cb-9a44-3c7b5d952390" />

## 9. Créditos e integrantes
Gerardo Humberto Herrera Quiroz
Saul Eduardo Gonzales Vargas

Materia: Topicos Avanzados de Programación
Docente: Francisco Emiliano Aguayo Serrano

## 10. Estado actual del proyecto o conclusiones

El proyecto se encuentra en un estado funcional, con un loop jugable completo que incluye movimiento, combate, enemigos dinámicos y sistemas de daño.

Las mecánicas implementadas en este parcial mejoran significativamente la experiencia del jugador, aumentando la dificultad y el dinamismo del juego.

Como trabajo futuro, se podrían implementar mejoras como:

+ Balance de dificultad
+ Mejora en animaciones
+ Sistema de progresión
+ Optimización mediante object pooling



