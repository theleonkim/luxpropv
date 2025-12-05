# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos
COPY AdvancedProgramming/Luxprop/*.csproj ./Luxprop/
COPY AdvancedProgramming/Luxprop.Business/*.csproj ./Luxprop.Business/
COPY AdvancedProgramming/Luxprop.Data/*.csproj ./Luxprop.Data/
COPY AdvancedProgramming/Luxprop.Database/*.csproj ./Luxprop.Database/
COPY AdvancedProgramming/Luxprop.Global/*.csproj ./Luxprop.Global/
COPY AdvancedProgramming/Luxprop.Web/*.csproj ./Luxprop.Web/

# Restaurar dependencias del proyecto Web
RUN dotnet restore Luxprop.Web/Luxprop.Web.csproj

# Copiar TODO el repositorio
COPY . .

# Publicar en modo Release
RUN dotnet publish AdvancedProgramming/Luxprop.Web/Luxprop.Web.csproj -c Release -o /app/publish

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "Luxprop.Web.dll"]
