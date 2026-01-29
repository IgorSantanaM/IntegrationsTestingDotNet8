# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["RentalMotorcycle.Presentation.WebAPI/RentalMotorcycle.Presentation.WebAPI.csproj", "RentalMotorcycle.Presentation.WebAPI/"]
COPY ["RentalMotorcycle.Application/RentalMotorcycle.Application.csproj", "RentalMotorcycle.Application/"]
COPY ["RentalMotorcycle.Domain/RentalMotorcycle.Domain.csproj", "RentalMotorcycle.Domain/"]
COPY ["RentalMotorcycle.Domain.Core/RentalMotorcycle.Domain.Core.csproj", "RentalMotorcycle.Domain.Core/"]
COPY ["RentalMotorcycle.Infra.CrossCutting.IoC/RentalMotorcycle.Infra.CrossCutting.IoC.csproj", "RentalMotorcycle.Infra.CrossCutting.IoC/"]
COPY ["RentalMotorcycle.Infra.Data/RentalMotorcycle.Infra.Data.csproj", "RentalMotorcycle.Infra.Data/"]
COPY ["RentalMotorcycle.Infra.Messaging/RentalMotorcycle.Infra.Messaging.csproj", "RentalMotorcycle.Infra.Messaging/"]
RUN dotnet restore "./RentalMotorcycle.Presentation.WebAPI/RentalMotorcycle.Presentation.WebAPI.csproj"
COPY . .
WORKDIR "/src/RentalMotorcycle.Presentation.WebAPI"
RUN dotnet build "./RentalMotorcycle.Presentation.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RentalMotorcycle.Presentation.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RentalMotorcycle.Presentation.WebAPI.dll"]