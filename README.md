# BackEndDevsu — API Bancaria

Backend del sistema bancario. Implementa CRUD de Clientes, Cuentas y Movimientos.

## Tecnologías
- .NET 8 / C#
- Entity Framework Core
- SQL Server
- Docker / Docker Compose
- xUnit para pruebas unitarias
- Swagger / OpenAPI

## Estructura
- BankDevsu.Api → controladores, Swagger.
- BankDevsu.Application → servicios, DTOs, casos de uso.
- BankDevsu.Domain → entidades y reglas de dominio.
- BankDevsu.Infrastructure → DbContext, repositorios, migraciones.
- BankDevsu.Test → pruebas unitarias.
- DataBase.sql → script inicial de BD.
- docker-compose.yml → levantar API + SQL Server.

## Configuración

### 1) Clonar

git clone https://github.com/<tu-usuario>/BankSystemDevsu.git
cd BackEndDevsu

### 2) Lebantar todo en Docker
docker-compose down -v
docker-compose up --build

### 3) Visitar el sitio

http://localhost:32768/index.html


# FrontEndDevsu — Aplicación Angular

Frontend administrar Clientes, Cuentas, Movimientos.

## Tecnologías
- Angular 16
- TypeScript
- HTML / CSS (sin frameworks)

## Configuración

### 1) Clonar e instalar dependencias

git clone https://github.com/<tu-usuario>/BankSystemDevsu.git
cd FrontEndDevsu
npm install

### 2) Lebantar el servidor

ng serve

### 3) Visitar el sitio

http://localhost:32768/swagger


# Pruebas Unitarias

El proyecto incluye pruebas unitarias para garantizar el correcto funcionamiento del backend.

## Backend (xUnit)

### Ejecutar pruebas:

cd BackEndDevsu/BankDevsu.Test
dotnet test


Qué se prueba:

CRUD de Client mediante GenericRepository.

CRUD y búsquedas de Account, incluyendo su relación con Client.

Lógica de MovementService:

Créditos y débitos.

Límite diario de retiro (1000$).

Manejo de saldo insuficiente.
