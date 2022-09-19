FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY . .
RUN dotnet publish "Concerto.Orchestrator/Concerto.Orchestrator.csproj" -c Release -o /app \
    --runtime alpine-x64 \
    --self-contained true \
    /p:PublishTrimmed=false \
    /p:PublishSingleFile=true

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine AS base
WORKDIR /app
ENTRYPOINT ["./Concerto.Orchestrator"]
COPY --from=build /app .