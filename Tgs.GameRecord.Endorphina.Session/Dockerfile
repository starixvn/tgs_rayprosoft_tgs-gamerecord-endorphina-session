#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tgs.GameRecord.Endorphina.Session/Tgs.GameRecord.Endorphina.Session.csproj", "Tgs.GameRecord.Endorphina.Session/"]
COPY ["Tgs.GameRecord.Endorphina.Session.Core/Tgs.GameRecord.Endorphina.Session.Core.csproj", "Tgs.GameRecord.Endorphina.Session.Core/"]
COPY ["Tgs.GameRecord.Endorphina.Session.Repositories/Tgs.GameRecord.Endorphina.Session.Repositories.csproj", "Tgs.GameRecord.Endorphina.Session.Repositories/"]
RUN dotnet restore "./Tgs.GameRecord.Endorphina.Session/Tgs.GameRecord.Endorphina.Session.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.Endorphina.Session"
RUN dotnet build "./Tgs.GameRecord.Endorphina.Session.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tgs.GameRecord.Endorphina.Session.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.Endorphina.Session.dll"]