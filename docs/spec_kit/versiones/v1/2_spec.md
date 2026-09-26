# 2_spec.md — v1: los 7 catálogos sin clave foránea

## 1. Propósito
Que Vicerrectoría Académica pueda registrar y consultar, desde una
pantalla, los 7 catálogos base del módulo de Innovación Curricular, sin
que tres programas monten la misma innovación sin saberlo.

## 2. Alcance
**Incluye:** los 6 endpoints (GET lista, GET por id, POST, PUT, PATCH,
DELETE) para `area_conocimiento`, `universidad`, `aspecto_normativo`,
`practica_estrategia`, `enfoque`, `car_innovacion` y `aliado`; el front
que lista y crea (y edita/retira) las 7; borrado lógico vía `activo`.

**No incluye:** ninguna tabla con clave foránea (`0_mapa_versiones.md`);
autenticación (llega en v3); consultas multitabla (llegan en v4).

## 3. Requisitos funcionales
Los mismos seis, para las 7 tablas (regla idéntica, se numera una vez).

### RF1 — Listar
`GET /api/<recurso>` devuelve las filas con `activo = 1`, envueltas en
`{recurso, limite, total, datos}`. Sin filas activas → **204**.

### RF2 — Obtener una
`GET /api/<recurso>/{id}` → 200 si existe y está activa · **404** si no
existe o `activo = 0`.

### RF3 — Crear
`POST /api/<recurso>` recibe el body con **todos** los campos, incluida
la llave (ninguna tabla tiene `IDENTITY`: la llave la pone quien crea,
igual que el NIT de un aliado). Falta un campo → **422** con `errores[]`,
sin tocar la base.

### RF4 — Reemplazar
`PUT /api/<recurso>/{id}` exige todos los campos (sin la llave, que va en
la ruta). Falta uno → 422. Id inexistente/inactivo → 404.

### RF5 — Actualizar parcial
`PATCH /api/<recurso>/{id}` acepta campos opcionales. El mismo cuerpo que
el PUT rechaza con 422, el PATCH lo acepta con 200. Id inexistente/
inactivo → 404.

### RF6 — Eliminar (lógico)
`DELETE /api/<recurso>/{id}` marca `activo = 0`. 200 la primera vez,
**404** la segunda.

## 4. Requisitos no funcionales
- El front nunca toca SQL (R5, Artículo 1).
- Toda consulta usa parámetros (Artículo 2).
- Un solo comando levanta los 3 servicios (Artículo 8).

## 5. Criterios de aceptación
1. `GET /api/universidads` responde 200 con `total: 6`.
2. `GET /api/area-conocimientos` responde 200 con `total: 218`.
3. `GET /api/aliados/9999` responde 404.
4. `POST /api/enfoques` sin `descripcion` responde 422 y no crea la fila.
5. El mismo body sin un campo: `PUT /api/car-innovacions/1` → 422; `PATCH /api/car-innovacions/1` → 200.
6. `DELETE /api/practica-estrategias/1` responde 200 la primera vez y 404 la segunda.
7. Las 7 pantallas del front listan y crean contra la API real.
8. La prueba de capas (`pruebas/`) corre con un repositorio falso en memoria, sin SQL Server.
9. `docker compose down -v && docker compose up -d --build`, corrido desde cero con los dos repos clonados como hermanos, deja arriba los 3 servicios y carga los catálogos.

## 6. Clarificaciones
- **C1 — ¿Las rutas usan plural correcto o la "s" literal del plan?**
  El resumen del plan da dos ejemplos textuales: `/api/area-conocimientos`
  y `/api/universidads`. **Resuelto:** se sigue el patrón literal —
  guión en vez de guión bajo, y "s" pegada — para las 7 rutas:
  `area-conocimientos`, `universidads`, `aspecto-normativos`,
  `practica-estrategias`, `enfoques`, `car-innovacions`, `aliados`.
- **C2 — ¿Cómo conviven dos repositorios con la regla de "un solo comando"
  (R6)?** Un `docker-compose.yml` no puede construir un `Dockerfile` que
  está en otro repositorio sin saber dónde queda. **Resuelto:**
  `docker-compose.yml` vive en el repositorio de la API, y su servicio de
  front usa como contexto de build la ruta relativa `../front_innovacion`.
  Esto exige que ambos integrantes clonen el repo del front con ese
  nombre exacto de carpeta, como hermana de la carpeta del repo de la
  API. Se documenta en el README de los dos repos y en `7_quickstart.md`.
- **C3 — ¿Dónde vive el spec kit, si hay dos repositorios?** **Resuelto:**
  en el repositorio de la API (es la fuente de verdad única); el repo del
  front tiene un README que enlaza a él, para no mantener dos copias que
  se puedan desincronizar.
- **C4 — ¿Quién asigna la llave si ninguna tabla tiene `IDENTITY`?**
  **Resuelto:** la persona que crea el registro la envía en el body del
  POST (igual que el NIT de `aliado` en el ejemplo del profesor). No se
  autogenera.
- **C5 — ¿Se valida que `id`/`nit`/`documento` no se repita?** El script
  no impone `UNIQUE` más allá de la propia llave primaria. **Resuelto:**
  la propia PK ya evita duplicados (el INSERT falla con 500 si se repite,
  documentado en `6_contracts.md`); no se agrega validación adicional
  (Artículo 10, YAGNI).

## 7. Definición de TERMINADA
Los 9 criterios de aceptación pasan, `dotnet build` compila los dos
repos, la prueba de capas pasa, y `9_checklist.md` está firmado.