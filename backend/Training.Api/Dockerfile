#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8000
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Training.Api/Training.Api.csproj", "Training.Api/"]
COPY ["Training.BusinessLogic/Training.BusinessLogic.csproj", "Training.BusinessLogic/"]
COPY ["Training.Repository/Training.Repository.csproj", "Training.Repository/"]
COPY ["Training.DataAccess/Training.DataAccess.csproj", "Training.DataAccess/"]
COPY ["Training.Common/Training.Common.csproj", "Training.Common/"]
COPY ["Training.CustomException/Training.CustomException.csproj", "Training.CustomException/"]
RUN dotnet restore "./Training.Api/Training.Api.csproj"
COPY . .
WORKDIR "/src/Training.Api"
RUN dotnet build "./Training.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Training.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Training.Api.dll"]