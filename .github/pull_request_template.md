## Qué cambia

<!-- Resumen breve. Enlaza el issue: Closes #N -->

## Paquete

- [ ] P0 Carlos  - [ ] P1 Integrante 1  - [ ] P2 Integrante 2  - [ ] P3 Integrante 3

## Checklist de arquitectura

- [ ] Cada caso de uso está en `UseCases/<Entidad>/Commands|Queries/<Accion>/` con Command o Query, Handler y Validator (o Dto).
- [ ] Commands y Queries inmutables (`init`); Handlers y Validators `internal sealed`.
- [ ] Sin lógica de negocio en controllers ni en Infrastructure; las reglas de negocio viven en el dominio.
- [ ] Contratos HTTP en `Api/Contracts`; los controllers solo llaman a `ISender.Send`.
- [ ] Repositorios por agregado; las queries proyectan a DTO.
- [ ] Aplicadas las correcciones de `docs/REVISION_ARQUITECTURA.md` que tocan a este paquete.

## Pruebas

- [ ] `dotnet build` sin warnings.
- [ ] `dotnet test --filter "Categoria!=Integracion"` en verde.
- [ ] Integración con PostgreSQL en verde (o explica por qué no se ejecutó).
- [ ] Una sola migración, generada después del rebase sobre `main`.
