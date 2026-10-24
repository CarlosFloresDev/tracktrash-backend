# Estado del desarrollo

Primer incremento: solicitud → aceptación → confirmación → puntos, conectado a Flutter.

Implementado: cuatro capas, repositorios, LINQ, UnitOfWork, middleware, migración, PostgreSQL local, demostración por roles, API y pruebas automatizadas. La integración de identidad Supabase está preparada mediante JWT/JWKS; falta validarla en el proyecto remoto.

Próximos incrementos:

1. Elegir identidad definitiva del curso y conectar registro/login reales, perfil y verificación de recicladores.
2. Registrar aliados y materiales/zona/capacidad; asignación y reglas de operación.
3. Añadir mapa, PostGIS, contenedores, incidencias y fotos en Supabase Storage.
4. Cancelación y seguimiento ampliado, eventos pendientes y notificaciones push.
5. Integración ESP32/simulador, antigüedad de datos y pruebas físicas.
6. Panel operativo, organizaciones y permisos por institución.

La regla inicial otorga 20 puntos por entrega, sin inferir peso verificado ni impacto ambiental. Las recompensas y su validación se revisan antes del piloto.

Las estimaciones del plan son orientativas; no se han implementado todos los RF/RNF del informe.
