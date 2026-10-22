# 6_contracts.md — Contratos API V2

## 1. Convenciones
Se conserva la forma de respuesta V1 salvo que una modificación sea imprescindible y esté documentada. Las rutas concretas siguen el estilo existente de la API; no cambiar rutas actuales por motivos cosméticos.

- Listado: respuesta paginada/envoltorio existente (`recurso`, `limite`, `total`, `datos`) cuando aplique.
- Consulta individual: `200 OK`; inexistente/inactivo: `404 Not Found`.
- Creación/actualización: respuesta con la fila guardada devuelta por el procedimiento almacenado cuando aplique.
- Conflicto de FK, clave duplicada o restricción referencial: `409 Conflict`, mensaje en español.
- Validación de datos de entrada: mantener el contrato existente de validación de la API; no convertirla en `500`.
- Error inesperado: respuesta genérica en español y registro interno, sin filtrar SQL, stack traces ni secretos.

## 2. Recursos de V2
| Recurso | Operaciones | Clave/ruta | Dependencias conocidas |
|---|---|---|---|
| facultad | GET lista, GET por clave, POST, PUT/PATCH según patrón existente, DELETE lógico/físico según esquema | Pendiente de confirmar con SQL | universidad |
| programa | CRUD completo | Pendiente de confirmar con SQL | facultad |
| acreditacion | CRUD completo | `resolucion` como clave según consigna | programa |
| registro_calificado | CRUD completo | `codigo` como clave según consigna | programa |
| activ_academica | CRUD completo | Pendiente de confirmar con SQL | Según esquema oficial |
| pasantia | CRUD completo | Pendiente de confirmar con SQL | programa |
| premio | CRUD completo | Pendiente de confirmar con SQL | programa y entidad otorgante según esquema |
| docente_departamento | CRUD/asignación y retirada | Posible clave compuesta; confirmar | programa y docente según esquema |
| alianza | CRUD completo | Pendiente de confirmar con SQL | aliado, programa, docente responsable |
| programa_ac | asignar/listar/retirar | Clave compuesta por confirmar | programa, area_conocimiento |
| programa_pe | asignar/listar/retirar | Clave compuesta por confirmar | programa, practica_estrategia |
| programa_ci | asignar/listar/retirar | Clave compuesta por confirmar | programa, car_innovacion |
| an_programa | asignar/listar/retirar | Clave compuesta por confirmar | aspecto_normativo, programa |
| enfoque_rc | asignar/listar/retirar | Clave compuesta por confirmar | enfoque, registro_calificado |
| aa_rc | asignar/listar/retirar | Clave compuesta por confirmar | activ_academica, registro_calificado |

Las rutas HTTP y los nombres de los parámetros se fijan después de revisar los controladores V1 y el esquema oficial; no se deben adivinar pluralizaciones ni tipos de clave.

## 3. Contratos de procedimientos almacenados
Para cada tabla regular:
- `sp_listar_<tabla>`: devuelve filas activas si la tabla tiene `activo`.
- `sp_consultar_<tabla>`: recibe la clave y devuelve el registro activo o ningún registro.
- `sp_crear_<tabla>`: recibe campos de entrada y devuelve la fila guardada.
- `sp_actualizar_<tabla>`: recibe clave y campos actualizables, devuelve la fila guardada.
- `sp_eliminar_<tabla>`: aplica borrado lógico si hay `activo`; de lo contrario, aplica la política aprobada.

Para tablas puente se documentan procedimientos para listar asociaciones, asignar una pareja y retirarla. La asignación repetida debe resultar en un conflicto reconocible por la API.

## 4. Contrato de errores
| Situación | HTTP esperado | Mensaje visible esperado |
|---|---:|---|
| Padre/FK inexistente | 409 | “No se puede guardar porque uno de los registros relacionados ya no existe o no está disponible.” |
| Pareja duplicada en tabla puente | 409 | “Esta relación ya está registrada.” |
| Retiro físico de un padre que conserva hijos (si aplica) | 409 | “No se puede retirar este registro porque tiene información relacionada.” |
| Registro inexistente/inactivo | 404 | “No se encontró el registro solicitado.” |
| Datos de entrada inválidos | Código de validación acordado con contrato V1 | Mensaje específico en español |

Los mensajes son propuestas iniciales; se deben uniformar con el formato de error real de la V1 durante la implementación.

## 5. Transacciones maestro-detalle
Cuando una petición representa una operación de negocio que guarda varios detalles, todas las escrituras se ejecutan en una transacción. Ante el fallo de un elemento, se revierte el conjunto y se devuelve un error controlado.
