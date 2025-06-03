FROM ubuntu:22.04 AS base
RUN apt-get update && apt-get install -y aspnetcore-runtime-8.0
RUN apt-get install -y dotnet-sdk-8.0
RUN apt-get install -y libnss3 libatk1.0-0 libgbm-dev chromium-browser libatk-bridge2.0-0 libcups2 libxkbcommon0 libatspi2.0-0 libxcomposite1 libxdamage1 libxfixes3 libxrandr2 libpango-1.0-0 libcairo2 libasound2
RUN useradd -m -u 1000 -s /bin/bash app && mkdir -p /app && chown -R app:app /app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

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
RUN dotnet publish "./iPDFGen.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish --no-restore /p:UseAppHost=false
# Debug: List files in publish output
RUN ls -l /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
RUN dotnet /app/publish/iPDFGen.Server.dll
RUN chown -R app:app /app/.playwright && chmod -R u+x /app/.playwright
ENV ASPNETCORE_URLS=http://+:8080
USER app
ENTRYPOINT ["dotnet", "iPDFGen.Server.dll"]