#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#=====================================================================================================================================
# Build 

ARG VERSION=8.0-jammy-amd64
FROM mcr.microsoft.com/dotnet/sdk:$VERSION AS build
WORKDIR /src

ARG LINUXARQ=linux-musl-x64
ARG PROJECT_NAME="Rent.Service.Api"

# Copiar todo o código fonte da aplicação
COPY Directory.Build.props /
COPY Nuget.config /config/Nuget.config
COPY /src/  /src/
COPY "/src/${PROJECT_NAME}" "/src/${PROJECT_NAME}"
WORKDIR "/src/${PROJECT_NAME}"
RUN dotnet restore "${PROJECT_NAME}.csproj" -r ${LINUXARQ} --configfile "/config/Nuget.config"

#=====================================================================================================================================
# Publish

FROM build AS publish
RUN dotnet publish "${PROJECT_NAME}.csproj" -c Release -r ${LINUXARQ} -o /app/publish --no-restore --self-contained false

#=====================================================================================================================================
# Image base

FROM mcr.microsoft.com/dotnet/aspnet:$VERSION AS base
ADD https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb /

# Definir ambiente e instalar dependências
ARG DOCKER_TAG
ENV APP_VERSION=$DOCKER_TAG
RUN dpkg -i packages-microsoft-prod.deb \
    && apt-get update \
    && apt-get upgrade -y \
    && apt-get install -y libmsquic=1.9*

WORKDIR /app

#=====================================================================================================================================
# Build runtime image

FROM base AS final

# Definição do usuário que será usado ao executar o contêiner
# From Ubuntu --------
RUN addgroup --group gnet --gid 2000 \
    && adduser --uid 1000 --gid 2000 unet\
    && chown unet:gnet /app

USER unet:gnet

# Copiar os arquivos do estágio "publish" para o diretório /app no contêiner
COPY --chown=unet:gnet --from=publish /app/publish .
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Rent.Service.Api.dll"]