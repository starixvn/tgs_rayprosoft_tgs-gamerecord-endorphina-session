#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BuildConfiguration=Release
WORKDIR /src
COPY ["nuget.config", "."]
COPY ["Tgs.GameRecord.Endorphina.Session/Tgs.GameRecord.Endorphina.Session.csproj", "Tgs.GameRecord.Endorphina.Session/"]
COPY ["Tgs.GameRecord.Endorphina.Session.Core/Tgs.GameRecord.Endorphina.Session.Core.csproj", "Tgs.GameRecord.Endorphina.Session.Core/"]
COPY ["Tgs.GameRecord.Endorphina.Session.Repositories/Tgs.GameRecord.Endorphina.Session.Repositories.csproj", "Tgs.GameRecord.Endorphina.Session.Repositories/"]
RUN dotnet restore "Tgs.GameRecord.Endorphina.Session/Tgs.GameRecord.Endorphina.Session.csproj"
COPY . .
WORKDIR "/src/Tgs.GameRecord.Endorphina.Session"
RUN dotnet build "Tgs.GameRecord.Endorphina.Session.csproj" -c $BuildConfiguration -o /app/build

FROM build AS publish
ARG BuildConfiguration
RUN dotnet publish "Tgs.GameRecord.Endorphina.Session.csproj" -c $BuildConfiguration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tgs.GameRecord.Endorphina.Session.dll"]