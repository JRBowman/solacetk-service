#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0-bullseye-slim AS build
WORKDIR /src

ARG PROJECT_NAME="SolaceTK.Core"
#ARG PROJECT_PAT="3o4ehyg34gmgylolh55a43zhlmc3ltw4wqfkt4lrl4rq6ynw2rwq"
#ENV VSS_NUGET_EXTERNAL_FEED_ENDPOINTS \
#	"{\"endpointCredentials\": [{\"endpoint\":\"https://pkgs.dev.azure.com/bowman-redhat/_packaging/bowman-redhat/nuget/v3/index.json\", \"password\":\"${PROJECT_PAT}\"}]}"

#RUN sh -c "$(curl -fsSL https://aka.ms/install-artifacts-credprovider.sh)"

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
#COPY ["./Packages/usr/bin/aseprite", "/usr/bin/aseprite"]
#COPY ["./Packages/usr/share/aseprite/", "/usr/share/aseprite/"]
#RUN chmod o+x /usr/bin/aseprite
#RUN #chmod 777 /usr/bin/aseprite &&\
	#chmod 777 ./Packages/usr/bin/aseprite &&\
	#chmod +x /usr/bin/aseprite &&\
	#chmod +x ./Packages/usr/bin/aseprite
ENTRYPOINT ["dotnet", "SolaceTK.Core.dll"]