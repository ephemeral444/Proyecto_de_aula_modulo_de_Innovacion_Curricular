# 3_plan.md — Plan técnico de V2

## 1. Stack y arquitectura existente
- API: C# / ASP.NET Core sobre .NET 8.
- Base de datos: Microsoft SQL Server.
- Acceso a datos en la V1: Dapper y `Microsoft.Data.SqlClient`.
- Front incluido en el ZIP: `front/index.html` con HTML/CSS/JavaScript. La documentación histórica de V1 menciona Blazor, pero no es lo que contiene este ZIP; se trabajará con el código real hasta que el equipo acuerde formalmente otra cosa.
- Capas de API: `Controllers/`, `Servicios/`, `Repositorios/`, `Modelos/` y `Peticiones/`.

## 2. Estrategia
1. Obtener y revisar el esquema SQL oficial completo; mapear cada tabla, PK, FK, columna `activo` y nulabilidad.
2. Diseñar procedimientos almacenados para las 22 tablas, incluidos los siete recursos V1.
3. Adaptar los repositorios existentes para invocar procedimientos parametrizados sin romper contratos HTTP actuales.
4. Implementar las 15 tablas V2 en el patrón de capas de la API.
5. Traducir conflictos de integridad de SQL Server a HTTP 409 en español.
6. Implementar trigger y pruebas específicas.
7. Incorporar pantallas y componentes de detalle en el front actual; cargar las opciones FK desde la API.
8. Ejecutar pruebas de regresión, actualizar documentación y preparar entrega.

## 3. Estructura esperada de la especificación
```text
docs/spec_kit/versiones/v2_innovacion_curricular/
├── 2_spec.md
├── 3_plan.md
├── 4_research.md
├── 5_data_model.md
├── 6_contracts.md
├── 7_quickstart.md
├── 8_tasks.md
├── 9_checklist.md
└── GUIA_IA2.md
```
La constitución compartida permanece en `docs/spec_kit/1_constitution.md`; no se duplica.

## 4. Patrón de API por recurso
Por cada entidad se mantienen Controller → Servicio → Repositorio. Los controladores no contienen SQL. Los servicios aplican reglas de negocio, especialmente la regla de retirar un maestro con detalles activos. Los repositorios llaman procedimientos almacenados con parámetros y mapean sus resultados.

## 5. Patrón de interfaz
- Listado, consulta y formulario para cada recurso.
- Para cada FK: desplegable de la API, etiqueta legible y clave como valor enviado.
- Para maestros con detalles: panel de detalles dentro de la pantalla del maestro.
- Tablas puente: acción asignar/retirar, no editar una pareja existente.
- Mostrar mensajes en español; no mostrar trazas, nombres de procedimientos ni errores crudos del servidor.

## 6. Estrategia de integración Git
El estudiante A implementa la fase asignada en su rama actual; hace commit y la publica. El estudiante B integra esos commits mediante merge en su propia rama, prueba y desarrolla la siguiente fase. Se alternan de este modo. No se crea una rama adicional solo para esta fase y nadie hace commits directos a `main`. Antes del Pull Request se ejecuta la regresión completa.

## 7. Seguridad y configuración
Las cadenas de conexión y otros secretos se leen desde variables de entorno/configuración local no versionada. El `.env` no se sube; se mantiene un `.env.example` sin valores secretos. La autenticación y JWT siguen fuera de alcance (V3), pero no se deben introducir secretos en el código.
