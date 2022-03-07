using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using BabelBot.Shared.Translation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

[TestClass]
public class TranslateCommandTests
{
    private TranslateCommand _command = null!;
    private IUsers _users = null!;
    private ITranslator _translator = null!;
    private IMessenger _messenger = null!;
    private TelegramOptions _options = new TelegramOptions { };

    [TestInitialize]
    public void Initialize()
    {
        var logger = Substitute.For<ILogger<TranslateCommand>>();
        _users = Substitute.For<IUsers>();
        _translator = Substitute.For<ITranslator>();
        _messenger = Substitute.For<IMessenger>();
        var options = Substitute.For<IOptions<TelegramOptions>>();
        options.Value.Returns(_options);

        _users.GetUser(Arg.Any<long>()).Returns(new User { Id = 1, Role = UserRole.TranslationUser });

        _command = new(logger, _users, _translator, _messenger, options);
    }

    [TestMethod]
    public async Task Run_RepliesToMessageWithTranslation()
    {
        // Arrange
        var message = new ReceivedMessage { Id = 42, Text = "some text", ChatId = 42, UserId = 1 };
        var translation = new TranslationResult { Text = "translation" };
        _translator.TranslateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(translation));

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        await _messenger.Received(1).SendTextMessage(message.ChatId, translation.Text, message.Id);
    }

    [TestMethod]
    public async Task Run_WithLongTranslation_SendsMultipleMessages()
    {
        // Arrange
        var message = new ReceivedMessage { Id = 42, Text = "some text", ChatId = 42, UserId = 1 };
        var translation = new TranslationResult { Text = "translation".PadRight(5000, 'A') };
        _translator.TranslateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(translation));

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        await _messenger.Received(1).SendTextMessage(message.ChatId, Arg.Is<string>(s => s.Length > 4000), message.Id);
        await _messenger.Received(1).SendTextMessage(message.ChatId, Arg.Is<string>(s => s.Length < 4000), message.Id);
    }
}
