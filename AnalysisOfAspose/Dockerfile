FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
ARG APP_UID=10001
WORKDIR /app
USER $APP_UID

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
RUN printf '%s\n' \
'<?xml version="1.0" encoding="utf-8"?>' \
'<configuration>' \
'  <packageSources>' \
'    <clear />' \
'    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" />' \
'  </packageSources>' \
'  <config>' \
'    <add key="globalPackagesFolder" value="/root/.nuget/packages" />' \
'  </config>' \
'</configuration>' \
> /src/NuGet.Config

COPY ["AnalysisOfAspose/AnalysisOfAspose.csproj", "AnalysisOfAspose/"]
COPY ["CommonDocxStateAnalyze/CommonDocxStateAnalyze.csproj", "CommonDocxStateAnalyze/"]
RUN dotnet restore "AnalysisOfAspose/AnalysisOfAspose.csproj" --configfile /src/NuGet.Config
COPY . .
WORKDIR "/src/AnalysisOfAspose"
RUN dotnet build "./AnalysisOfAspose.csproj" -c $BUILD_CONFIGURATION -o /app/build --configfile /src/NuGet.Config

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AnalysisOfAspose.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false --configfile /src/NuGet.Config

FROM base AS final

# PRZEŁĄCZ NA ROOTA
USER root
ARG APP_UID=10001

RUN apt-get update && apt-get install -y \
    fontconfig \
    libfontconfig1 \
    libfreetype6 \
    libharfbuzz0b \
    libjpeg-turbo8 \
    libpng16-16 \
    libgif7 \
    libtiff6 \
    libgl1 \
    libx11-6 \
    libxext6 \
    libxrender1 \
    && (apt-get install -y libwebp7 || apt-get install -y libwebp6 || apt-get install -y libwebp5) \
    && rm -rf /var/lib/apt/lists/*

# /data + prawa
RUN mkdir -p /data \
 && chown -R ${APP_UID}:${APP_UID} /data \
 && chmod -R 0775 /data

WORKDIR /app
COPY --from=publish /app/publish .

# WRÓĆ DO NIE-ROOT
USER $APP_UID
ENTRYPOINT ["dotnet", "AnalysisOfAspose.dll"]
