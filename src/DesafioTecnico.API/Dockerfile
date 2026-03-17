FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["src/DesafioTecnico.API/DesafioTecnico.API.csproj", "DesafioTecnico.API/"]
COPY ["src/DesafioTecnico.Application/DesafioTecnico.Application.csproj", "DesafioTecnico.Application/"]
COPY ["src/DesafioTecnico.Domain/DesafioTecnico.Domain.csproj", "DesafioTecnico.Domain/"]
COPY ["src/DesafioTecnico.Infrastructure/DesafioTecnico.Infrastructure.csproj", "DesafioTecnico.Infrastructure/"]

RUN dotnet restore "DesafioTecnico.API/DesafioTecnico.API.csproj"

COPY src/ .
WORKDIR "/src/DesafioTecnico.API"
RUN dotnet build "DesafioTecnico.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DesafioTecnico.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "DesafioTecnico.API.dll"]

