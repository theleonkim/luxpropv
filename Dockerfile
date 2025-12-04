# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy csproj and restore
COPY *.sln .
COPY Luxprop/*.csproj ./Luxprop/
COPY Luxprop.Business/*.csproj ./Luxprop.Business/
COPY Luxprop.Data/*.csproj ./Luxprop.Data/
COPY Luxprop.Database/*.csproj ./Luxprop.Database/
COPY Luxprop.Global/*.csproj ./Luxprop.Global/
COPY Luxprop.Web/*.csproj ./Luxprop.Web/

RUN dotnet restore

# Copy everything and publish
COPY . .
RUN dotnet publish -c Release -o /out

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "Luxprop.Web.dll"]
