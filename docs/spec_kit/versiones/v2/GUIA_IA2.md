# Registro de Prompts y Guía de IA — Versión 2

**Módulo:** Innovación Curricular  
**Integrantes:** Integrante A e Integrante B  
**Versión:** v2 (15 Tablas Relacionales con Clave Foránea + 6 Tablas Puente + Stored Procedures + Triggers + HTTP 409 + Maestro-Detalle)

---

## 1. Alcance de Tablas Implementadas en la v2

En esta versión se implementó el CRUD completo (Listar, Consultar, Crear, Actualizar y Eliminar) por Stored Procedures y el manejo de integridad referencial para la totalidad de la estructura del módulo:

* **Maestros Principales:** `facultad`, `programa`
* **Entidades de Detalle:** `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento`, `alianza`
* **Tablas Puente (Relaciones Mucho a Muchos):** `programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc`, `aa_rc`

---

## 2. Bitácora de Prompts e Interacción con la IA

### Prompt 1: Creación del Script de Base de Datos (SPs y Trigger)
* **Objetivo:** Generar la estructura SQL Server con procedimientos almacenados estandarizados para las 15 tablas con clave foránea y las 6 tablas puente, aplicando borrado lógico.
* **Prompt utilizado:**
  > "Genera un script SQL Server para el módulo de Innovación Curricular (`innovacion_curricular`). Necesitamos los Stored Procedures para las entidades `facultad`, `programa`, `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento` y `alianza` siguiendo la nomenclatura `sp_listar_*`, `sp_consultar_*`, `sp_crear_*`, `sp_actualizar_*` y `sp_eliminar_*`. El borrado debe ser lógico (`UPDATE activo = 0`), los SPs de creación y actualización deben devolver la fila guardada mediante `OUTPUT` o `SELECT`, y se deben incluir los SPs de asignación/retiro para las 6 tablas puente (`programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc`, `aa_rc`). Además, crea el trigger `trg_contar_facultades` que mantenga actualizado el campo `total_facultades` en `universidad`."
* **Corrección aplicada a la IA:**
  * La IA generó inicialmente un trigger que solo escuchaba `AFTER INSERT, DELETE`. Se le corrigió para escuchar también `AFTER UPDATE`, ya que en un sistema con borrado lógico el retiro de un registro es un `UPDATE (activo = 0)`. De no incluirlo, el contador desfasaba el total de facultades al inhabilitar una.

---

### Prompt 2: Arquitectura en C# (DTOs, Repositorios Dapper, Servicios y HTTP 409)
* **Objetivo:** Implementar la arquitectura en 3 capas (`Controlador` $\rightarrow$ `Servicio` $\rightarrow$ `Repositorio`) para todas las entidades y tablas puente, asegurando la captura de excepciones SQL.
* **Prompt utilizado:**
  > "Implementa la arquitectura por capas en C# / ASP.NET Core sobre .NET 8 usando Dapper. Crea los modelos, DTOs de petición en `Peticiones/`, interfaces y repositorios en `Repositorios/`, servicios en `Servicios/` y controladores REST en `Controllers/` para las 15 tablas con clave foránea y las 6 tablas puente. En cada repositorio, captura la `SqlException` con número 547 (violación de clave foránea) y 2627/2601 (clave duplicada) para lanzar `ConflictoExcepcion`, haciendo que los controladores respondan código HTTP 409 Conflict con mensajes claros en castellano."
* **Correcciones aplicadas a la IA:**
  1. La IA ubicó inicialmente algunas DTOs dentro de la carpeta `Modelos/` en lugar de `Peticiones/`, lo que causó errores de namespace (`CS0246`). Se ajustó la estructura moviendo todas las peticiones de entrada a `ApiInnovacion.Peticiones`.
  2. Para las tablas puente con llaves compuestas mixtas (`INT` y `VARCHAR`, como `enfoque_rc` o `aa_rc` con campos adicionales de componente y semestre), la IA usó firmas genéricas de enteros. Se le solicitó la refactorización con DTOs especializados (`TablaPuenteIntStringPeticion` y `TablaPuenteAaRcPeticion`) e interfaces de servicio dedicadas (`IServicioTablaPuente`).
  3. Faltaban las firmas del método `ActualizarAsync` en las interfaces `IServicio*.cs` de las entidades de detalle; se agregaron explícitamente para mantener el contrato de interfaces en C#.

---

