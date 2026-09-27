# 7_quickstart.md

## 0. Antes de arrancar (por la decisión C2/D1)
Los dos repos deben quedar clonados como carpetas hermanas, con el del
front llamado EXACTAMENTE `front_innovacion`:
```
proyecto/
├── <nombre-repo-api>/        <- aquí corres docker compose
└── front_innovacion/         <- nombre EXACTO, obligatorio
```

## 1. Arranque
```bash
cd <nombre-repo-api>
docker compose up -d --build
```
La primera vez tarda varios minutos. Revisa con `docker compose logs -f`.

## 2. Smoke test (recorre 2_spec.md §5)
```bash
curl http://localhost:8072/api/universidads              # total: 6
curl http://localhost:8072/api/area-conocimientos         # total: 218
curl -i http://localhost:8072/api/aliados/9999            # 404

curl -i -X POST http://localhost:8072/api/enfoques \
  -H "Content-Type: application/json" -d '{"id":1,"nombre":"Prueba"}'  # 422

curl -i -X PUT http://localhost:8072/api/car-innovacions/1 \
  -H "Content-Type: application/json" -d '{"nombre":"X"}'             # 422
curl -i -X PATCH http://localhost:8072/api/car-innovacions/1 \
  -H "Content-Type: application/json" -d '{"nombre":"X"}'             # 200

curl -i -X DELETE http://localhost:8072/api/practica-estrategias/1     # 200
curl -i -X DELETE http://localhost:8072/api/practica-estrategias/1     # 404
```

## 3. Front
`http://localhost:8073` — las 7 pantallas en el menú, cada una lista y
crea contra la API real.

## 4. Si algo falla
| Síntoma | Causa probable |
|---|---|
| El build del front falla con ruta no encontrada | El repo del front no está clonado como `front_innovacion`, hermano del repo de la API (D1) |
| 500 al listar | Cadena de conexión mal puesta en `appsettings.json`, o `init.sh` no terminó de cargar el script |
| El front no lista nada | La API está apagada o cambiaron el puerto 8072 |