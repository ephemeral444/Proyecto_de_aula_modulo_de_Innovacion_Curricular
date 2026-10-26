# 9_checklist.md — Criterios de aceptación V2

## Especificación y preparación
- [X] Se conserva la constitución y el mapa de versiones compartidos.
- [X] Los nueve documentos de esta versión describen solo el delta de V2.
- [X] Se obtuvo el script oficial completo y se completó el diccionario físico de las 15 tablas nuevas.
- [X] Se aclaró la ubicación final de los repositorios API y Front.

## Base de datos
- [X] Las 15 tablas V2 coinciden con el esquema oficial.
- [X] Todas las tablas mantienen PK, FK, nulabilidad, tipos e índices esperados.
- [X] Existen procedimientos de listar, consultar, crear, actualizar y eliminar/retirar para las 22 tablas.
- [X] Los procedimientos usan filtros de activos cuando corresponda.
- [X] Los procedimientos de escritura devuelven la fila guardada cuando aplica.
- [X] Existe al menos un trigger comprobable y documentado.
- [X] Las operaciones multirregistro necesarias son transaccionales.

## API
- [X] La API usa procedimientos almacenados; no queda SQL CRUD directo en repositorios.
- [X] Se conserva el patrón de capas Controller → Servicio → Repositorio.
- [X] Las FK inválidas y asociaciones duplicadas producen 409 en español.
- [X] Los registros inexistentes/inactivos producen 404.
- [X] No se exponen mensajes internos de SQL Server ni secretos.

## Interfaz
- [X] Las 22 tablas son operables desde la interfaz.
- [X] Las FK se seleccionan desde desplegables cargados por la API.
- [X] Las FK opcionales envían `null` cuando se elige “(ninguna)”.
- [X] Las pantallas maestro-detalle permiten ver y agregar detalles desde el maestro.
- [X] Las tablas puente permiten asignar y retirar relaciones, sin editar una pareja existente.
- [X] Los mensajes son comprensibles y están en español.

## Regresión y entrega
- [X] Los criterios de aceptación de V1 siguen pasando.
- [X] Se probaron escenarios exitosos y fallidos en la colección de pruebas.
- [X] Se comprobó el trigger desde una operación iniciada en la interfaz.
- [X] Compilan API y front.
- [X] Se guardaron los prompts reales de ambos integrantes y las correcciones hechas a la IA.
- [X] El checklist está completo antes del Pull Request.
- [X] El tag `v2` se crea solo sobre el commit que pasa todos los criterios.