### Prompt 3: Interface Frontend (Panel de CRUDs Independientes por Entidad)
* **Objetivo:** Construir una interfaz web modular organizada por secciones/pestañas donde cada entidad relacional posea su propia tabla de gestión y formulario CRUD.
* **Prompt utilizado:**
  > "Genera el código frontend para la gestión del módulo usando una estructura de navegación por pestañas (tabs). Cada entidad (`facultad`, `programa`, `acreditacion`, `registro_calificado`, etc.) debe tener su propio panel con un formulario para Crear/Editar y una tabla independiente para Listar y Eliminar. Todos los campos de clave foránea deben ser desplegables `<select>` cargados consumiendo la API REST (`GET /api/*`), mostrando los nombres descriptivos. Agrega el manejo del estado HTTP 409 para alertar violaciones de integridad referencial."---

## 3. Guía para la Sustentación Individual (Interpretabilidad — 4 Puntos)

Preguntas tipo para defender cualquier archivo del repositorio ante el profesor:

1. **¿Qué hace este Stored Procedure o método sin leerlo línea por línea?**
   * *Respuesta:* En el caso de los SPs de borrado (`sp_eliminar_*`), realiza un borrado lógico marcando `activo = 0`. Los listados (`sp_listar_*`) aplican el filtro `WHERE activo = 1` con `INNER JOIN` a las tablas padre para retornar los nombres descriptivos en lugar de simples códigos.
2. **¿Por qué el total de facultades lo calcula un disparador en SQL Server y no el servicio en C#?**
   * *Respuesta:* Por integridad de datos. El disparador garantiza que el conteo se actualice automáticamente ante cualquier operación (`INSERT`, `UPDATE`, `DELETE`) directamente en la base de datos, previniendo inconsistencias si la BD se modifica desde fuera de la API.
3. **Si la base de datos rechaza una clave foránea o una clave duplicada en una tabla puente, ¿qué sucede en el flujo?**
   * *Respuesta:* SQL Server lanza un error (547 para FK inexistente, 2627/2601 para duplicados). El Repositorio C# lo captura y lanza `ConflictoExcepcion`. El Controlador atrapa la excepción y retorna un estatus HTTP **409 Conflict** con un objeto JSON `{ "mensaje": "..." }`. En el Frontend, la petición detecta el `status === 409` y despliega una alerta visible para el usuario en castellano.
4. **Si mañana hay que agregar un nuevo atributo a una entidad (ej. a `activ_academica`), ¿qué capas se modifican?**
   * *Respuesta:* 
     1. Base de datos: `ALTER TABLE` y actualización de los SPs (`sp_crear_*`, `sp_actualizar_*`, `sp_consultar_*`).
     2. C#: Propiedad en `Modelos/ActivAcademica.cs`, DTOs en `Peticiones/ActivAcademicaCrear.cs` y `ActivAcademicaActualizar.cs`, y parámetros Dapper en `RepositorioActivAcademicaSqlServer.cs`.
     3. Frontend: Input en el formulario HTML y mapeo del JSON en el script JS.

---

## 4. Checklist de Criterios de Aceptación (v2)

| # | Criterio | Estado | Verificación |
|---|---|---|---|
| **1** | CRUD completo de tablas v1 | ✅ OK | Operables desde la interfaz gráfica. |
| **2** | CRUD de las 15 tablas relacionales v2 | ✅ OK | `facultad`, `programa`, `acreditacion`, `registro_calificado`, `activ_academica`, `pasantia`, `premio`, `docente_departamento`, `alianza`. |
| **3** | Operaciones vía Stored Procedures | ✅ OK | Cero SQL concatenado a mano en C#; todo pasa por `sp_*`. |
| **4** | Disparador (Trigger) funcionando | ✅ OK | `trg_contar_facultades` mantiene actualizado `total_facultades` en `universidad`. |
| **5** | Claves foráneas en listas desplegables | ✅ OK | Los `<select>` cargan dinámicamente desde la API mostrando nombres y enviando IDs/códigos. |
| **6** | FK Opcional acepta `null` | ✅ OK | Campos opcionales mapeados adecuadamente hacia `NULL` en SQL. |
| **7** | Error de FK/duplicados responde HTTP 409 en castellano | ✅ OK | Captura explícita de `SqlException` (547, 2627, 2601) lanzando `ConflictoExcepcion`. |
| **8** | Tablas puente asignan y retiran con ambas claves | ✅ OK | Operativas las 6 relaciones (`programa_ac`, `programa_pe`, `programa_ci`, `an_programa`, `enfoque_rc`, `aa_rc`). Asignar duplicado da 409. |
| **9** | Vista Tablas de entidades | ✅ OK | Cada entidad tiene su propia tabla para manejar su CRUD de manera eficiente y segura |
| **10** | Borrado lógico filtrado | ✅ OK | Registros con `activo = 0` no aparecen en ningún listado del sistema. |
| **11** | Interfaz limpia sin jerga técnica | ✅ OK | Mensajes al usuario libres de códigos de error de BD o términos como "FK" / "500". |
| **12** | Regresión v1 completa | ✅ OK | Todos los componentes y catálogos de la versión 1 continúan operando correctamente. |