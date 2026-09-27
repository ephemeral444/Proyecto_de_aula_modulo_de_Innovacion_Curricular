# 5_data_model.md

## 1. Las 7 tablas que esta versión puede nombrar
Del script oficial del módulo (`innovacion_curricular.ss.sql`), con
`activo BIT NOT NULL DEFAULT 1` agregado (defecto del script original,
corregido — ver PLAN_V1.md del ejemplo del profesor).

### area_conocimiento
| Columna | Tipo | Regla |
|---|---|---|
| id | VARCHAR(6) | PK; código alfanumérico, lo asigna quien crea |
| gran_area | VARCHAR(60) | obligatorio |
| area | VARCHAR(60) | obligatorio |
| disciplina | VARCHAR(150) | obligatorio |
| activo | BIT | default 1 |

### universidad
| Columna | Tipo | Regla |
|---|---|---|
| id | INT | PK; la asigna quien crea (D3) |
| nombre | VARCHAR(60) | obligatorio |
| tipo | VARCHAR(45) | obligatorio |
| ciudad | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

### aspecto_normativo
| Columna | Tipo | Regla |
|---|---|---|
| id | INT | PK |
| tipo | VARCHAR(45) | obligatorio |
| descripcion | VARCHAR(45) | obligatorio |
| fuente | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

### practica_estrategia
| Columna | Tipo | Regla |
|---|---|---|
| id | INT | PK |
| tipo | VARCHAR(45) | obligatorio |
| nombre | VARCHAR(45) | obligatorio |
| descripcion | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

### enfoque
| Columna | Tipo | Regla |
|---|---|---|
| id | INT | PK |
| nombre | VARCHAR(45) | obligatorio |
| descripcion | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

### car_innovacion
| Columna | Tipo | Regla |
|---|---|---|
| id | INT | PK |
| nombre | VARCHAR(45) | obligatorio |
| descripcion | VARCHAR(MAX) | obligatorio, sin límite de longitud |
| tipo | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

### aliado
| Columna | Tipo | Regla |
|---|---|---|
| nit | INT | PK; la asigna quien crea |
| razon_social | VARCHAR(60) | obligatorio |
| nombre_contacto | VARCHAR(60) | obligatorio |
| correo | VARCHAR(70) | obligatorio |
| telefono | VARCHAR(45) | obligatorio (texto, nunca aritmética) |
| ciudad | VARCHAR(45) | obligatorio |
| activo | BIT | default 1 |

## 2. Semillas exactas (del Excel del módulo)
| Tabla | Filas |
|---|---|
| area_conocimiento | 218 |
| universidad | 6 |
| aspecto_normativo, practica_estrategia, enfoque, car_innovacion, aliado | 0 (arrancan vacías) |

## 3. Invariantes
| Dato | Quién lo calcula | Quién tiene prohibido escribirlo |
|---|---|---|
| La llave (id/nit) | Quien crea, en el body del POST | La base nunca la autogenera (sin IDENTITY) |
| `activo` en creación | La base (`DEFAULT 1`) | La API no lo envía en el INSERT |
| `activo` en borrado | El repositorio (`UPDATE...SET activo=0`) | Nadie hace `DELETE FROM` físico |
| Tablas con clave foránea | Fuera de v1 | La API de v1 no las consulta ni las escribe |

## 4. Estados
```
[activo = 1] --DELETE--> [activo = 0]
[activo = 0] --DELETE--> 404 (sin transición de vuelta en v1)
```