# Inversor Project - System Architecture, Business Rules & SRE Guidelines

## 1. Technology Stack
- **Backend Framework:** .NET 10 (Web API + Background Worker Service).
- **Database & ORM:** PostgreSQL 15 + Entity Framework Core (Code-First Migrations).
- **Asynchronous Messaging:** RabbitMQ 3.x + MassTransit (AMQP Transport).
- **AI Integration:** Google Gemini API (`Google.GenAI` SDK).
- **Authentication:** External BaaS (Firebase Auth / Supabase Auth) validated via JWT Bearer tokens.
- **Real-Time Push Notifications:** SignalR (WebSockets).
- **Observability:** OpenTelemetry (Distributed Tracing across API, RabbitMQ, and Worker).

---

## 2. Business Tier Requirements

### A. Guest User (No Account)
- **Flow:** Ephemeral processing.
- **Persistence:** MUST NOT save `TranslationSubmission`, tags, or metrics in PostgreSQL.
- **Security & FinOps:** Restricted via IP Rate Limiting + Cloudflare Turnstile bot protection.

### B. Free Tier User
- **Flow:** Full Asynchronous Request-Reply pattern.
- **Persistence:** Full DB tracking (`TranslationSubmission`, `SubmitTag`, `TopicMastery`).
- **Limits:** Daily quota enforcement (e.g., 10-20 evaluations/day).

### C. Premium Tier User (PRACTICE Mode & SRS)
- **Flow:** Advanced learning loop using Spaced Repetition System (`Submission.Mode = PRACTICE`).
- **Feature:** Analyzes `TopicMastery` weak points to generate tailored practice sentences in the user's native language.
- **Review Scheduling:** Calculates and stores `NextReviewDate` for spaced review.

---

## 3. Strict Architectural Rules

1. **Asynchronous Request-Reply Pattern:**
   - All evaluation requests MUST originate in `Inversor.Api` with `Status = Pending`.
   - The API publishes `EvaluateTranslationCommand` to RabbitMQ via MassTransit and returns `HTTP 202 Accepted` immediately (<50ms) with a lightweight payload (`SubmissionId`, `Status`, `CreatedAt`).
   - Long-running LLM execution occurs asynchronously in `Inversor.Worker`.

2. **Dynamic Configuration (`IOptions<T>` Pattern):**
   - HARDCODING AI models, connection strings, broker credentials, or timeouts is STRICTLY FORBIDDEN.
   - All infrastructure settings MUST use strongly-typed `IOptions<T>` classes bound via `BindConfiguration(...)` with `.ValidateDataAnnotations()` and `.ValidateOnStart()`.
   - Environment variables (`Gemini__*`, `RabbitMQ__*`) take precedence at runtime via `.env` / `docker-compose.yml`.

3. **Domain-Driven Design (DDD) & Invariants:**
   - Entities (`TranslationSubmission`, `TopicMastery`, etc.) MUST use private setters.
   - Instantiation MUST go through Factory Methods (`Create(...)`).
   - State transitions MUST use explicit domain methods (`MarkAsProcessing()`, `MarkAsCompleted(...)`, `MarkAsFailed(...)`).

4. **Resilience & SRE Best Practices:**
   - **Worker LLM Calls:** MUST wrap HTTP calls to Gemini using Polly resilience policies (Exponential Backoff, Jitter, Circuit Breaker).
   - **Dead-Letter Queues (DLQ):** Unrecoverable message failures MUST route to MassTransit `_error` queues.
   - **Idempotency:** Consumers MUST verify entity state before processing to prevent duplicate LLM calls or charges.
   - **Transactional Outbox:** Database mutations and message dispatch MUST be atomic.

<!-- 5. **Code Style & Agent Rules (NO VIBECODING):**
   - **Language:** All code comments, documentation, and XML docstrings MUST be in English.
   - **Agent Execution:** AI agents MUST NEVER modify or write files directly on the system. All code blocks must be provided in chat for manual inspection and implementation. -->
