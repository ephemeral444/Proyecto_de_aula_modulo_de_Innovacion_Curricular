#!/bin/bash

echo "Esperando a que SQL Server esté listo..."

# Intentar conectarse a SQL Server en un bucle compatible con cualquier shell
for i in $(seq 1 30); do
    /opt/mssql-tools18/bin/sqlcmd -S db -U sa -P 'TuPasswordSegura123!' -C -Q "SELECT 1" > /dev/null 2>&1
    if [ $? -eq 0 ]; then
        echo "SQL Server está activo y respondiendo."
        break
    fi
    echo "SQL Server aún no está listo, reintentando en 2 segundos... ($i/30)"
    sleep 2
done

echo "Ejecutando script DDL de innovación curricular..."
/opt/mssql-tools18/bin/sqlcmd -S db -U sa -P 'TuPasswordSegura123!' -C -i /db/innovacion_curricular.sql

echo "Base de datos inicializada correctamente."