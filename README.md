# PetStore - MicroServices


## Preparar Docker
1. Crear una red
```
docker network create --attachable -d bridge petstore-network
```

2. Correr el **docker-compose.yml**
```
docker-compose up -d
```

---
## [ Paso 1 ] Nuevo Servicio - Solución
1. Crear **Blank Solution** y la carpeta física **src** al lado del archivo **.sln**
2. Crear la estructura del proyecto - Carpetas de Solución
    - 1_Api
    - 2_Application
    - 3_Data
    - 4_Events

3. Crear los proyectos
    - src/Data/Persistence = **ClassLibrary**
    - src/Data/Domain = **ClassLibrary**
    - src/Application = **ClassLibrary**
    - src/Events = **ClassLibrary**
    - src/Api = **WebApi**
        - [x] Configure for HTTPS
        - [x] Enable OpenAPI support
        - [x] Use controllers

4. Instalar paquetes **Nuget**
    - Sobre **Application:**
        - Microsoft.EntityFrameworkCore.Relational
        - Microsoft.EntityFrameworkCore.SqlServer
        - MediatR
        - FluentValidation.AspNetCore
        - AutoMapper
        - RabbitMQ.Client (_opcional_)
    - Sobre **Api:**
        - Microsoft.EntityFrameworkCore.SqlServer
        - Microsoft.EntityFrameworkCore.Design
    - Sobre **Persistence:**
        - Microsoft.EntityFrameworkCore
        - Microsoft.EntityFrameworkCore.Relational
        - Microsoft.EntityFrameworkCore.SqlServer
5. Crear clases
    - **Domain:** Todas las clases pertenecientes al dominio o entidades que usaremos para almacenar la información en las bases de datos.
    
    - **Persistence:** 
        - Agregar la referencia a **Domain**
        - Agregar la clase del **Contexto** y los **DbSets**.

    - **Application:** 
        - Agregar la referencia a **Domain** - **Persistence** y **Events**
        - Crear las carpetas de la lógica del negocio y los archivos. 
            - DTOs
            - Settings (_opcional_)
            - Producers (_opcional_)
            - CQRS / Entidad / Commands o Queries => (_Los commandos o las queries, validaciones y los handlers -  **Mediator**_)
            - Mapping
            - Exceptions (_opcional_)


>:warning: **Importante** Tener cuidado con los **namespaces**!

6. Crear clase Injection en **Application**
    - Agregar el **Bind** de los **Settings**
    - Agregar inyección de **Mapper**
    - Agregar inyección de **FluentValidations**
    - Agregar inyección de **MediatR**

7. Agregar referencia de **Application** a **Api**
8. Agregar referencia de **Persistence** a **Api**
9. En Program definir variable **config**
```cs
var config =  builder.Configuration
```
10. En Program Definir Variable **env**
```cs
var env =  builder.Environment
```
11. Crear carpeta **Config** en **Api** y archivo de extensiones 
    - Agregar la configuración del **Context**
12. En **Program** agregar los servicios de **Application** y **Config**
```cs
builder.Services.AddInfrastructure(config, env);
builder.Services.AddApplication(config);
```
13. Crear **BaseController** y agregar **MediatR**
14. Crear controladores de los **endpoints** que hereden de **BaseController** y los **Métodos**
15. Agregar los **Settings** al archivo **.json**
```json
 "ConnectionStrings": {
   "DefaultConnection": "Server=localhost,1433;User Id=sa;Password=Password123;Database=animalsDB;Trusted_Connection=false;MultipleActiveResultSets=True;TrustServerCertificate=Yes"
 },
 "RabbitMq": {
   "Host": "localhost",
   "User": "guest",
   "Password": "guest"
 }
```
16. Asignar el puerto en el archivo **Properties/launchSettings.json** en el **Profile** -> **http** -> **applicationUrl**
```
 "applicationUrl": "https://localhost:4434;http://localhost:8094"
```
17. Correr comando para las **Migraciones** -> abrir la terminar el la carpeta **src**
```
dotnet ef migrations add Initial -p .\Data\Persistence\ -s .\Api\
```
18. Correr comando para actualizar la BD
```
dotnet ef database update -p .\Data\Persistence\ -s .\Api\
```
19. Asignar el proyecto **Api** como inicio y **Ejecutar**
20. Abrir la url 
```
https://localhost:4434/swagger/index.html
```
21. Probar petición en **Postman**
```json
POST -> http://localhost:8094/api/users/Users

{
  "email": "string",
  "firstName": "string",
  "lastName": "string",
  "age": 0
}
```
22. Probar que se haya creado la **Queue** en **Rabbit**
```
http://localhost:15672/#/queues
```
## [ Paso 2 ] Consumer

1. Crear la nueva **Entidad** en **Data/Domain**
2. Agregar el **DbSet** en el **Context**
3. Crear y Mapear el **Event**
4. Crear el **Mapping**
5. Crear el **Consumer**
6. Agregar la inyección del nuevo **Consumer**
7. Comentar los otros **Listener**
8. Correr el nuevo **Listener**

## [ Paso 3 ] Search

1. Agregar un nuevo **EndPoint** en el servicio de **Search** que obtenga los registros de la nueva entidad
2. Agregar un nuevo **EndPoint** en el servicio de **Search** que obtenga un registro por su **ID**

## [ Paso 4 ] Gateway

1. Agregar el nuevo nodo del nuevo **servicio** al archivo **ocelot.json** que me solicite **Autorización** para **Crear**
2. Agregar el nuevo nodo del nuevo **servicio** al archivo **ocelot.json** que me **NO** solicite **Autorización** para **Buscar**



