#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src

ARG PROJECT_NAME="SolaceTK.Core"
COPY ["${PROJECT_NAME}.csproj", "/"]
COPY ["nuget.config", "/"]
COPY . .

RUN dotnet build "${PROJECT_NAME}.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolaceTK.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS "https://*:8080"

COPY --from=publish /app/publish .
RUN apt-get update
RUN apt-get install ffmpeg libsm6 libxext6 libx11-dev -y &&\
	apt-get install -f ./Packages/aseprite.deb

ENTRYPOINT ["dotnet", "SolaceTK.Core.dll"]
