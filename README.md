# Ecommerce API - .NET 8, Clean Architecture y EF Core

## 1. Arquitectura

La solucion respeta la direccion de dependencias solicitada: `Domain <- Application <- Infrastructure <- Presentation`.

- `Ecommerce.Domain`: contiene solamente entidades y reglas centrales del modelo. No conoce EF Core, Controllers ni JWT.
- `Ecommerce.Application`: contiene DTOs, interfaces y casos de uso. Depende de `Domain`, pero no depende de `Infrastructure`.
- `Ecommerce.Infrastructure`: implementa persistencia, repositorios, servicios JWT y BCrypt. Depende de las abstracciones de `Application`.
- `Ecommerce.Presentation`: expone Controllers, configura Dependency Injection, Swagger, JWT y EF Core.

La decision principal de Clean Architecture es que la logica de aplicacion trabaja contra interfaces (`IProductRepository`, `ICategoryRepository`, `IJwtTokenService`, etc.). De ese modo, los detalles tecnicos quedan invertidos: Infrastructure conoce Application, pero Application no conoce Infrastructure.

## 2. Estructura de carpetas

```text
Ecommerce.Domain/
  Entities/
Ecommerce.Application/
  DTOs/
  Interfaces/
  UseCases/
Ecommerce.Infrastructure/
  Configurations/
  Migrations/
  Persistence/
  Repositories/
  Services/
Ecommerce.Presentation/
  Controllers/
  Program.cs
  appsettings.json
```

## 3. Relaciones entre entidades

- `Category 1 - N Product`: una categoria agrupa muchos productos.

Las relaciones se configuran con Fluent API en `Ecommerce.Infrastructure/Configurations`. Se usa `DeleteBehavior.Restrict` entre categoria y producto para evitar borrados accidentales cuando existen productos asociados.

Por esa razon, antes de eliminar una categoria se debe eliminar o reasignar cada producto asociado. La API valida este caso y responde `400 Bad Request` con un mensaje claro.

## 4. Dependency Injection

`Program.cs` registra las interfaces de Application con sus implementaciones de Infrastructure y tambien registra interfaces para los casos de uso:

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IProductUseCase, ProductUseCase>();
```

Esto aplica inversion de dependencias: los Controllers dependen de interfaces de casos de uso y los casos de uso dependen de interfaces de repositorios/servicios, no de clases concretas de EF Core.

## 5. Authentication vs Authorization

- Authentication: valida identidad. El endpoint `POST /api/auth/login` emite un JWT cuando email y password son correctos.
- Authorization: decide permisos. `[Authorize]` exige token valido y `[Authorize(Roles = "Admin")]` exige rol `Admin`.

El JWT incluye claims de identificador, email y rol. Los Controllers leen el identificador con `ClaimTypes.NameIdentifier`.

## 6. Migraciones

Se incluye una migracion inicial de referencia en `Ecommerce.Infrastructure/Migrations/20260522000000_InitialCreate.cs`.

Comandos esperados con SDK instalado:

```bash
dotnet restore
dotnet ef database update --project Ecommerce.Infrastructure --startup-project Ecommerce.Presentation
dotnet run --project Ecommerce.Presentation
```

## 7. Swagger y Postman

Al ejecutar en Development, Swagger queda disponible en:

```text
https://localhost:{puerto}/swagger
```

Flujo recomendado:

1. Registrar usuario: `POST /api/auth/register`.
2. Login: `POST /api/auth/login`.
3. Copiar el token.
4. En Swagger, usar `Authorize` con el valor del JWT.
5. Probar endpoints protegidos como `GET /api/users/me`.

Para endpoints Admin se requiere un usuario cuyo rol en base de datos sea `Admin`.

## 8. Ejemplos

Registro:

```http
POST /api/auth/register
Content-Type: application/json

{
  "firstName": "Ana",
  "lastName": "Perez",
  "email": "ana@example.com",
  "password": "Password123!"
}
```

Login:

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "ana@example.com",
  "password": "Password123!"
}
```

Crear categoria como Admin:

```http
POST /api/categories
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Notebooks",
  "description": "Computadoras portatiles"
}
```

Crear producto como Admin:

```http
POST /api/products
Authorization: Bearer {token}
Content-Type: application/json

{
  "name": "Notebook Pro",
  "description": "Notebook para trabajo profesional",
  "price": 1500,
  "stock": 10,
  "categoryId": "{categoryId}"
}
```

## 9. Codigos HTTP aplicados

- `200 OK`: consultas exitosas y login.
- `201 Created`: registro, categorias y productos creados.
- `204 NoContent`: actualizaciones y eliminaciones exitosas.
- `401 Unauthorized`: credenciales invalidas o token ausente/invalido.
- `403 Forbidden`: token valido sin rol requerido.
- `404 NotFound`: recurso inexistente.

## 10. Observacion academica

No se usaron Minimal APIs, Identity, MediatR, AutoMapper ni patrones externos. La solucion se mantiene dentro de Controllers, EF Core, Repository Pattern, DTOs, JWT, roles, DbContext, Fluent API, migraciones, Swagger, SOLID y Dependency Injection.
