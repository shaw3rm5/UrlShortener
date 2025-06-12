FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Копируем всю папку src со всеми проектами
COPY src/ ./src/

# Устанавливаем рабочую директорию для API проекта
WORKDIR /app/src/UrlShortener.API

# Восстанавливаем зависимости
RUN dotnet restore

# Публикуем
RUN dotnet publish -c Release -o /app/out

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY --from=build /app/out ./

ENTRYPOINT ["dotnet", "UrlShortener.API.dll"]
