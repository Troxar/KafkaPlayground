# ============================
# 1. Build stage
# ============================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY ["src/OrderApi/OrderApi.WebApi/OrderApi.WebApi.csproj", "src/OrderApi/OrderApi.WebApi/"]
COPY ["src/OrderApi/OrderApi.Application/OrderApi.Application.csproj", "src/OrderApi/OrderApi.Application/"]
COPY ["src/OrderApi/OrderApi.Domain/OrderApi.Domain.csproj", "src/OrderApi/OrderApi.Domain/"]
COPY ["src/OrderApi/OrderApi.Persistence/OrderApi.Persistence.csproj", "src/OrderApi/OrderApi.Persistence/"]
COPY ["src/OrderApi/OrderApi.Messaging/OrderApi.Messaging.csproj", "src/OrderApi/OrderApi.Messaging/"]

RUN dotnet restore "src/OrderApi/OrderApi.WebApi/OrderApi.WebApi.csproj"

COPY . .

RUN dotnet publish "src/OrderApi/OrderApi.WebApi/OrderApi.WebApi.csproj" -c Release -o /app/publish

# ============================
# 2. Runtime stage
# ============================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

CMD ["dotnet", "OrderApi.WebApi.dll"]