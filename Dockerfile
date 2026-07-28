# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

# Install Node.js 22 LTS
RUN curl -fsSL https://deb.nodesource.com/setup_22.x | bash - \
    && apt-get install -y nodejs \
    && rm -rf /var/lib/apt/lists/*

WORKDIR /src

# Store NuGet packages inside the image layer so they are available to all
# subsequent RUN steps. The BuildKit cache mount is used only for the HTTP
# download cache (~/.local/share/NuGet/http-cache) so repeated builds on the
# same host skip re-downloading without hiding packages from later stages.
ENV NUGET_PACKAGES=/nuget/packages

# Restore .NET packages (layer cached until .csproj files change)
COPY Whist.sln .
COPY Whist.Rules/Whist.Rules.csproj Whist.Rules/
COPY Whist.Rules.Tests/Whist.Rules.Tests.csproj Whist.Rules.Tests/
COPY Whist.Server/Whist.Server.csproj Whist.Server/
COPY Whist.Server.Tests/Whist.Server.Tests.csproj Whist.Server.Tests/
RUN --mount=type=cache,id=nuget-http,target=/root/.local/share/NuGet/http-cache \
    dotnet restore

# Install npm packages (layer cached until package-lock.json changes)
COPY Whist.Server/ClientApp/package.json Whist.Server/ClientApp/package-lock.json Whist.Server/ClientApp/
RUN --mount=type=cache,id=npm,target=/root/.npm \
    npm ci --prefix Whist.Server/ClientApp

# Copy remaining source and build
COPY . .
RUN dotnet build --no-restore -c Release

# Run all tests (fails the build if any test fails)
FROM build AS test
RUN dotnet test --no-build -c Release --blame-hang-timeout 1m
RUN npm --prefix Whist.Server/ClientApp test

# Publish (only reachable if all tests pass)
FROM test AS publish
RUN dotnet publish Whist.Server/Whist.Server.csproj -c Release --no-build -o /app/publish

# Export publish output for CI artifact extraction
FROM scratch AS export
COPY --from=publish /app/publish /

# Minimal runtime image for container deployment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Whist.Server.dll"]
