# 2_spec.md — V2: tablas con claves foráneas

## 1. Propósito
Ampliar el módulo Innovación Curricular para gestionar facultades, programas académicos, acreditaciones, registros calificados, actividades académicas, pasantías, premios, relaciones docentes-programa, alianzas y las asociaciones entre programas y catálogos existentes, conservando la V1 operativa.

## 2. Alcance

### Incluye
- Los 15 recursos con claves foráneas enumerados en `5_data_model.md`.
- CRUD completo de las 22 tablas del módulo (7 de V1 + 15 de V2), sin incluir `usuario`, `rol` ni `rol_usuario` (V3).
- Todos los verbos CRUD implementados mediante procedimientos almacenados de SQL Server: listar, consultar, crear, actualizar y eliminar/retirar.
- Claves foráneas elegidas desde desplegables cargados mediante endpoints de la API; nunca se pide al usuario escribir una FK manualmente.
- Integridad referencial y respuesta HTTP 409 en español para FK inválidas, parejas duplicadas en tablas puente y conflictos referenciales de borrado físico.
- Relaciones maestro-detalle visibles y gestionables desde la pantalla del maestro. El envío de varios detalles pertenecientes a una misma operación debe ser atómico.
- Al menos un trigger funcional y verificable.
- Regresión de los criterios de aceptación de V1.
- Interfaz gráfica conectada a la API; no se considera terminada una fase si solo funciona la API.

### No incluye
- Login, JWT, autorización por roles ni CRUD de `usuario`, `rol` o `rol_usuario` (V3).
- Consultas multitabla de 4 o más tablas, dashboard, páginas corporativas, PWA ni publicación final (V4).
- Cambios de tecnología del front sin decisión explícita del equipo.

## 3. Requisitos funcionales

### RF01 — CRUD mediante procedimientos almacenados
Para cada tabla del módulo deben existir operaciones de listar, consultar por clave, crear, actualizar y eliminar/retirar. La capa C# solo invoca procedimientos con parámetros; no construye SQL CRUD concatenado ni escribe SELECT/INSERT/UPDATE/DELETE directamente en repositorios.

Convención propuesta: `sp_listar_<tabla>`, `sp_consultar_<tabla>`, `sp_crear_<tabla>`, `sp_actualizar_<tabla>`, `sp_eliminar_<tabla>`. Si el esquema exige claves compuestas, los procedimientos reciben todas las columnas de la clave.

### RF02 — Listados
Los listados excluyen filas inactivas cuando la tabla tenga columna `activo`. La forma de respuesta debe conservar el contrato existente de V1 salvo que una decisión de compatibilidad aprobada y documentada indique lo contrario.

### RF03 — Consulta individual
Una consulta por clave devuelve el registro activo; si no existe o está inactivo, devuelve 404 con mensaje en español.

### RF04 — Crear y actualizar
La API valida los campos obligatorios y delega la escritura al procedimiento correspondiente. Los procedimientos de creación y actualización deben devolver la fila como quedó guardada en la base de datos, incluyendo valores por defecto y valores calculados por triggers.

### RF05 — Borrado
Si la tabla tiene `activo`, el borrado es lógico (`activo = 0`) y los listados deben filtrar los inactivos. Si no tiene `activo`, se aplicará la regla definida en el esquema y en `5_data_model.md`. No se inventará borrado lógico para una tabla sin decidirlo en el spec.

### RF06 — Desplegables de claves foráneas
Cada campo FK de la interfaz se representa con un desplegable cargado desde un endpoint de la API. Se muestra una etiqueta legible (nombre o descripción) y se envía la clave real. Las opciones solo deben incluir padres activos cuando corresponda. Una FK opcional debe representar “(ninguna)” como `null`, no como cadena vacía.

### RF07 — Integridad referencial
Si SQL Server rechaza una operación por FK inexistente, PK compuesta duplicada o conflicto referencial, la API responde 409 con un mensaje en español y sin filtrar detalles internos de SQL Server. Los errores de validación de entrada no deben convertirse en 500.

### RF08 — Tablas puente
Las tablas puente se asignan y retiran usando las dos claves correspondientes. Una pareja ya existente no se edita: se informa el conflicto con 409. La interfaz no debe ofrecer una edición que contradiga esta regla.

