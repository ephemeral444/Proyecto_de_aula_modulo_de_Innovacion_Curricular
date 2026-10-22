# GUIA_IA2.md — Prompts utilizados para construir la V2

## Importante
Este archivo debe contener los prompts reales usados por cada integrante y las correcciones que tuvieron que hacer. No se debe presentar una plantilla como si fuera el historial real. Completarlo durante el trabajo y conservar los prompts de chat y del IDE agéntico si se usan ambos.

## Prompt inicial — Estudiante A

Actúa como líder técnico del módulo Innovación Curricular. Estoy trabajando en la Fase 1 de la V2 desde mi rama existente; no crees una rama nueva ni hagas commits directos a `main`. Usa como fuentes la constitución compartida, el mapa de versiones, la especificación de V1 y el documento del profesor. Crea el spec kit de V2 bajo `docs/spec_kit/versiones/v2_innovacion_curricular/` con los documentos 2_spec.md, 3_plan.md, 4_research.md, 5_data_model.md, 6_contracts.md, 7_quickstart.md, 8_tasks.md y 9_checklist.md, además de esta guía. Describe solo el delta de V2 y conserva la terminología del curso. No inventes columnas ni tipos: si falta el script SQL oficial completo, registra el bloqueo explícitamente. Incluye los 15 recursos V2, los requisitos de procedimientos almacenados para las 22 tablas, trigger, FK por desplegables, HTTP 409, maestro-detalle, transacciones, regresión V1 y exclusiones V3/V4. Antes de finalizar, compara la especificación con los archivos reales del repositorio y enumera cualquier discrepancia pendiente.

## Prompt inicial — Estudiante B (para usar al comenzar su fase)

Actúa como líder técnico del módulo Innovación Curricular. Integra primero los commits de la fase anterior en mi rama actual, sin trabajar directamente en `main`. Lee el spec kit de V2 completo antes de modificar código. No inventes el esquema: utiliza el script SQL oficial del curso y registra cualquier contradicción con la especificación antes de implementar. Implementa únicamente la fase que me corresponde, con rutas exactas, archivos completos, comentarios útiles, comandos PowerShell y pruebas reproducibles. Todas las operaciones CRUD deben pasar por procedimientos almacenados parametrizados. Mantén compatibilidad con V1, responde 409 en español ante conflictos referenciales, no expongas errores SQL crudos y comprueba API e interfaz. Al terminar, resume los archivos modificados, las pruebas ejecutadas, resultados esperados y comandos Git para hacer commit en mi rama.

## Registro obligatorio de prompts reales
| Fecha | Integrante | Herramienta | Fase | Prompt usado (archivo o enlace) | Error/corrección aplicada | Commit relacionado |
|---|---|---|---|---|---|---|
| Pendiente | Estudiante A | Pendiente | 1 | Reemplazar con el prompt real usado | Registrar correcciones reales | Pendiente |
| Pendiente | Estudiante B | Pendiente | Pendiente | Reemplazar con el prompt real usado | Registrar correcciones reales | Pendiente |

## Lecciones aprendidas
Registrar aquí qué generó incorrectamente la IA, qué se le pidió corregir y cómo se verificó la corrección. No afirmar que una prueba pasó si no se ejecutó.
