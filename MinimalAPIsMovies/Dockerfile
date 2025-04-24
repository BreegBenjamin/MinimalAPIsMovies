# Imagen base para ejecución
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Imagen base para compilación
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copiar solo el .csproj y restaurar dependencias
COPY ["MinimalAPIsMovies.csproj", "."]
RUN dotnet restore "MinimalAPIsMovies.csproj"

# Copiar el resto del contenido y compilar
COPY . .
RUN dotnet build "MinimalAPIsMovies.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publicar
FROM build AS publish
RUN dotnet publish "MinimalAPIsMovies.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Imagen final para producción
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MinimalAPIsMovies.dll"]
