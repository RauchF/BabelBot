using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

[TestClass]
public class AddUsersCommandTests
{
    private AddUsersCommand _command = null!;
    private IUsers _users = null!;

    [TestInitialize]
    public void Initialize()
    {
        var logger = Substitute.For<ILogger<AddUsersCommand>>();
        _users = Substitute.For<IUsers>();

        _users.GetUser(1).Returns(new User { Id = 1, Role = UserRole.Superuser });

        _command = new(logger, _users);
    }

    [TestMethod]
    public async Task Run_WithoutIds_ReturnsError()
    {
        // Act
        var message = new ReceivedMessage { UserId = 1 };
        var result = await _command.Run(message, Array.Empty<string>(), new CancellationToken());

        // Assert
        Assert.IsTrue(result.HasError);
    }

    [TestMethod]
    public async Task Run_WithIds_SavesIdsAndReturnsSuccessMessage()
    {
        // Act
        var message = new ReceivedMessage { UserId = 1 };
        var result = await _command.Run(message, new[] { "1", "2", "3" }, new CancellationToken());

        // Assert
        var expectedIds = new[] { 1L, 2L, 3L };
        _users.Received(1).AddTranslationUsers(Arg.Is<IEnumerable<long>>(ids => !ids.Except(expectedIds).Any()));
        Assert.IsTrue(result.Success);
    }
}
