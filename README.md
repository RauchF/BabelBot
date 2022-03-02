# BabelBot

A simple chat bot connecting Instant Messaging and Translation APIs

[Guide to Getting Started](https://felixrau.ch/babelbot/)

## Installation

You can either use the precompiled packages or compile BabelBot yourself.

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

- .NET Core 6.0 SDK


## Configuration

BabelBot uses the [configuration facilities](https://docs.microsoft.com/en-us/dotnet/core/extensions/configuration) provided by .NET Core.

This means configuration can be applied in different ways:

- Changing the values inside `appsettings.json`
- Setting environment variables (useful for cloud deployments)
- Probably others I dunno

### Available configuration parameters

The required configuration values are documented in the table below.

| Key (`appsettings.json`)        | Key (Environment) | Type | Description | Default | Required                 |
|---------------------------------|-------------------|------|-------------|---------|--------------------------|
| DeepL:AuthKey                   | DeepL__AuthKey | string | Your DeepL API auth key | _empty_ | true if DeepL is used    |
| DeepL:DefaultTargetLanguageCode | DeepL__DefaultTargetLanguageCode | string | The target language for DeepL translations | `en-GB` | false |
| Telegram:ApiKey                 | Telegram__ApiKey | string | Your Telegram bot's API key | _empty_ | true if Telegram is used |
| Telegram:AllowedUsers           | Telegram__AllowedUsers | array of long | List of Telegram user IDs allowed to use the bot | _empty_ | true if Telegram is used |
| Worker:Receivers                | Worker__Receivers | array of string | List active receivers (instant messengers) | `["Telegram"]` | true                     |
| Worker:Translator               | Worker__Translator | string | Translation API to be used | `"DeepL"` | true                     |

### Notes regarding configuration via Environment variables

Some of the configuration parameters are arrays. You can set those with environment variables using the following syntax:

**Example**

```
Telegram__AllowedUsers__1=4711
Telegram__AllowedUsers__2=420420
```

## See also

- [DeepL API Signup](https://www.deepl.com/pro-api) - Sign up for DeepL API access here
- [DeepL API Docs](https://www.deepl.com/docs-api/translating-text/request/) - Here you can find a list of target language codes supported by DeepL
- [Telegram Botfather](https://t.me/botfather) - Register your Telegram bot here

## Contributing

Contributions are always welcome!

See [CONTRIBUTING.md](CONTRIBUTING.md) for ways to get started.

Please adhere to this project's [code of conduct](CODE_OF_CONDUCT.md).


## Support

This software is provided without warranty or implicit or explicit support.

If you believe you have found an issue (very likely), please issue a bug report as detailed in the [contribution guidelines](CONTRIBUTING.md).

In very serious cases, you can send a mail to babelbot@kaputty.de and I'll try to get back to you.

Please note that I do not provide support for misconfigurations or missing installed dependencies.


## License

[MIT](https://choosealicense.com/licenses/mit/)
