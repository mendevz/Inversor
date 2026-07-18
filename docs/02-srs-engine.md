# Motor de Repetición Espaciada (SRS)

El núcleo de retención de la aplicación vive en la entidad `TopicMastery`. Utiliza una adaptación del algoritmo científico **SuperMemo-2 (SM-2)** para calcular cuándo el usuario está a punto de olvidar un tema gramatical.

## Variables Clave
* **`MasteryScore`:** Porcentaje visual de dominio (0.0 a 1.0) para mostrar al usuario.
* **`ConsecutiveSuccesses`:** Racha actual de aciertos.
* **`EasinessFactor` (EF):** Multiplicador de dificultad. Inicia en 2.5. Sube si el usuario acierta (tema fácil), baja si el usuario falla (tema difícil).
* **`CurrentIntervalDays`:** Días que deben pasar hasta la próxima revisión.

## Lógica Matemática
Cuando un usuario es evaluado en un concepto gramatical:

1. **Si Falla (`isError = true`):**
   * La racha vuelve a 0.
   * El intervalo vuelve a 1 día (forzando repaso inmediato).
   * El $EF$ recibe una penalización para que los futuros intervalos crezcan más lento.

2. **Si Acierta (`isError = false`):**
   * La racha incrementa.
   * El $EF$ sube ligeramente.
   * El nuevo intervalo crece de forma exponencial según la fórmula:
   
   $$I_n = \text{Round}(I_{n-1} \times EF)$$

Esta matemática garantiza que los temas dominados desaparezcan del dashboard por meses, mientras que las debilidades se repasen constantemente.