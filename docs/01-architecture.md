# Arquitectura del Sistema

Utilizamos **Clean Architecture** dividida en 3 proyectos para garantizar el desacoplamiento:

``mermaid
graph TD;
    A[Inversor.Api] -->|Inyecta dependencias e invoca Casos de Uso| B(Inversor.Core);
    C[Inversor.Infrastructure] -->|Implementa interfaces de Core| B;
    style B fill:#2ecc71,stroke:#27ae60,stroke-width:2px; ``

### Reglas de Desarrollo

-   **Core (Application & Domain):** Es el corazón del sistema. NO puede tener referencias a Entity Framework, SQL, ni HTTP. Toda interacción externa se hace mediante abstracciones (ej. `IApplicationDbContext`).
-   **Domain Entities:** Nunca se deben instanciar usando `new Entity()`. Se debe utilizar exclusivamente el Factory Method estático `Create(...)` para proteger las invariantes.
-   **Infrastructure:** Contiene las implementaciones técnicas concretas. Aquí vive la configuración de EF Core (Fluent API) y los clientes HTTP externos (ej. Gemini/OpenAI).
-   **Casos de Uso (Use Cases):** Reemplazamos MediatR por inyección directa de Casos de Uso para mantener el pragmatismo y evitar burocracia, manteniendo el principio de Responsabilidad Única.