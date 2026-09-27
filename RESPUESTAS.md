# Sustentación Teórica y Arquitectónica — Módulo de Innovación Curricular (v1)

### 1. ¿Por qué se implementa borrado lógico en lugar de borrado físico?
El borrado lógico mediante el atributo `activo BIT DEFAULT 1` garantiza la integridad referencial histórica y la trazabilidad de los datos institucionales. En sistemas curriculares y académicos, los registros nunca deben destruirse físicamente, ya que su desaparición generaría inconsistencias en auditorías y reportes históricos.

### 2. ¿Por qué las consultas SQL deben ser estrictamente parametrizadas?
El uso de consultas parametrizadas a través de `Microsoft.Data.SqlClient` y Dapper previene ataques de inyección SQL (SQL Injection), asegurando que cualquier dato suministrado por el cliente sea tratado como un literal y no como código ejecutable por el motor de base de datos.

### 3. ¿Cómo se garantiza el desacoplamiento entre las capas de Servicio y Presentación (API)?
La capa de Servicios desconoce por completo el protocolo HTTP: no utiliza tipos como `IActionResult`, `StatusCodes` ni referencias a ASP.NET Core. Ante la ausencia o inactividad de un recurso, la capa de servicio lanza una excepción de negocio pura (`NoEncontradoExcepcion`), delegando al controlador la responsabilidad de traducir dicha situación a un código de respuesta HTTP `404 Not Found`.

### 4. ¿Cuál es la diferencia semántica entre las peticiones PUT y PATCH en el módulo?
- **PUT (Reemplazo Total):** Exige recibir el cuerpo completo con todas las propiedades obligatorias (`*Reemplazo.cs`). Si falta algún campo obligatorio, el controlador retorna `422 Unprocessable Entity`.
- **PATCH (Actualización Parcial):** Permite recibir únicamente los atributos que se desean modificar a través de propiedades opcionales/anulables (`*Actualizar.cs`), preservando los valores existentes del registro.

### 5. ¿Cómo se logró la resiliencia del Front-End ante fallos del Backend?
La interfaz web en `front/index.html` captura las excepciones de red (`fetch catch`) y los códigos de error HTTP de forma controlada. Si el backend en el puerto 8124 se encuentra fuera de servicio, la interfaz no se rompe ni colapsa, mostrando un mensaje visible en español que informa la indisponibilidad de la API.