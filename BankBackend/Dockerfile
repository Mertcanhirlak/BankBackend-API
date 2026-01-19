# 1. Build Stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Proje dosyasini kopyala ve bagimliliklari yukle
COPY ["BankBackend.csproj", "./"]
RUN dotnet restore

# Tum kodlari kopyala ve yayinla
COPY . .
RUN dotnet publish -c Release -o out

# 2. Runtime Stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Uygulamanin dinleyecegi port
EXPOSE 8080
ENTRYPOINT ["dotnet", "BankBackend.dll"]
