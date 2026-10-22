# 4_research.md — Decisiones de diseño de V2

## D1 — Procedimientos almacenados para todas las operaciones CRUD
**Contexto:** la consigna de V2 exige que listar, consultar, crear, modificar y eliminar de todas las tablas pase por procedimientos almacenados.
**Decisión:** el repositorio C# llama procedimientos almacenados con parámetros; el SQL de SELECT/INSERT/UPDATE/DELETE y JOIN necesarios vive en SQL Server.
**Consecuencia:** hay que adaptar también los repositorios de las siete tablas V1, no solo crear recursos nuevos.
**Estado:** requerido por la consigna.

## D2 — Convención de nombres
**Decisión propuesta:** `sp_listar_<tabla>`, `sp_consultar_<tabla>`, `sp_crear_<tabla>`, `sp_actualizar_<tabla>`, `sp_eliminar_<tabla>`. Los nombres se mantendrán estables y documentados en el script SQL. Cualquier excepción se justifica aquí.
**Estado:** propuesta para aprobación del equipo.

## D3 — Borrado lógico condicionado al esquema
**Contexto:** no todas las tablas de todos los módulos tienen `activo`; el documento del profesor indica que el esquema real determina el tipo de borrado.
**Decisión:** las tablas con `activo` usan `UPDATE ... SET activo = 0` y los listados filtran `activo = 1`. Para las que no tengan esa columna, se define explícitamente si procede borrado físico y cómo se responde ante hijos existentes. No se agrega `activo` automáticamente a ciegas.
**Estado:** pendiente de validar contra el SQL oficial.

## D4 — Conflictos referenciales como HTTP 409
**Decisión:** traducir errores de FK y PK duplicada que correspondan a conflicto de negocio a HTTP 409 con mensaje español. Los errores inesperados siguen siendo errores de servidor y se registran sin mostrar detalles internos al usuario.
**Estado:** requerido por la consigna.

## D5 — Claves foráneas mediante desplegables
**Decisión:** el front obtiene opciones de la API, muestra nombres y envía claves. Las FK opcionales envían `null`. Los catálogos usados como padres deben excluir inactivos cuando exista borrado lógico.
**Estado:** requerido por la consigna.

## D6 — Maestro-detalle y atomicidad
**Decisión:** el detalle se administra dentro de la pantalla del maestro. Si una operación incluye varios renglones que constituyen una sola operación de negocio, se procesa en una transacción para evitar guardar parcialmente el conjunto.
**Estado:** requerido por la consigna; los pares maestro-detalle están en `5_data_model.md`.

## D7 — Trigger
**Alternativas:** sello de modificación, bitácora de retiros o estado derivado, según columnas disponibles.
**Decisión provisional:** escoger después de revisar el esquema real. No crear una columna ni trigger que dependa de una columna no confirmada. Si el borrado es lógico, probar que el trigger escuche UPDATE.
**Estado:** pendiente del SQL oficial.

## D8 — Dos repositorios frente al ZIP recibido
**Contexto:** la consigna general exige repositorio privado de API y repositorio privado de Front; el ZIP recibido contiene una carpeta `front/` junto con la API.
**Decisión:** la especificación se mantiene en el repositorio de la API como fuente de verdad, siguiendo la consigna. Antes de modificar estructura o Compose, el equipo debe confirmar dónde está el repositorio real del front y su ruta local. Esta fase no mueve archivos.
**Estado:** pendiente de confirmación.

## D9 — Esquema oficial ausente en el ZIP
**Contexto:** la consigna referencia `db_scripts/sqlserver/innovacion_curricular.ss.sql`, pero el ZIP recibido solo incluye `db/innovacion_curricular.sql` con las siete tablas V1.
**Decisión:** no inventar columnas ni claves de las tablas V2. Conseguir el script oficial y completar `5_data_model.md` antes de generar DDL, modelos o procedimientos.
**Estado:** bloqueante para implementación de base de datos.
