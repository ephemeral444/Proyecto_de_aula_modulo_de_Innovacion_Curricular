#!/bin/bash

set -e

SQLCMD="/opt/mssql-tools18/bin/sqlcmd"
PASSWORD="TuPasswordSegura123!"

 echo "Esperando a que SQL Server esté listo..."

LISTO=0
for i in $(seq 1 30); do
    if "$SQLCMD" -S db -U sa -P "$PASSWORD" -C -Q "SELECT 1" >/dev/null 2>&1; then
        LISTO=1
        break
    fi
    echo "SQL Server aún no está listo. Intento $i/30..."
    sleep 2
done

if [ "$LISTO" -ne 1 ]; then
    echo "ERROR: SQL Server no respondió a tiempo."
    exit 1
fi

echo "Comprobando si la base de datos ya existe..."
DB_EXISTE=$(
    "$SQLCMD" -S db -U sa -P "$PASSWORD" -C \
        -h -1 -W \
        -Q "SET NOCOUNT ON; SELECT CASE WHEN DB_ID(N'innovacion_curricular') IS NULL THEN 0 ELSE 1 END;" |
        tr -d '[:space:]'
)

if [ "$DB_EXISTE" = "0" ]; then
    echo "Creando la estructura inicial..."
    "$SQLCMD" -S db -U sa -P "$PASSWORD" -C -b \
        -i /db/innovacion_curricular.sql
else
    echo "La base de datos ya existe. Se omiten las inserciones iniciales."
fi

echo "Aplicando estructura y procedimientos consolidados en V2..."
"$SQLCMD" -S db -U sa -P "$PASSWORD" -C -b \
    -i /db/v2_innovacion_curricular.sql

echo "Base de datos inicializada correctamente (V2 consolidada)."
