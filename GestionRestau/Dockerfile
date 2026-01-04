FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["GestionRestau/GestionRestau.csproj", "GestionRestau/"]
RUN dotnet restore "GestionRestau/GestionRestau.csproj"
COPY . .
WORKDIR "/src/GestionRestau"
RUN dotnet build "GestionRestau.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GestionRestau.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:5000
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV LC_ALL=fr_FR.UTF-8
ENV LANG=fr_FR.UTF-8
ENTRYPOINT ["dotnet", "GestionRestau.dll"]
