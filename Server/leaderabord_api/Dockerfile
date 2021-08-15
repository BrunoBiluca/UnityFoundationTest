FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["leaderabord_api.csproj", "."]
RUN dotnet restore "./leaderabord_api.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "leaderabord_api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "leaderabord_api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "leaderabord_api.dll"]

# CMD ASPNETCORE_URLS=http://*:$PORT dotnet leaderabord_api.dll