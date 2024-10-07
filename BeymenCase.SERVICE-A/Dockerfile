#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["BeymenCase.SERVICE-A/BeymenCase.Service-A.csproj", "BeymenCase.SERVICE-A/"]
COPY ["BeymenCase.Configurations/BeymenCase.Configurations.csproj", "BeymenCase.Configurations/"]
COPY ["BeymenCase.Entities/BeymenCase.Core.csproj", "BeymenCase.Entities/"]
RUN dotnet restore "./BeymenCase.SERVICE-A/BeymenCase.Service-A.csproj"
COPY . .
WORKDIR "/src/BeymenCase.SERVICE-A"
RUN dotnet build "./BeymenCase.Service-A.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./BeymenCase.Service-A.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BeymenCase.Service-A.dll"]