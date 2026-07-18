# Inversor Language - AI Grammar Evaluator

Motor de aprendizaje adaptativo de idiomas impulsado por LLMs (IA) y Repetición Espaciada (SRS). 
El sistema evalúa entradas de texto libre, detecta aciertos/errores gramaticales de forma atómica y adapta el intervalo de revisión según el rendimiento cognitivo del usuario.

## 🏗️ Stack Tecnológico
* **Backend:** .NET 10 (C# 13)
* **Base de datos:** PostgreSQL 15
* **ORM:** Entity Framework Core 10 (Code-First)
* **Arquitectura:** Clean Architecture + Domain-Driven Design (DDD)
* **Infraestructura:** Docker & Docker Compose

## Cómo iniciar el entorno local

El proyecto utiliza Docker Compose para replicar el entorno de producción, incluyendo un contenedor efímero para migraciones.

1. Clona el repositorio.
2. Asegúrate de tener configurado tu archivo `.env` en la raíz (ver `.env.example`).
3. Ejecuta el entorno:
   ``bash
   docker compose up --build
``
4. El contenedor inversor-migrator aplicará las migraciones de EF Core automáticamente.
5. Una vez encendida la API, visita http://localhost:5001/swagger/index.html para ver la documentación de los endpoints.

