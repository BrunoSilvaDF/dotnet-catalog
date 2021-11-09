# Course Status

https://youtu.be/ZXdFisA_hOY?t=17784

### Tips

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

### Health Checks

https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks

`dotnet user-secrets set MongoDbSettings:Password Pass#word` :: adiciona o segredo ao .NET SecretManager

=> a chave é o nome que estiver no appsettings

`dotnet add package AspNetCore.HealthChecks.MongoDb` :: adiciona o Health Check do MongoDB
