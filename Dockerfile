FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

COPY ["BabelBot.Receiver.Telegram/BabelBot.Receiver.Telegram.csproj", "BabelBot.Receiver.Telegram/"]
RUN dotnet restore "BabelBot.Receiver.Telegram/BabelBot.Receiver.Telegram.csproj"

COPY ["BabelBot.Shared/BabelBot.Shared.csproj", "BabelBot.Shared/"]
RUN dotnet restore "BabelBot.Shared/BabelBot.Shared.csproj"

COPY ["BabelBot.Translator.DeepL/BabelBot.Translator.DeepL.csproj", "BabelBot.Translator.DeepL/"]
RUN dotnet restore "BabelBot.Translator.DeepL/BabelBot.Translator.DeepL.csproj"

COPY ["BabelBot.Worker/BabelBot.Worker.csproj", "BabelBot.Worker/"]
RUN dotnet restore "BabelBot.Worker/BabelBot.Worker.csproj"

COPY ["BabelBot.Receiver.Telegram/", "BabelBot.Receiver.Telegram/"]
COPY ["BabelBot.Shared/", "BabelBot.Shared/"]
COPY ["BabelBot.Translator.DeepL/", "BabelBot.Translator.DeepL/"]
COPY ["BabelBot.Worker/", "BabelBot.Worker/"]

WORKDIR "/src/BabelBot.Worker"
RUN dotnet build "BabelBot.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BabelBot.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BabelBot.Worker.dll"]
