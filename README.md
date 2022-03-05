# BabelBot

A simple chat bot connecting Instant Messaging and Translation APIs

[Guide to Getting Started](https://felixrau.ch/babelbot/)

## Installation

You can either use the precompiled packages, compile BabelBot yourself or use the provided Dockerfile.\
You can also deploy BabelBot on a Kubernetes cluster using Helm.

### Precompiled packages

1. Grab the latest [precompiled release](https://github.com/RauchF/BabelBot/releases/latest) for your platform
2. [Configure](#Configuration)
3. Run `BabelBot.Worker` / `BabelBot.Worker.exe`

### Installation from source

1. Make sure you have all [prerequisites](#Prerequisites) installed.
2. Download and extract an archive of your desired version (or development branch), or `git clone` the repository into a path of your choice.
3. Run `dotnet publish -c Release` to build a release version for your currently used .NET runtime. See the .NET Core documentation for more details on how to use the `dotnet` CLI utility.
4. [Configure](#Configuration)
5. Run

#### Prerequisites

A (possibly incomplete, please report if you had to install more) list of prerequisites:

- [.NET Core](https://docs.microsoft.com/en-us/dotnet/core/install) 6.0 SDK

### Build Docker image

1. Make sure you have all [prerequisites](#Prerequisites) installed.
2. Download and extract an archive of your desired version (or development branch), or `git clone` the repository into a path of your choice.
3. Build an image: `docker build -t babelbot:local .`
   1. You can freely choose the image name and tag.
4. Run BabelBot in Docker: `docker run -e Telegram__ApiKey='<api key>' -e Telegram__AllowedUsers__1='<id>' -e DeepL__AuthKey='<auth key>' babelbot:local`

#### Prerequisites

- [Docker](https://docs.docker.com/get-docker/) v20+

### Deploy on Kubernetes using Helm

1. Make sure you have all [prerequisites](#Prerequisites) installed.
2. Download and extract an archive of your desired version (or development branch), or `git clone` the repository into a path of your choice.
3. Create a `babelbot-values.yaml` ([details about which values to provide](#./helm/README.md))
4. Run the following command: `helm upgrade --install -n babelbot --create-namespace babelbot ./helm -f ./babelbot-values.yaml`

#### Prerequisites

- [Helm](https://helm.sh/docs/intro/install/) v3


## Configuration

BabelBot uses the [configuration facilities](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) provided by .NET Core.

This means configuration can be applied in different ways:

- Changing the values inside `appsettings.json`
- Setting environment variables (useful for cloud deployments)
- Probably others I dunno

### Available configuration parameters

The required configuration values are documented in the table below.

| Key (`appsettings.json`)         | Key (Environment)                 | Type            | Description                                                                                                                                 | Default        | Required                 |
| -------------------------------- | --------------------------------- | --------------- | ------------------------------------------------------------------------------------------------------------------------------------------- | -------------- | ------------------------ |
| DeepL:AuthKey                    | DeepL__AuthKey                    | string          | Your DeepL API auth key                                                                                                                     | _empty_        | true if DeepL is used    |
| DeepL:DefaultTargetLanguageCode  | DeepL__DefaultTargetLanguageCode  | string          | The target language for DeepL translations (see [here](https://www.deepl.com/en/docs-api/translating-text/) for a list of supported values) | `en-GB`        | false                    |
| Telegram:ApiKey                  | Telegram__ApiKey                  | string          | Your Telegram bot's API key                                                                                                                 | _empty_        | true if Telegram is used |
| Telegram:AllowedUsers            | Telegram__AllowedUsers            | array of long   | List of Telegram user IDs allowed to use the bot                                                                                            | _empty_        | true if Telegram is used |
| Telegram:OnlyReactToAllowedUsers | Telegram__OnlyReactToAllowedUsers | boolean         | When `true`, the bot ignores all unknown users                                                                                              | `true`         | false                    |
| Worker:Receivers                 | Worker__Receivers                 | array of string | List active receivers (instant messengers)                                                                                                  | `["Telegram"]` | true                     |
| Worker:Translator                | Worker__Translator                | string          | Translation API to be used                                                                                                                  | `"DeepL"`      | true                     |

### Notes regarding configuration via Environment variables

Some of the configuration parameters are arrays. You can set those with environment variables using the following syntax:

**Example**

```
Telegram__AllowedUsers__1=4711
Telegram__AllowedUsers__2=420420
```

### Using user secrets during development

As long as the program is running in the Development environment, you can use the [Secret Manager](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-6.0&tabs=windows#secret-manager) built into dotnet.

Set your API tokens (and other options) with the `dotnet` command, like this:

```
dotnet user-secrets set "DeepL:AuthKey" "1234-5678"
```

They will persist through checkouts and are applied *after* `appsettings.json` and environment variables.

## See also

- [DeepL API Signup](https://www.deepl.com/pro-api) - Sign up for DeepL API access here
- [DeepL API Docs](https://www.deepl.com/docs-api/translating-text/request/) - Here you can find a list of target language codes supported by DeepL
- [Telegram Botfather](https://t.me/botfather) - Register your Telegram bot here

## Contributing

Contributions are always welcome!

See [CONTRIBUTING.md](CONTRIBUTING.md) for ways to get started.

Please adhere to this project's [code of conduct](CODE_OF_CONDUCT.md).

## Acknowledgements

Massive thanks to (amongst many more) the following projects for making this possible:

- [Telegram.Bot](https://github.com/TelegramBots/telegram.bot) for providing a really nice MIT-licensed API library for .NET
- [DeepL](https://deepl.com) for creating a fantastic translation service and providing a [.NET library](https://www.nuget.org/packages/DeepL.net/) for it

## Support

This software is provided without warranty or implicit or explicit support.

If you believe you have found an issue (very likely), please issue a bug report as detailed in the [contribution guidelines](CONTRIBUTING.md).

In very serious cases, you can send a mail to babelbot@kaputty.de and I'll try to get back to you.

Please note that I do not provide support for misconfigurations or missing installed dependencies.

## License

[MIT](https://choosealicense.com/licenses/mit/)
