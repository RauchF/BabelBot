FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BabelBot.Worker/BabelBot.Worker.csproj", "BabelBot.Worker/"]
RUN dotnet restore "BabelBot.Worker/BabelBot.Worker.csproj"
COPY . .
WORKDIR "/src/BabelBot.Worker"
RUN dotnet build "BabelBot.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BabelBot.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BabelBot.Worker.dll"]
