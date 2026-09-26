# 0_mapa_versiones.md

## v1 — las 7 tablas sin clave foránea (esta es la que construimos ahora)
`area_conocimiento`, `universidad`, `aspecto_normativo`,
`practica_estrategia`, `enfoque`, `car_innovacion`, `aliado`. Los seis
endpoints completos para cada una, API REST + front Blazor.

## v2 — todas las tablas, con clave foránea (fuera de alcance por ahora)
`facultad`, `programa`, `acreditacion`, `registro_calificado`,
`activ_academica`, `pasantia`, `premio` y las tablas puente
(`programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc`,
`aa_rc`, `docente_departamento`, `alianza`). Listas desplegables en el
front para las FK.

## v3 — fuera de alcance por ahora
JWT, sesiones, control de acceso por roles, CRUD de `usuario`/`rol`.

## v4 — fuera de alcance por ahora
10 consultas multitabla como endpoints, dashboard con gráficos, imagen
corporativa, sitio publicado.

## Regla de esta versión
Solo se construye v1. Todo lo de v2 a v4 se declara aquí y no se toca
ahora — ver Artículo 10 (YAGNI) de la constitución.