# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todos los proyectos
COPY AdvancedProgramming/Luxprop/*.csproj ./Luxprop/
COPY AdvancedProgramming/Luxprop.Business/*.csproj ./Luxprop.Business/
COPY AdvancedProgramming/Luxprop.Data/*.csproj ./Luxprop.Data/
COPY AdvancedProgramming/Luxprop.Database/*.csproj ./Luxprop.Database/
COPY AdvancedProgramming/Luxprop.Global/*.csproj ./Luxprop.Global/
COPY AdvancedProgramming/Luxprop.Web/*.csproj ./Luxprop.Web/

# Restaurar dependencias del proyecto web
RUN dotnet restore Luxprop.Web/Luxprop.Web.csproj

# Copiar TODO el repo
COPY . .

# Publicar el proyecto web (EL QUE SE EJECUTA)
RUN dotnet publish AdvancedProgramming/Luxprop.Web/Luxprop.Web.csproj -c Release -o /app/publish

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Luxprop.Web.dll"]
