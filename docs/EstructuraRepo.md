# Documento de Organización del Repositorio

## Descripción general

El repositorio está estructurado siguiendo una organización típica de proyectos en Unity, separando los recursos por tipo y funcionalidad para facilitar su mantenimiento y escalabilidad.

---

## Estructura principal

### Assets/

Contiene todos los recursos del proyecto.

---

### Scripts/

Contiene la lógica principal del juego.  
Incluye controladores del jugador, enemigos, spawners y sistemas generales.

---

### Scenes/

Contiene las escenas del juego.  
Incluye la escena principal de gameplay.

---

### Prefabs/

Contiene objetos reutilizables del proyecto.  
Ejemplos: enemigos, proyectiles, elementos de UI.

---

### Sprites/

Contiene los recursos gráficos utilizados en el juego.

---

### UI/

Incluye elementos de interfaz como:
- HUD  
- Paneles  
- Pantallas de Game Over  

---

### Managers/

Contiene scripts globales del juego, como el **GameFlowManager**, encargado de controlar los estados principales del sistema.

---

### docs/

Contiene la documentación del proyecto.  
Incluye:

- Documento de arquitectura  
- Documento de cambios del parcial  
- Documento de organización del repositorio  

---

## Flujo de trabajo del repositorio

- Uso de ramas para el desarrollo de nuevas funcionalidades (features).  
- Integración de cambios a la rama principal (`main`) mediante merge.  
- Uso de commits descriptivos para mantener claridad en el historial.  
- Eliminación de ramas después de su integración.  

---

## Buenas prácticas implementadas

- Separación de responsabilidades en los scripts.  
- Uso de prefabs para reutilización de objetos.  
- Modularidad en la arquitectura del proyecto.  
- Uso de control de versiones con Git.  

---

## Conclusión

La estructura del repositorio permite una organización clara y escalable, facilitando el mantenimiento del proyecto y el trabajo colaborativo.