# SolaceTK Core Service
[![solace-toolkit](https://img.shields.io/badge/SolaceTK-Home-green)](https://github.com/JRBowman/solace-toolkit)
[![solacetk](https://img.shields.io/badge/SolaceTK-API%20Demo-blue)](https://solacetk-core-dev-bowman-dev.apps.bocp.onbowman.com/swagger)

The SolaceTK Core Service provides an API for Tool Kit UI to perform common CRUD operations as well as File Upload support, this service uses `Entity Framework` (EF) as the 'ORM' to interact with SQL databases (EF supports using many DBs as sources using the proper SQLConnectionProvider Nuget Package.)
- `Version 1` of the Core Services is delivered through a single `.NET 7` ASP.NET WebAPI for ease of use and debugging.
- `Version 2` will be delivered as microservices, the current 'boilerplate' has a number of EF Contexts to define the different segmentation of data.
- `Swashbuckle` provides the Swagger/OpenAPI3 API Documentation and UI: `https://*:<port>/swagger` (this will redirect to the Swagger UI)
- `ASP.NET Identity` & `API Authentication` is disabled by default in `Version 1` but is fully configured for it using an OAuth/OIDC Identity Provider of your choosing
- Check the `Startup.cs` file for the configuration, .NET WebAPIs most commonly make use of Bearer Token Auth and all of the .NET Service Middleware code is implemented.
- See the `solacetk-identity` repository for more information on using `.NET 7` to host your own Central Token Authority / Claims-based Identity Provider using `Identity Server Nuget Pacakge` + `ASP.NET Identity` + `Entity Framework`
- Using `Identity Server` allows you to host an entire Identity Provider/Service Provider solution as a Source-Controlled Microservice, maximum control over integrations and application lifecycle.
- `solacektk-identity` repository provides a full working example to deploy alongside the `solacetk-service`.
- [![solacetk](https://img.shields.io/badge/SolaceTK-API%20Demo-blue)](https://solacetk-core-dev-bowman-dev.apps.bocp.onbowman.com/swagger) - Demo of the Service can be accessed here!

# Application Runtime Configuration:
The example `kubernetes` secret below shows the configurations that can be applied via `Environment Variables`. Additional Configuration Options using the common .NET `appsettings.json` will be added as development continues!
- `SOLTK_AUTHORITY` -> specifies the host and port of the `Identity Provider` it should be using as the Authority.
- `SOLTK_ENABLE_TLS` -> true/false, configures the .NET Kestrel Server to enable HTTPS and TLS - Certificates must be mounted to `/app/certs/` and include both `tls.crt` and `tls.key`
- Specified path is a container path, local development can use self-signed certs or provide certs in the app's build output folder.
- Using `OpenShift Service Certificates` in a container-based deployment allows you to automatically enable end-to-end encryption without obtaining any certs!
- `SOLTK_CONNECTIONSTRING` -> The ConnectionString that Entity Framework will use during runtime to Connect, Create, Migrate, and Transact with a database.
- If *no* ConnectionString is provided, `solacetk-service` will just an `InMemory` database provided by EF.

```
kind: Secret
apiVersion: v1
metadata:
  name: solacetk-config-secret
data:
  ASPNETCORE_ENVIRONMENT: 'Development'
  ASPNETCORE_URLS: 'http://*:8080'
  SOLTK_AUTHORITY: 'https://localhost:5000'
  SOLTK_ENABLE_TLS: 'false'
  SOLTK_CONNECTIONSTRING: ''
type: Opaque
```

Check out the `Program.cs` to see the different Global Static variables being loaded at runtime from Environment Variables and the `Startup.cs` to see those Static Variables being used to configure the Middleware for runtime.

# Building from Source:
Two options currently exist to build the `solacetk-service` (outside of creating your own ways)
1. Build using `Docker` or `Podman+Buildah` with the provided `Dockerfile` in the root of the repo.
2. Build using `dotnet sdk` with the usual `dotnet build ./path/to/.csproj`

Using method `1` will include the underlying dependency for using `Aseprite` to load `.ase` files and process them - `Aseprite` isn't required for the API to function, it's only needed when you intend to use the `Animations` features with the `Aseprite` Integration.

Using method `2` will produce the application.dll and it's dependencies and is ready to be deployed or ran. (This method doesn't automatically include aseprite in the runtime environment and Aseprites features will be unavailable.)
