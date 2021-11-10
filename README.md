# Course Status

https://youtu.be/ZXdFisA_hOY?t=19856

### Tips

`dotnet new webapi -n Catalog`:: criando o projeto

`dotnet dev-certs https --trust` :: adiciona certificado https
`dotnet user-secrets init` :: inicia o .NET SecretManager, criando um Id no .csproj

`docker build -t brunoosilva/catalog:v2 .` docker build image

`kubectl config current-context` :: config kubernetes
`kubectl create secret generic catalog-secrets --from-literal=mongodb-password=Pass#word` :: create secret
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
`dotnet new xunit -n Catalog.UnitTests` :: criando o projeto de teste
`dotnet add reference ..\Catalog.API\Catalog.API.csproj` adiciona ref do projeto ao teste
`dotnet add package Microsoft.Extensions.Loggin.Abstractions` abstrai a parte de log
`dotnet add package moq` pacote para mock
`dotnet test` executa os testes
extensão .net core test explorer para visualizar melhor os testes

### SecretManager

`dotnet user-secrets set MongoDbSettings:Password Pass#word` :: adiciona o segredo ao .NET SecretManager

### Health Checks

https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

=> a chave é o nome que estiver no appsettings

`dotnet add package AspNetCore.HealthChecks.MongoDb` :: adiciona o Health Check do MongoDB
