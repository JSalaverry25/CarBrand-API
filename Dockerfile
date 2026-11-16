
# Etapa 1: compilar y publicar
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar archivos de proyectos
COPY CarBrand.Api/CarBrand.Api.csproj CarBrand.Api/
COPY CarBrand.Application/CarBrand.Application.csproj CarBrand.Application/
COPY CarBrandDomain/CarBrand.Domain.csproj CarBrandDomain/
COPY CarBrand.Infrastructure/CarBrand.Infrastructure.csproj CarBrand.Infrastructure/

# Restaurar dependencias
RUN dotnet restore CarBrand.Api/CarBrand.Api.csproj

# Copiar el resto del código
COPY . .

# Publicar API
WORKDIR /src/CarBrand.Api
RUN dotnet publish CarBrand.Api.csproj -c Release -o /app/publish --no-restore

# Etapa 2: ejecutar API
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .


EXPOSE 8080

ENTRYPOINT ["dotnet", "CarBrand.Api.dll"]
