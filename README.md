# 🌍 CrazyRisk

CrazyRisk es una implementación digital del clásico juego de mesa Risk, desarrollada en C# como parte del curso *Algoritmos y Estructuras de Datos I (CE1103)* en el Instituto Tecnológico de Costa Rica.  
El proyecto pone en práctica programación orientada a objetos, estructuras de datos personalizadas y comunicación cliente-servidor para recrear la experiencia del juego en computadora.

---

## 📋 Características principales

- 🎮 Modo 2 jugadores + ejército neutral (defiende territorios, no ataca).
- 🗺️ Mapa con 42 territorios y 6 continentes.
- ⚔️ Batallas con dados (máx. 3 atacantes vs 2 defensores).
- ♟️ Refuerzos dinámicos basados en:
  - Cantidad de territorios controlados.
  - Bonos por continente.
  - Intercambio de tarjetas (serie de Fibonacci global).
- 🔄 Turnos divididos en fases:
  1. Refuerzos
  2. Ataques
  3. Planeación (movimiento de tropas).
- 🌐 Arquitectura cliente-servidor (sockets TCP/IP).
- 🎨 Interfaz gráfica (UI) para visualizar el mapa, jugadores y resultados.
- 📚 Estructuras de datos propias:
  - LinkedList
  - Queue
  - Stack
  - HashMap<K,V>

---

## 🏗️ Arquitectura por capas

El proyecto sigue una arquitectura en tres capas:

1. 📡 Capa de Comunicación (Networking)  
   Manejo de sockets, envío/recepción de mensajes (JSON/XML) y sincronización cliente-servidor.

2. 🧠 Capa de Lógica (Core)  
   Reglas del juego, turnos, refuerzos, ataques, movimiento de tropas y condiciones de victoria.

3. 🎨 Capa de Presentación (UI)  
   Interfaz gráfica con menús, mapa interactivo, paneles de jugadores y chat opcional.

👉 Todos los diagramas UML se encuentran en la carpeta /docs.

---

## 📂 Estructura del repositorio
