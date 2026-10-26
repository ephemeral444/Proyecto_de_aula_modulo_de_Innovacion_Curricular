# 8_tasks.md — Plan de tareas V2: CRUD de tablas con claves foráneas

## 1. Objetivo de la V2

El objetivo central de la V2 es **completar el CRUD de las 15 tablas que tienen claves foráneas (FK)**, conectando base de datos, procedimientos almacenados, API por capas e interfaz web. La V2 **no se limita** a `facultad` y `programa`: esas dos entidades son solo el inicio de la jerarquía de datos.

El alcance se toma del proyecto existente, en particular de `db/v2_innovacion_curricular.sql`, los controladores y servicios en `api_innovacion/`, y la configuración de módulos en `front/index.html`. Si el SQL oficial del curso difiere del script incluido, se registra la discrepancia antes de cambiar modelos o procedimientos; no se inventan columnas, tipos ni claves.

### Tablas incluidas en la V2

| Grupo | Tablas | Funcionalidad requerida |
|---|---|---|
| Jerarquía académica | `facultad`, `programa` | Listar, consultar por clave, crear, actualizar y eliminar/inactivar según el esquema. |
| Recursos vinculados a un programa | `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento` | CRUD completo, selección de la FK de programa y consulta por clave. |
| Relación de aliado y programa | `alianza` | CRUD completo, selector para aliado y programa. |
| Tablas puente | `programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc`, `aa_rc` | Consultar/listar relaciones, asignar y retirar. Por tener clave primaria compuesta, no se edita una pareja existente; se retira y se crea la nueva pareja. |

**Total: 15 tablas nuevas/con FK en el alcance funcional de V2.** Las siete tablas de V1 (`universidad`, `aliado`, `area_conocimiento`, `practica_estrategia`, `car_innovacion`, `aspecto_normativo`, `enfoque`) deben seguir operativas porque proporcionan registros para los desplegables y porque forman parte de las pruebas de regresión. No se deben sustituir por datos escritos manualmente en formularios.

## 2. Relaciones que deben reflejarse en API y frontend

| Tabla V2 | FK o dependencia según el SQL del proyecto |
|---|---|
| `facultad` | `universidad` |
| `programa` | `facultad` |
| `acreditacion` | `programa` |
| `registro_calificado` | `programa` |
| `activ_academica` | `programa` |
| `pasantia` | `programa` |
| `premio` | `programa` |
| `docente_departamento` | `programa` |
| `alianza` | `aliado`, `programa` |
| `programa_ac` | `programa`, `area_conocimiento` |
| `programa_pe` | `programa`, `practica_estrategia` |
| `programa_ci` | `programa`, `car_innovacion` |
| `an_programa` | `aspecto_normativo`, `programa` |
| `enfoque_rc` | `enfoque`, `registro_calificado` |
| `aa_rc` | `activ_academica`, `registro_calificado` |

Toda FK que capture la interfaz se presenta mediante un desplegable alimentado por la API. El usuario ve una etiqueta legible y la aplicación envía la clave real. No debe escribir IDs ni claves foráneas a mano.

## 3. Punto de partida encontrado en el repositorio

Este plan parte del código actual y exige completar sus brechas, no volver a construir sin necesidad lo que ya existe:

- El script SQL ya define las 15 tablas con FK y un trigger para el conteo de facultades.
- La API ya cuenta con controladores y operaciones de lista/creación/actualización/eliminación para las nueve entidades regulares. Se debe revisar y completar la consulta individual por clave allí donde falta.
- En `db/v2_innovacion_curricular.sql` faltan procedimientos `sp_consultar` para `activ_academica`, `pasantia`, `premio`, `docente_departamento` y `alianza`.
- El backend y el frontend contemplan asignar/retirar cuatro relaciones: `programa_ac`, `programa_pe`, `programa_ci` y `an_programa`.
- `enfoque_rc` y `aa_rc` existen como tablas en el SQL, pero no tienen todavía el circuito completo de procedimientos, endpoints y pantallas de relación.
- El frontend ya contiene módulos para las nueve entidades regulares, pero el manejo de tablas puente debe completarse para que las seis relaciones puedan consultarse, asignarse y retirarse desde la interfaz.

Estas observaciones son una lista inicial de brechas; en la fase de auditoría se compara cada operación con los archivos reales y se actualiza su estado sin marcar tareas como terminadas si no se han probado.

## 4. Fases de trabajo

Las fases se ejecutan en las ramas actuales de los responsables. No se crea una rama adicional solo para la división del trabajo y nadie hace commits directamente a `main`.

- [X] **Fase 1 — Auditoría de esquema y contratos (Estudiante A).** Revisar `db/innovacion_curricular.sql`, `db/v2_innovacion_curricular.sql`, modelos, DTOs, repositorios, servicios, controladores y módulos de `front/index.html`. Confirmar PK, FK, columnas obligatorias, valores por defecto, columna `activo`, política de borrado y rutas. Registrar cualquier diferencia con el SQL oficial antes de modificar la implementación.

