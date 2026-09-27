# Lista de Chequeo y Criterios de Aceptación — Versión 1

## Integridad Estructural y Base de Datos
- [x] Script DDL `db/innovacion_curricular.sql` contiene las 7 tablas independientes (`area_conocimiento`, `universidad`, `aspecto_normativo`, `practica_estrategia`, `enfoque`, `car_innovacion`, `aliado`).
- [x] Todas las tablas incluyen la columna `activo BIT NOT NULL DEFAULT 1`.
- [x] Datos iniciales semilla insertados y validados para pruebas iniciales.

## Arquitectura y Desacoplamiento por Capas
- [x] Modelos de dominio puros en `Modelos/` con propiedad `Activo`.
- [x] DTOs de peticiones (`Crear`, `Reemplazo`, `Actualizar`) definidos en `Peticiones/`.
- [x] Excepción de dominio `NoEncontradoExcepcion` desacoplada de dependencias HTTP.
- [x] Repositorios SQL Server con `Microsoft.Data.SqlClient` y Dapper utilizando consultas 100% parametrizadas.
- [x] Servicios en `Servicios/` desacoplados de HTTP y validados con pruebas en consola (`pruebas/PruebaCapas.csproj`).
- [x] Controladores ASP.NET Core capturan excepciones de dominio y devuelven códigos HTTP correctos (200, 204, 404, 422).

## Interfaz de Usuario y Despliegue
- [x] Front-end en `front/index.html` consumiendo el puerto HTTP 8124.
- [x] Interfaz resiliente con aviso en español si la API no está disponible.
- [x] Contenedores configurados con Dockerfiles y `docker-compose.yml`.