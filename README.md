# .NET 5 Rest API Tutorial

Source: https://youtu.be/ZXdFisA_hOY

### Tips/Commands

`dotnet new webapi -n Catalog`:: create project

`dotnet build` :: project build

`dotnet run` :: running

`dotnet dev-certs https --trust` :: add https certificate

`dotnet user-secrets init` :: init .NET SecretManager
`dotnet user-secrets set MongoDbSettings:Password Pass#word` :: add .NET SecretManager key/secret

`docker build -t brunoosilva/catalog:v2 .` docker build image

`kubectl create secret generic catalog-secrets --from-literal=mongodb-password=Pass#word` :: create kubernetes secret

`kubectl config current-context` :: config kubernetes

`kubectl apply -f .\catalog.yaml`:: create kubernetes for api

`kubectl apply -f .\mongodb.yaml` :: create kubernetes for db

`kubectl get deployments` :: get kubernetes

`kubectl delete deployment catalog-deployment` :: delete deployment

`kubectl get pods` :: get pods

`kubectl get statefulsets` :: get statefulsets

`kubectl scale deployments/catalog-deployment --replicas=3` :: escalar

`kubectl logs <pod name> -f` :: ver log do pod

### Unit Tests

xUnit
`dotnet new xunit -n Catalog.UnitTests` :: create unit test project

`dotnet add reference ..\Catalog.API\Catalog.API.csproj` add prod project reference

`dotnet add package Microsoft.Extensions.Loggin.Abstractions` add log abstraction

`dotnet add package moq` add mock package

`dotnet test` run the tests

aldo add .NET CORE TEST EXPLORER vscode extension

`dotnet add package FluentAssertions` cool assertion lib

### Health Checks

https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

`dotnet add package AspNetCore.HealthChecks.MongoDb` :: add Health Checks to MongoDB
