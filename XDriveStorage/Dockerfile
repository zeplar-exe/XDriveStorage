FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["XDriveStorage/XDriveStorage.csproj", "XDriveStorage/"]
RUN dotnet restore "XDriveStorage/XDriveStorage.csproj"
COPY . .
WORKDIR "/src/XDriveStorage"
RUN dotnet build "XDriveStorage.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "XDriveStorage.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "XDriveStorage.dll"]