- [X] **Fase 2 — Procedimientos de las nueve entidades regulares (Estudiante A).** Garantizar `listar`, `consultar por clave`, `crear`, `actualizar` y `eliminar/inactivar` para `facultad`, `programa`, `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento` y `alianza`. Completar en particular los procedimientos de consulta por clave que faltan. Las operaciones deben ser parametrizadas, respetar las FK, filtrar inactivos cuando proceda y devolver el registro guardado cuando aplique.

- [X] **Fase 3 — CRUD de entidades regulares en API (Estudiante B).** Completar el recorrido Controller → Servicio → Repositorio de las nueve entidades: `facultad`, `programa`, `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento` y `alianza`. Cada recurso debe ofrecer listado, consulta individual por su clave real, creación, actualización y eliminación/inactivación. Mantener los contratos de rutas existentes; validar entradas y traducir FK inválidas o conflictos referenciales a HTTP 409 en español, sin exponer errores SQL crudos.

- [X] **Fase 4 — CRUD de las seis tablas puente (Estudiante A).** Completar `programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc` y `aa_rc`. Para cada relación crear los procedimientos para listar/consultar las asociaciones, asignar e retirar una relación. Implementar en API las rutas correspondientes. Para `enfoque_rc` y `aa_rc`, crear el circuito completo que actualmente falta. Una pareja duplicada debe responder con conflicto controlado; no debe existir una edición directa de una pareja de claves compuestas.

- [X] **Fase 5 — Formularios CRUD y selectores FK en el frontend (Estudiante B).** Revisar los formularios y editores de las nueve entidades regulares. Garantizar selectores de universidad en facultad; facultad en programa; programa en acreditación, registro calificado, actividad académica, pasantía, premio y docente-departamento; programa y aliado en alianza. Al editar, cargar el valor seleccionado actual. Al guardar, enviar la clave real y refrescar la tabla con los datos guardados por la API.

- [X] **Fase 6 — Pantallas de relaciones y vistas maestro-detalle (Estudiante B).** Completar las pantallas de las seis tablas puente: programa ↔ área de conocimiento; programa ↔ práctica/estrategia; programa ↔ característica de innovación; aspecto normativo ↔ programa; enfoque ↔ registro calificado; actividad académica ↔ registro calificado. Permitir ver las asociaciones existentes, asignar nuevas y retirar las seleccionadas. Completar las vistas maestro-detalle de universidad → facultad → programa y mostrar desde programa los recursos que le pertenecen cuando corresponda. La interfaz debe usar nombres legibles, no solo IDs.

- [X] **Fase 7 — Integridad, transacciones y manejo de errores (Ambos).** Comprobar FK inexistentes, asociaciones duplicadas, eliminación de registros con detalles relacionados y registros que no existen o están inactivos. Devolver 409 para conflictos referenciales, 404 para registros inexistentes y mensajes visibles en español. Las operaciones que creen o actualicen varios detalles dentro de una sola operación de negocio deben ser transaccionales.

- [X] **Fase 8 — Pruebas de aceptación y regresión (Ambos).** Ejecutar el CRUD de las nueve entidades regulares y las operaciones de listar/asignar/retirar de las seis tablas puente desde la interfaz. Probar cada selector FK, el trigger, el manejo de errores, el guardado y la persistencia después de recargar. Repetir las pruebas CRUD de las siete tablas V1. Registrar comandos, resultados reales y defectos pendientes; no marcar como aprobada una prueba que no se haya ejecutado.

- [X] **Fase 9 — Cierre e integración (Ambos).** Revisar `git diff` y `git status`, actualizar este archivo y `9_checklist.md`, documentar las decisiones sobre el esquema, integrar los commits de la fase anterior en la rama de trabajo, ejecutar pruebas de regresión y preparar el Pull Request. Crear el tag `v2` solo cuando pasen los criterios de aceptación.

## 5. Criterios para dar por terminada la V2

- [X] Las nueve entidades regulares V2 permiten listar, consultar por clave, crear, actualizar y eliminar/inactivar desde la interfaz.
- [X] Las seis tablas puente permiten consultar las asociaciones existentes, asignar relaciones y retirarlas; no permiten editar directamente una pareja compuesta.
- [X] Cada campo FK de formulario usa un desplegable cargado desde la API y muestra una descripción legible.
- [X] Las rutas, nombres de claves y tipos coinciden con los controladores, DTOs y SQL real.
- [X] No quedan procedimientos, endpoints o pantallas pendientes para `enfoque_rc` ni `aa_rc`.
- [X] Las FK inexistentes, duplicados y conflictos referenciales se manejan de forma controlada y en español.
- [X] El trigger está probado desde una operación iniciada en el frontend.
- [X] Las siete entidades de V1 continúan operativas y pasan sus pruebas de regresión.
- [X] API y frontend compilan o se ejecutan sin errores; se registran expresamente las pruebas que no se puedan ejecutar por falta de entorno.

## 6. Regla para cerrar cada fase

1. Compilar o ejecutar la parte modificada.
2. Ejecutar pruebas nuevas y las regresiones afectadas.
3. Revisar `git diff` y `git status`.
4. Hacer commit en la rama propia y publicar la rama.
5. El otro integrante integra los cambios en su rama y vuelve a ejecutar las pruebas antes de continuar.
