# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Lexilearn.WebApi/Lexilearn.WebApi.csproj", "Lexilearn.WebApi/"]
COPY ["Lexilearn.DataTransfer/Lexilearn.DataTransfer.csproj", "Lexilearn.DataTransfer/"]
COPY ["Lexilearn.LibreTranslate/Lexilearn.LibreTranslate.csproj", "Lexilearn.LibreTranslate/"]
COPY ["Lexilearn.Application/Lexilearn.Application.csproj", "Lexilearn.Application/"]
COPY ["Lexilearn.Identity/Lexilearn.Identity.csproj", "Lexilearn.Identity/"]
COPY ["Lexilearn.MySql/Lexilearn.MySql.csproj", "Lexilearn.MySql/"]
COPY ["Lexilearn.Shared/Lexilearn.Shared.csproj", "Lexilearn.Shared/"]
COPY ["Lexilearn.Domain/Lexilearn.Domain.csproj", "Lexilearn.Domain/"]
RUN dotnet restore "./Lexilearn.WebApi/Lexilearn.WebApi.csproj"
COPY . .
WORKDIR "/src/Lexilearn.WebApi"
RUN dotnet build "./Lexilearn.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Lexilearn.WebApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lexilearn.WebApi.dll"]