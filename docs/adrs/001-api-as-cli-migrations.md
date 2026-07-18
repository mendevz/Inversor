# ADR 001: Estrategia de Migraciones con "API as a CLI"

* **Fecha:** Julio 2026
* **Estado:** Aceptado

## Contexto
En un entorno con múltiples contenedores (Docker/K8s), ejecutar `context.Database.MigrateAsync()` al arrancar la API causa colisiones, bloqueos en base de datos e inconsistencias si varias instancias de la API arrancan simultáneamente. Generar *bundles* manuales para entornos locales rompe la automatización.

## Decisión
Adoptamos el patrón **API as a CLI**. La propia imagen de la API contiene un flag `--only-migrate` en su `Program.cs`. 
En el `docker-compose.yml`, orquestamos un contenedor efímero (`inversor-migrator`) que ejecuta este flag, aplica las migraciones de forma aislada y se apaga. La API principal arranca condicionada a que este contenedor termine exitosamente (`service_completed_successfully`).

## Consecuencias
* **Positivas:** Paridad del 100% entre Desarrollo y Producción. Cero colisiones de base de datos. Se reutiliza la misma imagen de Docker ahorrando espacio y tiempos de compilación.
* **Negativas:** Requiere comprender la orquestación de dependencias en Docker Compose, en lugar de depender del comportamiento por defecto de EF Core.