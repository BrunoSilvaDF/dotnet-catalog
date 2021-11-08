# Course Status

https://youtu.be/ZXdFisA_hOY?t=10845

### Tips

`dotnet user-secrets init` :: inicia o .NET SecretManager, criando um Id no .csproj

### Health Checks

https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

`dotnet user-secrets set MongoDbSettings:Password Pass#word` :: adiciona o segredo ao .NET SecretManager

=> a chave é o nome que estiver no appsettings

`dotnet add package AspNetCore.HealthChecks.MongoDb` :: adiciona o Health Check do MongoDB
