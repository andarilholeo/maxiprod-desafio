FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY ["src/MaxiProd.API/MaxiProd.API.csproj", "MaxiProd.API/"]
COPY ["src/MaxiProd.Application/MaxiProd.Application.csproj", "MaxiProd.Application/"]
COPY ["src/MaxiProd.Domain/MaxiProd.Domain.csproj", "MaxiProd.Domain/"]
COPY ["src/MaxiProd.Infrastructure/MaxiProd.Infrastructure.csproj", "MaxiProd.Infrastructure/"]

RUN dotnet restore "MaxiProd.API/MaxiProd.API.csproj"

COPY src/ .
WORKDIR "/src/MaxiProd.API"
RUN dotnet build "MaxiProd.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MaxiProd.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Development

ENTRYPOINT ["dotnet", "MaxiProd.API.dll"]

