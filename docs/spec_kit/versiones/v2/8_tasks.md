# 8_tasks.md — Fases de trabajo de V2

> Esta es la división inicial propuesta para dos integrantes. Cada fase se hace en la rama actual del responsable; el otro integrante integra el commit en su propia rama, prueba y continúa. No se crea una rama adicional solo para esta división. Los nombres y límites pueden ajustarse si el esquema oficial exige otra agrupación.

- [ ] **Fase 1 — Estudiante A: especificación.** Crear y revisar los nueve documentos de `v2_innovacion_curricular/` y esta guía IA. Identificar bloqueos de esquema y arquitectura; no modificar lógica ni base de datos.
- [ ] **Fase 2 — Estudiante B: esquema SQL y procedimientos.** Tras recibir el SQL oficial, versionar el DDL real de las 15 tablas y los procedimientos CRUD de las 22 tablas; definir el trigger; comprobar ejecución idempotente y datos existentes.
- [ ] **Fase 3 — Estudiante A: acceso a datos y contratos comunes.** Adaptar los repositorios de V1 para llamar procedimientos; establecer manejo común de conflictos y preparar contratos de acceso a datos para V2.
- [ ] **Fase 4 — Estudiante B: facultad y programa.** Implementar modelos, DTOs, repositorios, servicios y controladores de `facultad` y `programa`; validar las relaciones universidad→facultad→programa.
- [ ] **Fase 5 — Estudiante A: recursos académicos.** Implementar `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia` y `premio`, conforme al esquema real.
- [ ] **Fase 6 — Estudiante B: relaciones y tablas puente.** Implementar `docente_departamento`, `alianza`, `programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc` y `aa_rc`.
- [ ] **Fase 7 — Estudiante A: interfaz.** Completar pantallas CRUD, desplegables FK, vistas maestro-detalle, asignación/retirada de tablas puente y mensajes de error en español.
- [ ] **Fase 8 — Ambos: integración y entrega.** Probar todos los criterios, regresión V1, trigger, 409, transacciones, prompts reales, guía IA, checklist, tag `v2` y Pull Request.

## Regla para cerrar cada fase
1. Compilar lo que corresponda.
2. Ejecutar pruebas nuevas y las regresiones afectadas.
3. Revisar `git diff` y `git status`.
4. Hacer commit en la rama propia y publicar la rama.
5. El otro integrante hace merge en su rama y repite las pruebas antes de continuar.
