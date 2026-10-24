# Continuar TrackTrash

Abrir una terminal en esta carpeta y ejecutar `codex`. Prompt sugerido:

> Continúa TrackTrash. Lee docs/ESTADO_DESARROLLO.md, los README y AGENTS.md de ambos repositorios y backend/docs/MATRIZ_COBERTURA.md. Revisa git status, ramas y PR antes de modificar. Conserva repositorios separados y commits por cambios coherentes. No des por terminados los requisitos que requieren servicios externos o hardware.

Repos: `tracktrash-backend` y `tracktrash-mobile`. Trabajo actual en `feat/ecosistema-tracktrash`; el primer incremento ya está integrado en main. Los documentos originales de clase permanecen aquí.

Inicio backend:

```powershell
cd tracktrash-backend
docker compose up -d --wait
dotnet run --project src/TrackTrash.Api
```

Otra terminal para app:

```powershell
cd tracktrash-mobile
flutter run -d edge --web-port=5300 --dart-define=DEMO_MODE=true --dart-define=API_BASE_URL=http://localhost:5080
```

Si falta la configuración local de BD, usar primero scripts/dev-setup.ps1 del backend. Docker Desktop debe estar activo. No guardar secretos en documentos/Git. Las instrucciones completas y pendientes están en los README y en la matriz.
