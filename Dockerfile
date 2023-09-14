#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM image-registry.openshift-image-registry.svc:5000/bowman-dev/solacetk-ase-image AS base
WORKDIR /app

FROM registry.access.redhat.com/ubi8/dotnet-70 AS build
WORKDIR /src

ARG PROJECT_NAME="SolaceTK.Standalone"

#COPY ["${PROJECT_NAME}.csproj", "/"]
#COPY ["nuget.config", "/"]
COPY . .

RUN dotnet build "${PROJECT_NAME}/${PROJECT_NAME}.csproj" -c Release -o /src/build

FROM build AS publish
RUN dotnet publish ./SolaceTK.Standalone/SolaceTK.Standalone.csproj -c Release -o /src/publish

FROM base AS final
WORKDIR /app
EXPOSE 8080

ENV ASPNETCORE_URLS "https://*:8080"

COPY --from=publish /src/publish .
ENTRYPOINT ["dotnet", "SolaceTK.Standalone.dll"]