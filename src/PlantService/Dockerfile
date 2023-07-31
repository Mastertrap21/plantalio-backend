FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PlantService/PlantService.csproj", "PlantService/"]
RUN dotnet restore "PlantService/PlantService.csproj"
COPY . .
WORKDIR "/src/PlantService"
RUN dotnet build "PlantService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PlantService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PlantService.dll"]
