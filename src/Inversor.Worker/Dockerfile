FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app


# Copy csproj files and restore dependencies
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Inversor.Worker/Inversor.Worker.csproj", "src/Inversor.Worker/"]
COPY ["src/Inversor.Core/Inversor.Core.csproj", "src/Inversor.Core/"]
COPY ["src/Inversor.Infrastructure/Inversor.Infrastructure.csproj", "src/Inversor.Infrastructure/"]
RUN dotnet restore "./src/Inversor.Worker/Inversor.Worker.csproj"
COPY . .
WORKDIR "/src/src/Inversor.Worker"
RUN dotnet build "./Inversor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Inversor.Worker.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inversor.Worker.dll"]