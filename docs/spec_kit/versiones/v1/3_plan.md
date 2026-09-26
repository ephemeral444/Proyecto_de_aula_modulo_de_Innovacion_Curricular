# 3_plan.md — El CÓMO

## 1. Stack
- API: C# / ASP.NET Core Web API (.NET 8), SQL Server.
- Datos: Dapper + `Microsoft.Data.SqlClient`, SQL a mano.
- Front: Blazor Server (tercer proceso, habla solo HTTP con la API).
- Contenedores: `docker-compose.yml` en el repo de la API, 4 servicios
  (sqlserver, sqlserver-init, api-innovacion, front-innovacion).

## 2. Estructura de carpetas

### Repositorio 1 — la API (aquí vive el spec kit)
```
docs/spec_kit/                  este spec kit completo
db/                              innovacion_curricular.sql + init.sh
api_innovacion/
├── Controllers/                 CAPA 1 — 7 controllers
├── Modelos/                     7 entidades
├── Peticiones/                  21 clases (3 por recurso)
├── Servicios/                   7 interfaces + 7 implementaciones
├── Repositorios/                 7 interfaces + 7 implementaciones SQL Server
├── Excepciones/                 NoEncontradoExcepcion.cs
└── pruebas/                     prueba de capas sin base de datos
docker-compose.yml               referencia ../front_innovacion (ver C2)
postman/                        (opcional)
README.md
```

### Repositorio 2 — el front
```
front_blazor/
├── Program.cs
├── Servicios/                   7 servicios HTTP (uno por recurso)
├── Components/Pages/            7 pantallas
└── wwwroot/
README.md                        enlaza al spec kit del repo de la API
```

## 3. Arquitectura (el viaje de una petición)
```
Navegador → Blazor Server (front, otro proceso)
          → HTTP → Controller (API) → Servicio → Repositorio → SQL Server
```
Igual para las 7 tablas — mismo patrón repetido 7 veces (ver D5).

## 4. Decisiones de diseño
Ver `4_research.md` D1–D5. Resumen: llave la pone el cliente (sin
IDENTITY); rutas con el patrón literal del plan (C1); compose en el
repo de la API con contexto relativo al front (C2).

## 5. Inventario

**Nuevos, por cada una de las 7 tablas (mismo patrón):**
| Archivo | Papel |
|---|---|
| `Modelos/<Recurso>.cs` | Entidad |
| `Peticiones/<Recurso>Crear.cs` | Body del POST, con la llave |
| `Peticiones/<Recurso>Reemplazo.cs` | Body del PUT, sin la llave |
| `Peticiones/<Recurso>Actualizar.cs` | Body del PATCH, todo opcional |
| `Repositorios/IRepositorio<Recurso>.cs` / `Repositorio<Recurso>SqlServer.cs` | Datos |
| `Servicios/IServicio<Recurso>.cs` / `Servicio<Recurso>.cs` | Negocio |
| `Controllers/<Recurso>Controller.cs` | HTTP |
| (front) `Servicios/Servicio<Recurso>.cs` | Cliente HTTP del front |
| (front) `Components/Pages/<Recurso>.razor` | Pantalla |

**Únicos:** `Excepciones/NoEncontradoExcepcion.cs`, `Program.cs` (los dos
repos), `pruebas/`, `db/innovacion_curricular.sql`, `db/init.sh`,
`docker-compose.yml`, los dos `Dockerfile`.

## 6. Chequeo de constitución
| Artículo | Cómo lo cumple v1 |
|---|---|
| 1 — Capas | Controller/Servicio/Repositorio en los 7 recursos |
| 2 — SQL parametrizado | Dapper con `@parametros` |
| 3 — Borrado lógico | `DELETE` → `UPDATE ... SET activo = 0` |
| 4 — Rutas específicas | 7 rutas fijas, resueltas en C1 |
| 5 — Petición ≠ modelo | 21 clases en `Peticiones/` |
| 6 — Interfaces | `IServicioX`/`IRepositorioX` × 7 |
| 7 — Front en cada entrega | Front Blazor con las 7 pantallas |
| 8 — Un comando | `docker compose up -d --build`, resuelto en C2 |
| 9 — Dos repos, ramas, main protegida | Ver `8_tasks.md` (flujo de ramas) |
| 10 — YAGNI | Sin tablas con FK en v1 |
| 11 — Ambigüedad marcada | C1–C5 en `2_spec.md` |

Sin desviaciones a la constitución en esta versión.