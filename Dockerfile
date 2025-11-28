#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Create non-root user
RUN useradd -m -u 1000 appuser

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["src/cafedebug-backend.api/cafedebug-backend.api.csproj", "src/cafedebug-backend.api/"]
COPY ["src/cafedebug-backend.domain/cafedebug-backend.domain.csproj", "src/cafedebug-backend.domain/"]
COPY ["src/cafedebug.backend.application/cafedebug.backend.application.csproj", "src/cafedebug.backend.application/"]
COPY ["cafedebug-backend.infrastructure/cafedebug-backend.infrastructure.csproj", "cafedebug-backend.infrastructure/"]
RUN dotnet restore "src/cafedebug-backend.api/cafedebug-backend.api.csproj"
COPY . .
WORKDIR "/src/src/cafedebug-backend.api"
RUN dotnet build "cafedebug-backend.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "cafedebug-backend.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set non-root user
USER appuser

# Healthcheck - adjust the URL path based on your API health endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
    CMD curl -f http://localhost:80/health || exit 1

ENTRYPOINT ["dotnet", "cafedebug-backend.api.dll"]
