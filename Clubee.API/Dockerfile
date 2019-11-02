FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Clubee.API/Clubee.API.csproj", "Clubee.API/"]
RUN dotnet restore "Clubee.API/Clubee.API.csproj"
COPY . .
WORKDIR "/src/Clubee.API"
RUN dotnet build "Clubee.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Clubee.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Clubee.API.dll"]
