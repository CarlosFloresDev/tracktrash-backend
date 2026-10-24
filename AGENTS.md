# Desarrollo de TrackTrash

- Leer README, docs/REQUISITOS_REVISADOS.md y docs/MATRIZ_COBERTURA.md antes de ampliar alcance.
- Domain no depende de EF ni ASP.NET. Repositorios e IUnitOfWork son contratos de Domain; casos de uso y DTOs están en Application; implementaciones en Infrastructure; host/controllers en Api.
- Mantener las reglas de permisos, privacidad, idempotencia y concurrencia en el servidor, con transacciones para cambios de saldo/stock/capacidad.
- Un caso de uso concreto por operación, con ExecuteAsync. Evitar repositorios genéricos y capas sin responsabilidad.
- Cambios de esquema requieren migración EF y pruebas PostgreSQL/PostGIS. Nunca ejecutar las pruebas que crean/borran bases contra un proyecto remoto o producción.
- No guardar secretos ni credenciales reales. No presentar datos demo, reportes ciudadanos ni cantidades estimadas como mediciones verificadas.
- Ejecutar scripts/test.ps1 y compilación Release para cambios relevantes; crear commits por unidades coherentes y conservar contratos y matriz de cobertura actualizados.
