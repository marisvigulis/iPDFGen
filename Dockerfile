FROM ubuntu:24.04 AS base
RUN apt-get update && apt-get install -y aspnetcore-runtime-8.0
RUN useradd -m -u 1001 -s /bin/bash app && mkdir -p /app && chown -R app:app /app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["iPDFGen.Server/iPDFGen.Server.csproj", "iPDFGen.Server/"]
RUN dotnet restore "iPDFGen.Server/iPDFGen.Server.csproj"
COPY . .
WORKDIR "/src/iPDFGen.Server"
RUN dotnet build "./iPDFGen.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./iPDFGen.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
# Debug: List files in publish output
RUN ls -l /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet iPDFGen.Server.dll install-deps
RUN chown -R app:app /app && chmod -R u+x /app
ENV ASPNETCORE_URLS=http://+:8080
USER app
ENTRYPOINT ["dotnet", "iPDFGen.Server.dll"]