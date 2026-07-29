# syntax=docker/dockerfile:1

# ── Client build stage ─────────────────────────────────────────────────────────
FROM node:22-alpine AS client-build
WORKDIR /client

# Install npm packages (layer cached until package-lock.json changes)
COPY client/package.json client/package-lock.json ./
RUN --mount=type=cache,id=npm,target=/root/.npm \
    npm ci

# Copy source and run tests, then build
COPY client/ .
RUN npm test
RUN npm run build

# ── .NET build stage ───────────────────────────────────────────────────────────
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

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

# Copy remaining source and build
COPY . .
RUN dotnet build --no-restore -c Release

# Run .NET tests (fails the build if any test fails)
FROM build AS test
RUN dotnet test --no-build -c Release --blame-hang-timeout 1m

# Publish (only reachable if all tests pass)
FROM test AS publish
# Copy client build output into wwwroot before publishing
COPY --from=client-build /client/dist ./Whist.Server/wwwroot/
RUN dotnet publish Whist.Server/Whist.Server.csproj -c Release --no-build -o /app/publish

# Minimal runtime image for container deployment
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Whist.Server.dll"]
