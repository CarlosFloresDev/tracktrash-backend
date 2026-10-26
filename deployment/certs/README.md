# CA pública de Supabase

`supabase-root-2021.crt` se obtuvo de la [distribución oficial](https://supabase-downloads.s3-ap-southeast-1.amazonaws.com/prod/ssl/prod-ca-2021.crt), siguiendo la [documentación de conexiones TLS](https://supabase.com/docs/guides/database/connecting-to-postgres).

Supabase Root 2021 CA, vigencia 28/04/2021–26/04/2031. SHA-256:

```text
80:70:25:AD:50:D4:ED:21:9D:2C:9C:7D:29:9C:00:4F:82:4E:B0:0C:F7:F6:5A:FE:F6:07:D0:7B:72:E6:CA:FA
```

Es un certificado público, sin clave privada. El contenedor lo incluye en `/app/certs/`; Npgsql lo usa con `SSL Mode=VerifyFull`, comprobando también el hostname. Revisar la distribución oficial ante una rotación, sin desactivar la validación.
