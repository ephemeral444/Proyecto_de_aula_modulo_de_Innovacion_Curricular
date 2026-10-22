# 5_data_model.md — Inventario de datos de V2

## 1. Fuente de verdad
El esquema físico de las tablas nuevas debe transcribirse desde el script oficial `db_scripts/sqlserver/innovacion_curricular.ss.sql` entregado por el curso. Ese archivo no está en el ZIP recibido. Por eso esta versión del documento registra las relaciones y claves conocidas, pero **no inventa columnas, tipos, nulabilidad, claves primarias ni columnas `activo`**. Completar el diccionario físico antes de implementar el SQL.

## 2. Tablas V1 que deben conservarse
`area_conocimiento`, `universidad`, `aspecto_normativo`, `practica_estrategia`, `enfoque`, `car_innovacion`, `aliado`.

El script incluido en el ZIP define las siete tablas y sus claves; las columnas exactas se consultan en `db/innovacion_curricular.sql`. Se deben conservar datos y contratos existentes y migrar sus CRUD a procedimientos almacenados.

## 3. Tablas V2 y relaciones conocidas
| Tabla | Relación conocida | Maestro / padre | Clave conocida |
|---|---|---|---|
| `facultad` | Facultad pertenece a universidad | `universidad` | Debe confirmarse en SQL oficial |
| `programa` | Programa pertenece a facultad | `facultad` | Debe confirmarse en SQL oficial |
| `acreditacion` | Acreditación pertenece a programa | `programa` | `resolucion` como PK, según consigna |
| `registro_calificado` | Registro calificado pertenece a programa | `programa` | `codigo` como PK, según consigna |
| `activ_academica` | Actividad académica asociada al programa/registro según esquema | Confirmar SQL oficial | Debe confirmarse en SQL oficial |
| `pasantia` | Pasantía pertenece a programa | `programa` | Debe confirmarse en SQL oficial |
| `premio` | Premio pertenece a programa e incluye entidad otorgante | `programa` y entidad indicada por esquema | Debe confirmarse en SQL oficial |
| `docente_departamento` | Vinculación docente ↔ programa; datos de dedicación, modalidad y fechas | `programa` y referencia docente según esquema | Debe confirmarse en SQL oficial |
| `alianza` | Alianza entre aliado y programa, con fechas y docente responsable | `aliado`, `programa` y docente responsable | Debe confirmarse en SQL oficial |
| `programa_ac` | Puente programa ↔ área de conocimiento | `programa`, `area_conocimiento` | Clave compuesta por confirmar |
| `programa_pe` | Puente programa ↔ práctica/estrategia | `programa`, `practica_estrategia` | Clave compuesta por confirmar |
| `programa_ci` | Puente programa ↔ característica de innovación | `programa`, `car_innovacion` | Clave compuesta por confirmar |
| `an_programa` | Puente aspecto normativo ↔ programa | `aspecto_normativo`, `programa` | Clave compuesta por confirmar |
| `enfoque_rc` | Puente enfoque ↔ registro calificado | `enfoque`, `registro_calificado` | Clave compuesta por confirmar |
| `aa_rc` | Puente actividad académica ↔ registro calificado, con componente y semestre | `activ_academica`, `registro_calificado` | Clave compuesta por confirmar |

## 4. Relaciones maestro-detalle exigidas
| Maestro | Detalles |
|---|---|
| `universidad` | `facultad` |
| `facultad` | `programa` |
| `programa` | `acreditacion`, `activ_academica`, `pasantia`, `premio`, `registro_calificado` |
| `programa` | Asociaciones de `docente_departamento`, `alianza` y tablas puente que correspondan al esquema |

El listado de detalles es una relación funcional inicial basada en la consigna. La cardinalidad exacta, las claves y las restricciones deben verificarse contra el SQL oficial.

## 5. Diccionario físico pendiente
Para cada una de las 15 tablas se debe completar esta ficha con el SQL oficial:
- nombre exacto de cada columna;
- tipo y longitud/precisión;
- PK simple o compuesta;
- FK y tabla/columna destino;
- nulabilidad y valores por defecto;
- presencia de `activo` y regla de borrado;
- índices y restricciones únicas/check;
- columnas que el trigger puede calcular o actualizar.

No comenzar la fase de DDL/modelos hasta cerrar este diccionario.

## 6. Invariantes
- Las claves FK siempre apuntan a filas válidas según restricciones de SQL Server.
- Una tabla puente no permite repetir la misma pareja de claves.
- Los campos calculados por la base de datos o el trigger no los debe mandar el formulario/API.
- Si existe `activo`, el listado normal solo incluye filas activas.
- El modelo C# debe coincidir con los tipos del esquema real, no con suposiciones.
