# 1_constitution.md — Reglas permanentes del proyecto

**Módulo Innovación Curricular**. Versión de la constitución: 1.0 — 14 de
septiembre de 2026. Una sola para las 4 entregas (v1→v4). Sale de
0_METODOLOGIA.md y del Plan de Desarrollo (§2.c, restricciones R1–R8).

## Artículo 1 — Capas y su dirección
Controller → Servicio → Repositorio. Cada capa conoce solo a la
inmediatamente inferior, por interfaz. El front NUNCA habla con la base de
datos (R5): solo con la API, por HTTP.

## Artículo 2 — SQL siempre parametrizado, sin ORM (R2)
Dapper + `Microsoft.Data.SqlClient`. Valores como `@parametros`. Ninguna
cadena de SQL se concatena con datos del cliente.

## Artículo 3 — El borrado es siempre lógico (R1)
Ninguna tabla se borra físicamente. `DELETE` marca `activo = 0`; los
listados y búsquedas filtran `WHERE activo = 1`.

## Artículo 4 — Rutas específicas por recurso, nunca genéricas (R3)
`/api/<recurso>`, nunca `/api/{tabla}` con un parámetro de tabla. El nombre
exacto de cada ruta se fija en `2_spec.md` §6 de cada versión.

## Artículo 5 — Petición distinta de modelo
El body que llega del cliente se describe con una clase en `Peticiones/`,
nunca con la entidad de `Modelos/`. Un verbo, una clase.

## Artículo 6 — Una interfaz por servicio y por repositorio
Todo servicio y todo repositorio se inyecta por su interfaz. El ensamblador
(`Program.cs`) es el único archivo que conoce las clases concretas.

## Artículo 7 — Cada entrega incluye su front (R4)
Ninguna versión se considera terminada si solo existe la API. El front
consume la API por HTTP; nunca queda una versión "solo de backend".

## Artículo 8 — Todo levanta con un solo comando (R6)
`docker compose up -d --build`, corrido desde la carpeta del repo de la
API (con el repo del front clonado como hermano, carpeta `front_innovacion`),
deja arriba la base de datos, la API y el front. Ver D1 en `4_research.md`.

## Artículo 9 — Dos repositorios, ramas por persona, main protegida (R7, R8)
El repositorio de la API y el del front son privados, con `ccastro2050`
invitado en los dos. Nadie programa directo en `main`: cada integrante
trabaja en su propia rama, y solo el encargado del main hace merge, por PR,
cuando los criterios de la entrega pasan.

## Artículo 10 — No se anticipa (YAGNI)
Ninguna entrega construye tablas o reglas de una entrega futura. v1 no
toca `facultad`, `programa`, `usuario`, `rol` ni ninguna tabla con clave
foránea — esas llegan en v2 y v3 (`0_mapa_versiones.md`).

## Artículo 11 — Ambigüedad se marca, no se rellena
Cuando el módulo, la entrevista o las historias no dicen algo, se escribe
`[NECESITA ACLARACIÓN]` en `2_spec.md` y se resuelve ahí, con fecha y razón.

## Artículo 12 — Enmiendas
Una entrega que necesite contradecir un artículo lo propone en su
`4_research.md`. Si se acepta, esta constitución sube de versión.