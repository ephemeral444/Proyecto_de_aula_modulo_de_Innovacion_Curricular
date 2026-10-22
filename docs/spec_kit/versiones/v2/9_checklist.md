# 9_checklist.md — Criterios de aceptación V2

## Especificación y preparación
- [ ] Se conserva la constitución y el mapa de versiones compartidos.
- [ ] Los nueve documentos de esta versión describen solo el delta de V2.
- [ ] Se obtuvo el script oficial completo y se completó el diccionario físico de las 15 tablas nuevas.
- [ ] Se aclaró la ubicación final de los repositorios API y Front.

## Base de datos
- [ ] Las 15 tablas V2 coinciden con el esquema oficial.
- [ ] Todas las tablas mantienen PK, FK, nulabilidad, tipos e índices esperados.
- [ ] Existen procedimientos de listar, consultar, crear, actualizar y eliminar/retirar para las 22 tablas.
- [ ] Los procedimientos usan filtros de activos cuando corresponda.
- [ ] Los procedimientos de escritura devuelven la fila guardada cuando aplica.
- [ ] Existe al menos un trigger comprobable y documentado.
- [ ] Las operaciones multirregistro necesarias son transaccionales.

## API
- [ ] La API usa procedimientos almacenados; no queda SQL CRUD directo en repositorios.
- [ ] Se conserva el patrón de capas Controller → Servicio → Repositorio.
- [ ] Las FK inválidas y asociaciones duplicadas producen 409 en español.
- [ ] Los registros inexistentes/inactivos producen 404.
- [ ] No se exponen mensajes internos de SQL Server ni secretos.

## Interfaz
- [ ] Las 22 tablas son operables desde la interfaz.
- [ ] Las FK se seleccionan desde desplegables cargados por la API.
- [ ] Las FK opcionales envían `null` cuando se elige “(ninguna)”.
- [ ] Las pantallas maestro-detalle permiten ver y agregar detalles desde el maestro.
- [ ] Las tablas puente permiten asignar y retirar relaciones, sin editar una pareja existente.
- [ ] Los mensajes son comprensibles y están en español.

## Regresión y entrega
- [ ] Los criterios de aceptación de V1 siguen pasando.
- [ ] Se probaron escenarios exitosos y fallidos en la colección de pruebas.
- [ ] Se comprobó el trigger desde una operación iniciada en la interfaz.
- [ ] Compilan API y front.
- [ ] Se guardaron los prompts reales de ambos integrantes y las correcciones hechas a la IA.
- [ ] El checklist está completo antes del Pull Request.
- [ ] El tag `v2` se crea solo sobre el commit que pasa todos los criterios.
