using System.Collections.Generic;
using BabelBot.Shared.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

[TestClass]
public class CommandFactoryTests
{
    private CommandFactory _factory = null!;
    private List<ICommand> _commands = null!;
    private ICommand _defaultCommand = null!;

    [TestInitialize]
    public void Initialize()
    {
        var command = Substitute.For<ICommand>();
        command.Keyword.Returns("cmd");
        _defaultCommand = Substitute.For<ICommand>();
        _defaultCommand.IsDefault.Returns(true);

        _commands = new() { command, _defaultCommand };

        _factory = new(_commands);
    }

    [TestMethod]
    public void GetCommand_WithMessageWithoutCommand_ShouldReturnDefaultCommand()
    {
        // Arrange
        var message = "no command here";

        // Act
        var command = _factory.GetCommand(message);

        // Assert
        Assert.AreEqual(_defaultCommand, command);
    }

    [TestMethod]
    public void GetCommand_WithMessageWithoutCommandAndMissingDefaultCommand_ShouldThrowException()
    {
        // Arrange
        var message = "no command here";
        _commands.Remove(_defaultCommand);

        // Act
        var test = () => _factory.GetCommand(message);

        // Assert
        Assert.ThrowsException<NoDefaultCommandException>(test);
    }

    [TestMethod]
    public void GetCommand_WithMessageWithUnknownCommand_ShouldReturnMissingCommand()
    {
        // Arrange
        var message = "/unknown";

        // Act
        var command = _factory.GetCommand(message);

        // Assert
        Assert.IsInstanceOfType(command, typeof(MissingCommand));
        Assert.AreEqual("unknown", command.Keyword);
    }

    [TestMethod]
    public void GetCommand_WithMessageWithKnownCommand_ShouldReturnThatCommand()
    {
        // Arrange
        var message = "/cmd";

        // Act
        var command = _factory.GetCommand(message);

        // Assert
        Assert.IsNotNull(command);
    }
}
