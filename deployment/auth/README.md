# Redirecciones de Supabase Auth

Proyecto del piloto: `owfjolparskdiazhfubr`. Configuración pública, sin secretos, limitada a la URL del sitio y las URL de retorno exactas. Las migraciones EF siguen controlando el esquema SQL.

Se aplicó con Supabase CLI 2.122.0. Esta versión solo actualiza propiedades declaradas; revisar siempre el diff antes de aplicar y comprobar que solo cambian las dos propiedades esperadas de Auth:

```powershell
npx --yes supabase@2.122.0 config diff --project-ref owfjolparskdiazhfubr --workdir deployment/auth
npx --yes supabase@2.122.0 config push --project-ref owfjolparskdiazhfubr --workdir deployment/auth --yes
```

`site_url` debe ser `https://tracktrash.pages.dev`, con retorno exacto al mismo sitio, con o sin `/` final. No se permiten dominios de preview arbitrarios ni comodines. [Referencia oficial](https://supabase.com/docs/guides/auth/redirect-urls).

La corrección evita el valor inicial `http://localhost:3000` en la confirmación de registro. No cambia contraseñas, usuarios existentes ni la exigencia de confirmar el correo. Es una configuración del servicio: no requiere recompilar web/APK ni reiniciar Docker local.

Validación realizada: `config diff` sin cambios pendientes en las propiedades declaradas; enlace de registro generado para una identidad sintética, retorno HTTP a la web pública, cuenta confirmada y login posterior con contraseña aprobado. La identidad de prueba se eliminó después. No se enviaron correos ni se consumieron enlaces de usuarios reales durante esta prueba.

Esto comprueba el destino y la confirmación en Auth; SMTP propio, entrega masiva, recuperación completa y retorno directo por enlace profundo a Android siguen pendientes. Después de confirmar desde el navegador se puede volver a la APK para iniciar sesión con la cuenta creada.
