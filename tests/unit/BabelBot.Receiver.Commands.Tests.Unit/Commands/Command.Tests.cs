using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Commands;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

public class TestCommand : Command
{
    public IEnumerable<UserRole> AllowedRolesForTest;
    public List<(ReceivedMessage, IEnumerable<string>)> Calls = new();

    public TestCommand(
        ILogger<Command> logger,
        IUsers users)
        : base(logger, users)
    {
        AllowedRolesForTest = new[] { UserRole.Superuser };
    }

    public override string Keyword => "test";

    public override IEnumerable<UserRole> AllowedRoles => AllowedRolesForTest;

    public override Task<Shared.Commands.CommandResult> Run(ReceivedMessage message, IEnumerable<string> commandArguments, CancellationToken cancellationToken)
    {
        Calls.Add((message, commandArguments));

        return Task.FromResult(new CommandResult());
    }
}

[TestClass]
public class CommandTests
{
    private TestCommand _command = null!;
    private IUsers _users = null!;

    [TestInitialize]
    public void Initialize()
    {
        _users = Substitute.For<IUsers>();
        _users.GetUser(1).Returns(new User { Id = 1, Role = UserRole.Superuser });

        _command = new TestCommand(Substitute.For<ILogger<Command>>(), _users);
    }

    [TestMethod]
    public async Task Run_WithoutUserId_DoesNothing()
    {
        // Arrange
        var message = new ReceivedMessage { UserId = null };

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        Assert.IsFalse(_command.Calls.Any());
    }

    [TestMethod]
    public async Task Run_WithInvalidUserRole_DoesNothing()
    {
        // Arrange
        _users.GetUser(2).Returns(new User { Id = 2, Role = UserRole.TranslationUser });
        var message = new ReceivedMessage { UserId = 2 };

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        Assert.IsFalse(_command.Calls.Any());
    }
    public static IEnumerable<object[]> Run_WithUnknownUser_DefaultsToAnonymousUser_TestData()
    {
        yield return new object[] { new[] { UserRole.Superuser, UserRole.TranslationUser }, false };
        yield return new object[] { new[] { UserRole.Anonymous }, true };
    }
    [TestMethod]
    [DynamicData(nameof(Run_WithUnknownUser_DefaultsToAnonymousUser_TestData), DynamicDataSourceType.Method)]
    public async Task Run_WithUnknownUser_DefaultsToAnonymousUser(IEnumerable<UserRole> allowedRoles, bool expected)
    {
        // Arrange
        _command.AllowedRolesForTest = allowedRoles;
        _users.GetUser(2).Returns(null as User);
        var message = new ReceivedMessage { UserId = 2, Text = "" };

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        Assert.AreEqual(expected, _command.Calls.Any());
    }

    [TestMethod]
    public async Task Run_StripsCommandFromArgumentListAndProvidesMessage()
    {
        // Arrange
        var message = new ReceivedMessage { UserId = 1, Text = "/test 1 2 abc" };

        // Act
        await _command.Run(message, new CancellationToken());

        // Assert
        Assert.IsTrue(_command.Calls.Any());

        var call = _command.Calls.First();
        var receivedMessage = call.Item1;
        var receivedArguments = call.Item2;
        Assert.AreEqual(message, receivedMessage);
        CollectionAssert.AreEqual(new[] { "1", "2", "abc" }, receivedArguments.ToArray());
    }
}
