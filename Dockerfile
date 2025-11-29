# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj files first for caching
COPY ["src/cafedebug-backend.api/cafedebug-backend.api.csproj", "src/cafedebug-backend.api/"]
COPY ["src/cafedebug-backend.domain/cafedebug-backend.domain.csproj", "src/cafedebug-backend.domain/"]
COPY ["src/cafedebug.backend.application/cafedebug.backend.application.csproj", "src/cafedebug.backend.application/"]
COPY ["src/cafedebug-backend.infrastructure/cafedebug-backend.infrastructure.csproj", "src/cafedebug-backend.infrastructure/"]

RUN dotnet restore "src/cafedebug-backend.api/cafedebug-backend.api.csproj"

COPY . .
WORKDIR "/src/src/cafedebug-backend.api"

RUN dotnet publish "cafedebug-backend.api.csproj" \
    -c Release \
    -o /app/publish \
    /p:UseAppHost=false 

# Stage 2: Final Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Install curl for healthcheck (standard images usually have it, but ensuring it's there)
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Non-root user setup (standard in MS images now, but good to be explicit with ports)
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_CLI_TELEMETRY_OPTOUT=1

USER $APP_UID

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "cafedebug-backend.api.dll"]