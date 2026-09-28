# Promts utilizados para la construcción de la VERSIÓN 1 del proyecto de Innovacion Curricular

## IA utilizada: Google Gemini 

# Prompt de Cristian Diaz

Actúa como Líder Técnico de Software. Soy Cristian Díaz Álvarez y me corresponde implementar las Fases 0, 2, 4, 6 y 8 del proyecto Módulo de Innovación Curricular en mi rama Rama_CristianDiazAlvarez.
Genera el paso a paso detallado, los comandos de PowerShell y el código listo para implementar mis fases asignadas, respetando la Constitución del proyecto (Arquitectura en capas Controller → Servicio → Repositorio, Dapper con SQL parametrizado, borrado lógico activo = 1/0, DTOs desacoplados y Docker Compose):

- Fase 0 (Infraestructura y Esqueleto): Creación del .gitignore, generación de la estructura de directorios/archivos del Spec Kit v1, creación del script DDL db/innovacion_curricular.sql, e inicialización de la solución .NET 8 (Web API ApiInnovacion y consola de pruebas PruebaCapas).

- Fase 2 (Contratos y Excepciones): Definición de DTOs en Peticiones/ para desacoplar las entradas HTTP de los modelos de dominio y creación de la excepción personalizada NoEncontradoExcepcion en Excepciones/.

- Fase 4 (Servicios y Pruebas de Aislamiento): Implementación de la lógica de negocio desacoplada mediante interfaces de servicio en Servicios/ y desarrollo de la suite de validación unitaria/aislamiento en el proyecto pruebas/PruebaCapas.

- Fase 6 (Interfaz Web Front-End): Creación del cliente web ligero en front/index.html (HTML5, CSS3, JavaScript Vanilla) para consumir la API mediante fetch, soportando operaciones CRUD y manejo de estados/errores en español.

- Fase 8 (Cierre, Sustentación y Pull Request): Verificación de criterios en docs/spec_kit/versiones/v1/9_checklist.md, redacción del informe de sustentación en RESPUESTAS.md y ejecución del flujo de integración mediante Pull Request hacia la rama main sin commits directos.

Asegúrate de incluir los comandos Git/PowerShell precisos para realizar los commits de cada una de mis fases y subir los cambios a GitHub

# Prompt de Juan Pablo Acevedo

Actúa como Líder Técnico de Software. Soy Juan Pablo Acevedo y me corresponde implementar las Fases 1, 3, 5 y 7 del proyecto Módulo de Innovación Curricular en mi rama Rama_JuanPabloAcevedo.
Genera el paso a paso detallado, los comandos de PowerShell y el código listo para implementar mis fases asignadas, sincronizándome con la base inicial (Fase 0) creada en el repositorio y respetando los lineamientos de la Constitución del proyecto:

- Fase 1 (Modelos de Dominio): Creación de las entidades de C# en Modelos/ alineadas exactamente con la estructura de la base de datos SQL Server y con soporte nativo para el atributo de borrado lógico (Activo).

- Fase 3 (Acceso a Datos y Repositorios Dapper): Definición de las interfaces de repositorio y sus implementaciones concretas en Repositorios/ usando Dapper y Microsoft.Data.SqlClient, asegurando que todas las consultas SQL estén estrictamente parametrizadas y filtren por activo = 1.

- Fase 5 (Controladores RESTful y Cableado): Creación de los controladores HTTP en Controllers/ con rutas específicas por recurso (ej. /api/docentes), manejo de códigos de estado HTTP estandarizados y registro de inyección de dependencias en Program.cs.

- Fase 7 (Contenerización y Orquestación): Creación de los archivos Dockerfile para la API y el Front, configuración del script db/init.sh y orquestación unificada en docker-compose.yml para levantar la solución completa con un solo comando (docker compose up -d --build).

- Asegúrate de incluir los comandos Git de sincronización para traer el esqueleto inicial desde la rama de Cristian (Rama_CristianDiazAlvarez), trabajar en mi propia rama y enviar las correcciones/fases a GitHub para la combinación final.