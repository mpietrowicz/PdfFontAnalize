FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Własny NuGet.Config (bez fallbacków)
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

COPY . ./

# UWAGA: robimy publish z restore w tym samym kroku i NADPISUJEMY fallback
RUN dotnet publish AnalysisOfAspose.csproj -c Release -o /out \
    --configfile /src/NuGet.Config \
    -p:RestoreFallbackFolders="" \
    -p:RestoreNoCache=true \
    --disable-parallel

# ===== RUNTIME =====
FROM mcr.microsoft.com/dotnet/runtime:10.0
WORKDIR /app

# Celowo NIE instalujemy fonts-*
RUN apt-get update && apt-get install -y fontconfig && rm -rf /var/lib/apt/lists/*
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




RUN mkdir -p /data
COPY --from=build /out ./

ENTRYPOINT ["dotnet", "AnalysisOfAspose.dll"]
