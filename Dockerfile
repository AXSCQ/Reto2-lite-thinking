FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY nuget-packages/ /root/.nuget/packages/

COPY ["Reto2_Architecture.Api/Reto2_Architecture.Api.csproj", "Reto2_Architecture.Api/"]
COPY ["Reto2_Architecture.Application/Reto2_Architecture.Application.csproj", "Reto2_Architecture.Application/"]
COPY ["Reto2_Architecture.Domain/Reto2_Architecture.Domain.csproj", "Reto2_Architecture.Domain/"]
COPY ["Reto2_Architecture.Infrastructure/Reto2_Architecture.Infrastructure.csproj", "Reto2_Architecture.Infrastructure/"]

RUN dotnet restore "Reto2_Architecture.Api/Reto2_Architecture.Api.csproj" --source /root/.nuget/packages
COPY . .

WORKDIR /src/Reto2_Architecture.Api
RUN dotnet publish "Reto2_Architecture.Api.csproj" -c Release -o /app/publish --source /root/.nuget/packages

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "Reto2_Architecture.Api.dll"]
