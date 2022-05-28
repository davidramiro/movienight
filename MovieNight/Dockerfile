FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["movienight/movienight.csproj", "movienight/"]
RUN dotnet restore "movienight/movienight.csproj"
COPY . .
WORKDIR "/src/movienight"
RUN dotnet build "movienight.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "movienight.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "movienight.dll"]
