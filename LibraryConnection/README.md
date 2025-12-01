# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test
TODO: Describe and show how to build your code and run the tests. 

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)

# Run Migrations (correct startup project)
Siempre ejecuta los comandos desde el proyecto API `MSSnackGol`, porque ahí están los `appsettings.json` y se carga el `ConnectionStrings:LocalHostConnection`. Si ejecutas con `--startup-project ./` dentro de `LibraryConnection`, la conexión queda vacía y verás el error: "The ConnectionString property has not been initialized".

Pasos:
- `dotnet tool install --global dotnet-ef`
- `dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL`

Desde `MSSnackGol`:
```bash
pushd "../MSSnackGol"
dotnet ef migrations add <Nombre> --project ../LibraryConnection --startup-project ./
dotnet ef database update --project ../LibraryConnection --startup-project ./
popd
```

# Reset rápido en desarrollo
Para limpiar y recrear con datos sembrados:
```bash
pushd "../MSSnackGol"
dotnet ef database drop --project ../LibraryConnection --startup-project ./ --force
dotnet ef database update --project ../LibraryConnection --startup-project ./
popd
```

# Nota: InMemory para pruebas
También puedes activar la BD en memoria exportando `USE_INMEMORY_DB=1` antes de ejecutar la API. No uses `dotnet ef database update` en ese modo.

