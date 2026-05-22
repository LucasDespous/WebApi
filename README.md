# Ecommerce API

Proyecto Backend realizado en .NET 8 utilizando Clean Architecture simple para una API de stock estilo Ecommerce

## Tecnologias y conceptos usados

- .NET 8
- ASP.NET Core Web API
- Entity Framework Core
- SQLite
- DbContext
- Migraciones
- Repository Pattern
- JWT Authentication
- Authorization con roles
- Swagger

## Arquitectura

El proyecto esta separado en cuatro capas:


Ecommerce.Domain
Ecommerce.Application
Ecommerce.Infrastructure
Ecommerce.Presentation


## Funcionalidades

- Registro de usuarios
- Login con JWT
- Usuario autenticado puede ver su perfil
- Roles: `Admin` y `User`
- Endpoints protegidos con `[Authorize]`
- Endpoints exclusivos para Admin con `[Authorize(Roles = "Admin")]`
- CRUD de categorias
- CRUD de productos
- Busqueda de productos por nombre
- Relacion entre categoria y productos

## Usuario admin

Usuario administrador ya utilizado para probar la API:

Email: lucasdespous@gmail.com
Password: Lucas1234
Rol: Admin

Con este usuario se puede iniciar sesion, copiar el token JWT y probar los demas endpoints.


http://localhost:5000/swagger
