# 6_contracts.md

## 0. Convenciones
Igual en las 7: sobre de listado `{recurso, limite, total, datos}`;
éxito de escritura `{"estado":200,"mensaje":"..."}` (o `filasAfectadas`
en PUT/PATCH/DELETE); error `{"estado":XXX,"mensaje":"...","detalle":"..."}`
(422 además trae `errores[]`).

| Recurso | Ruta (resuelta en C1) | Llave |
|---|---|---|
| area_conocimiento | `/api/area-conocimientos` | `id` (string, 6) |
| universidad | `/api/universidads` | `id` (int) |
| aspecto_normativo | `/api/aspecto-normativos` | `id` (int) |
| practica_estrategia | `/api/practica-estrategias` | `id` (int) |
| enfoque | `/api/enfoques` | `id` (int) |
| car_innovacion | `/api/car-innovacions` | `id` (int) |
| aliado | `/api/aliados` | `nit` (int) |

Los seis endpoints (GET lista, GET por llave, POST, PUT, PATCH, DELETE)
son EXACTAMENTE el patrón de `2_spec.md` §3 para las 7 rutas de arriba.
Dos ejemplos completos, el resto es el mismo patrón con sus propios campos
(`5_data_model.md`):

### `/api/universidads`
```
GET    /api/universidads                    -> 200 {recurso, limite, total, datos} | 204
GET    /api/universidads/{id}               -> 200 objeto | 404
POST   /api/universidads   {"id":7,"nombre":"...","tipo":"...","ciudad":"..."} -> 200 | 422
PUT    /api/universidads/{id}  (los 3 campos, sin id)                          -> 200 filasAfectadas | 422 | 404
PATCH  /api/universidads/{id}  (campos opcionales)                             -> 200 filasAfectadas | 404
DELETE /api/universidads/{id}                                                  -> 200 filasAfectadas | 404
```

### `/api/aliados`
```
GET    /api/aliados                          -> 200 {recurso, limite, total, datos} | 204
GET    /api/aliados/{nit}                    -> 200 objeto | 404
POST   /api/aliados {"nit":900123456,"razonSocial":"...","nombreContacto":"...","correo":"...","telefono":"...","ciudad":"..."} -> 200 | 422
PUT    /api/aliados/{nit}   (los 5 campos, sin nit)                            -> 200 filasAfectadas | 422 | 404
PATCH  /api/aliados/{nit}   (campos opcionales)                                -> 200 filasAfectadas | 404
DELETE /api/aliados/{nit}                                                      -> 200 filasAfectadas | 404
```

Las otras 5 (`area-conocimientos`, `aspecto-normativos`,
`practica-estrategias`, `enfoques`, `car-innovacions`) siguen el mismo
patrón exacto, con los campos que cada una tiene en `5_data_model.md`.