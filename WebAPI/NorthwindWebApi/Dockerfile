#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["WebAPI/NorthwindWebApi/NorthwindWebApi.csproj", "WebAPI/NorthwindWebApi/"]
COPY ["Infrastructure/Northwind.Repository/Northwind.Repository.csproj", "Infrastructure/Northwind.Repository/"]
COPY ["Domain/Northwind.Contracts/Northwind.Contracts.csproj", "Domain/Northwind.Contracts/"]
COPY ["Domain/Northwind.Entities/Northwind.Entities.csproj", "Domain/Northwind.Entities/"]
COPY ["Domain/Northwind.LoggerService/Northwind.LoggerService.csproj", "Domain/Northwind.LoggerService/"]
RUN dotnet restore "WebAPI/NorthwindWebApi/NorthwindWebApi.csproj"
COPY . .
WORKDIR "/src/WebAPI/NorthwindWebApi"
RUN dotnet build "NorthwindWebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NorthwindWebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NorthwindWebApi.dll"]