### RF09 — Maestro-detalle
Al abrir un maestro, la pantalla muestra sus registros detalle y permite agregar o retirar filas sin obligar al usuario a volver a seleccionar el maestro. Cuando se envíen varios detalles como una sola operación de negocio, se guardarán en una transacción; si uno falla, ninguno queda parcialmente guardado.

### RF10 — Trigger
Debe implementarse al menos un disparador útil y comprobable, documentado en el script SQL. Se recomienda un sello de modificación o una regla derivada coherente con el esquema real. El trigger debe contemplar operaciones INSERT, UPDATE y DELETE cuando su lógica lo requiera; si el borrado es lógico, debe reaccionar al UPDATE.

### RF11 — Regresión V1
Las siete tablas de V1 continúan operativas desde la interfaz. Sus cinco operaciones CRUD deben pasar por procedimientos almacenados también en esta versión.

## 4. Requisitos no funcionales
- Mantener la arquitectura por capas ya presente en la API: Controller → Servicio → Repositorio.
- SQL Server es responsable de las consultas SQL dentro de los procedimientos almacenados.
- Usar parámetros tipados en llamadas desde C#; prohibido concatenar valores de usuario en SQL.
- Mensajes visibles en la interfaz en español y sin jerga HTTP/SQL (por ejemplo, no mostrar “FK”, “PUT”, “PATCH”, “409” o “500”).
- No incluir contraseñas, cadenas de conexión ni secretos en el código o en commits.
- Mantener compatibilidad con la V1 y probar compilación y funcionamiento de API y front.

## 5. Criterios de aceptación
1. Las siete tablas de V1 pasan nuevamente sus pruebas CRUD desde la interfaz.
2. Las quince tablas de V2 se pueden listar, consultar, crear, actualizar y retirar desde la interfaz.
3. Las operaciones CRUD de las 22 tablas invocan procedimientos almacenados; no queda SQL CRUD escrito directamente en repositorios.
4. Los procedimientos de creación/actualización devuelven la fila guardada cuando aplica.
5. Cada FK de la interfaz se selecciona mediante un desplegable alimentado por la API.
6. Las FK opcionales admiten “(ninguna)” y envían `null`.
7. Crear un hijo con un padre inexistente responde 409 en español, no 500.
8. Repetir una pareja de una tabla puente responde 409; retirar la pareja funciona con las dos claves.
9. El detalle aparece en la pantalla del maestro y se puede agregar desde allí.
10. Las operaciones de varios detalles se ejecutan atómicamente cuando formen una sola operación de negocio.
11. Al menos un trigger cambia el dato esperado sin que la API envíe directamente ese valor calculado.
12. Los listados excluyen registros inactivos en tablas con borrado lógico.
13. La interfaz no presenta códigos técnicos ni mensajes crudos del motor SQL.
14. La colección de pruebas incluye casos exitosos y casos que deben fallar.
15. Compilan la API y el front y se conserva la regresión V1.

## 6. Bloqueos y aclaraciones obligatorias antes de implementar
- El ZIP recibido contiene `db/innovacion_curricular.sql`, que define las siete tablas de V1, pero no contiene el script oficial completo `db_scripts/sqlserver/innovacion_curricular.ss.sql` mencionado en la consigna. No se deben inventar columnas, tipos, claves ni nulabilidad de las 15 tablas nuevas. Obtener el script oficial y usarlo como fuente de verdad antes de escribir DDL o modelos.
- El documento general exige dos repositorios (API y Front), mientras que el ZIP actual contiene `api_innovacion/` y `front/index.html` dentro de un mismo repositorio. Confirmar con el equipo/profesor si el front se separará en el repositorio independiente ya previsto o si la entrega actual es una excepción. No mover archivos en esta fase.
- El documento general menciona `docs/spec_kit/versiones/` y el molde V1 usa nueve documentos numerados del 2 al 9 más una guía IA. Esta carpeta conserva esa convención.
- Las tablas del módulo son 22 (7 V1 + 15 V2); las tres tablas de gestión de usuarios suman 25 en la base de datos total, pero pertenecen a V3 y quedan fuera de esta versión.

## 7. Definición de TERMINADA
Los 15 criterios de aceptación se verifican; la regresión de V1 pasa; la API y la interfaz están conectadas; el script de base de datos y los procedimientos están versionados; existe una colección de pruebas con errores esperados; la guía de IA y los prompts reales están guardados; el checklist está revisado y el tag `v2` se crea sobre el commit aprobado.
