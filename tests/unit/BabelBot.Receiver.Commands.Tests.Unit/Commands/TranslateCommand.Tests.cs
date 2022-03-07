using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Translation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

[TestClass]
public class TranslateCommandTests
{
    private TranslateCommand _command = null!;
    private ITranslator _translator = null!;
    private IMessenger _messenger = null!;

    [TestInitialize]
    public void Initialize()
    {
        _translator = Substitute.For<ITranslator>();

        _messenger = Substitute.For<IMessenger>();

        _command = new(_translator, _messenger);
    }

    [TestMethod]
    public async Task Run_RepliesToMessageWithTranslation()
    {
        // Arrange
        var message = new ReceivedMessage { Text = "some text", ChatId = 42, Id = 42 };
        var translation = new TranslationResult { Text = "translation" };
        _translator.TranslateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(translation));

        // Act
        await _command.Run(new CancellationToken(), message);

        // Assert
        await _messenger.Received(1).SendTextMessage(message.ChatId, translation.Text, message.Id);
    }

    [TestMethod]
    public async Task Run_WithLongTranslation_SendsMultipleMessages()
    {
        // Arrange
        var message = new ReceivedMessage { Text = "some text", ChatId = 42, Id = 42 };
        var translation = new TranslationResult { Text = "translation".PadRight(5000, 'A') };
        _translator.TranslateAsync(Arg.Any<string>(), Arg.Any<CancellationToken>()).Returns(Task.FromResult(translation));

        // Act
        await _command.Run(new CancellationToken(), message);

        // Assert
        await _messenger.Received(1).SendTextMessage(message.ChatId, Arg.Is<string>(s => s.Length > 4000), message.Id);
        await _messenger.Received(1).SendTextMessage(message.ChatId, Arg.Is<string>(s => s.Length < 4000), message.Id);
    }
}
