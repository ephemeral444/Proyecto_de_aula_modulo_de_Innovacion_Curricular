# 7_quickstart.md — Preparación y pruebas de V2

## 1. Prerrequisitos
- Visual Studio o .NET SDK compatible con el proyecto (.NET 8).
- SQL Server disponible y acceso a la base `innovacion_curricular`.
- Repositorio API y, si se confirma la separación indicada por la consigna, repositorio Front clonado en su ubicación acordada.
- Script oficial del esquema completo del módulo. El ZIP inicial no lo incluye; no ejecutar scripts de tablas V2 inventados.

## 2. Antes de implementar
1. Confirmar que estás en la rama asignada al estudiante A y que `git status` no muestra cambios ajenos.
2. Revisar `docs/spec_kit/1_constitution.md`, `docs/spec_kit/versiones/0_mapa_versiones.md` y los nueve documentos de V1.
3. Conseguir el script SQL oficial y completar `5_data_model.md` con las columnas y restricciones reales.
4. Confirmar el repositorio real del front y mantener la estructura actual hasta resolver la diferencia entre la consigna y el ZIP.

## 3. Pruebas previstas al terminar V2
Las siguientes son pruebas de aceptación a preparar; no todas se pueden ejecutar durante la fase de especificación.

1. Levantar la base de datos y aplicar el script versionado.
2. Ejecutar la API y comprobar que cada recurso responde desde sus rutas documentadas.
3. Ejecutar el CRUD de cada tabla V1 y V2 desde la interfaz.
4. Crear un hijo seleccionando un padre activo desde el desplegable.
5. Intentar crear un hijo con una FK inválida desde una petición de prueba y verificar HTTP 409 en español.
6. Asignar dos veces la misma pareja de una tabla puente y verificar 409 en la segunda operación.
7. Retirar una asociación y comprobar que deja de aparecer en la lista.
8. Abrir un maestro y verificar que sus detalles se muestran y se pueden agregar desde la misma pantalla.
9. Probar un envío de varios detalles con un elemento inválido y comprobar que no queda guardado parcialmente.
10. Probar el trigger haciendo la operación por la interfaz y verificando el valor calculado directamente en SQL Server.
11. Retirar un registro con borrado lógico y comprobar que desaparece del listado pero permanece en la base.
12. Repetir las pruebas de regresión definidas en `docs/spec_kit/versiones/v1/2_spec.md`.

## 4. Comandos Git orientativos en PowerShell
```powershell
git status
git branch --show-current
# Después de guardar los archivos de esta fase:
git add docs/spec_kit/versiones/v2_innovacion_curricular
git diff --cached
git commit -m "docs: define spec kit de V2 de innovacion curricular"
git push
```
No ejecutar `git push` sin revisar primero que la rama actual es la del estudiante A y que el commit contiene solo los archivos de esta fase. No hacer commits directos a `main`.

## 5. Errores comunes
- Empezar a crear entidades antes de tener el esquema oficial.
- Olvidar adaptar los CRUD de V1 a procedimientos almacenados.
- Mostrar el código FK en un campo de texto en vez de un desplegable.
- Responder 500 cuando SQL Server rechaza una FK o pareja duplicada.
- Probar solo la API y no el front.
- Subir cadenas de conexión, contraseñas o secretos al repositorio.
