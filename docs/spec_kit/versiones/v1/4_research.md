# 4_research.md — Decisiones (v1)

## D1 — Un solo comando con dos repositorios separados
**Contexto:** R6 exige levantar todo con un comando; R7 exige dos repos
separados (API y front). Un `docker-compose.yml` necesita saber dónde
está cada `Dockerfile` para construirlo.
**Alternativas:** (a) git submodules (uno de los repos incluye al otro
como submódulo); (b) duplicar el compose en los dos repos, cada uno
levantando solo su parte; (c) un solo `docker-compose.yml`, en el repo
de la API, con el servicio del front apuntando a `../front_innovacion`
como contexto de build, exigiendo esa carpeta hermana al clonar.
**Decisión: (c).** Los submódulos (a) complican el flujo de ramas que ya
es nuevo para el equipo; duplicar el compose (b) no cumple "un solo
comando" para todo el sistema junto.
**Consecuencias que se aceptan:** los dos integrantes deben clonar el
repo del front con el nombre exacto `front_innovacion`, documentado en
ambos README. Si alguien lo clona con otro nombre, el build del front
falla con una ruta no encontrada — error claro, no silencioso.
**Estado:** vigente.

## D2 — Rutas con el patrón literal del plan, no el plural gramatical
**Contexto:** el resumen del plan escribe `/api/area-conocimientos` y
`/api/universidads` — la segunda no es plural correcto de "universidad".
**Alternativas:** (a) corregir a plural gramatical (`universidades`,
`áreas-conocimiento`); (b) seguir el patrón literal (guión + "s" pegada)
de los dos ejemplos dados.
**Decisión: (b).** Son dos ejemplos textuales, no una errata aislada;
adivinar "lo que el profesor quiso decir" es exactamente lo que el
método pide no hacer (Artículo 11) — se sigue lo escrito.
**Estado:** vigente.

## D3 — Sin `IDENTITY`: la llave la asigna quien crea
**Contexto:** igual que en el ejemplo de `aliado` (llave = NIT), ninguna
de las 7 tablas declara `IDENTITY`.
**Alternativas:** (a) el repositorio calcula `MAX(id)+1`; (b) el cliente
la envía en el body.
**Decisión: (b)**, por consistencia con el patrón que ya trae `aliado`
en el repositorio de referencia del profesor — no se inventa un
mecanismo distinto para las otras 6.
**Estado:** vigente.

## D4 — Spec kit en un solo repositorio, no en los dos
**Contexto:** con dos repos, ¿dónde vive la fuente de verdad?
**Alternativas:** (a) duplicarlo en los dos; (b) vivir en el de la API,
con un enlace desde el README del front.
**Decisión: (b)** — evita que las dos copias se desincronicen.
**Estado:** vigente.

## D5 — Sin repositorio genérico compartido entre las 7 tablas
**Contexto:** los seis endpoints se repiten igual para las 7 tablas.
**Alternativas:** (a) una clase base genérica; (b) siete pares
interfaz+implementación independientes.
**Decisión: (b)** — por la misma razón que en la evaluación anterior:
menos acoplamiento entre recursos, a costa de repetir código.
**Estado:** vigente.