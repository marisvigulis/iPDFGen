FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["iPDFGen.Server/iPDFGen.Server.csproj", "iPDFGen.Server/"]
RUN dotnet restore "iPDFGen.Server/iPDFGen.Server.csproj"
COPY . .
RUN dotnet publish "iPDFGen.Server/iPDFGen.Server.csproj" \
    -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM ubuntu:24.04 AS final
RUN apt-get update \
    && apt-get install -y --no-install-recommends aspnetcore-runtime-10.0 ca-certificates \
    && apt-get clean && rm -rf /var/lib/apt/lists/*
RUN useradd -m -u 1001 -s /bin/bash app
WORKDIR /app
COPY --chown=app:app --from=build /app/publish .
RUN apt-get update \
    && dotnet iPDFGen.Server.dll install-deps \
    && apt-get clean && rm -rf /var/lib/apt/lists/*
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
USER app
ENTRYPOINT ["dotnet", "iPDFGen.Server.dll"]
