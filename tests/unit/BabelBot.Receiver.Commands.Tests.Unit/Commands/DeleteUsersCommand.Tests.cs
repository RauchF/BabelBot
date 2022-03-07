using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Options;
using BabelBot.Shared.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace BabelBot.Receiver.Commands.Tests.Unit;

[TestClass]
public class DeleteUsersCommandTests
{
    private DeleteUsersCommand _command = null!;
    private IUsers _users = null!;
    private TelegramOptions _options = new TelegramOptions { };

    [TestInitialize]
    public void Initialize()
    {
        var logger = Substitute.For<ILogger<DeleteUsersCommand>>();
        _users = Substitute.For<IUsers>();
        var options = Substitute.For<IOptions<TelegramOptions>>();
        options.Value.Returns(_options);

        _command = new(logger, _users, options);
    }

    [TestMethod]
    public async Task Run_WithoutIds_ReturnsError()
    {
        // Act
        var message = new ReceivedMessage { };
        var result = await _command.Run(message, Array.Empty<string>(), new CancellationToken());

        // Assert
        Assert.IsTrue(result.HasError);
    }

    [TestMethod]
    public async Task Run_WithIds_RemovesAndReturnsSuccessMessage()
    {
        // Act
        var message = new ReceivedMessage { };
        var result = await _command.Run(message, new[] { "1", "2", "3" }, new CancellationToken());

        // Assert
        var expectedIds = new[] { 1L, 2L, 3L };
        _users.Received(1).DeleteTranslationUsers(Arg.Is<IEnumerable<long>>(ids => !ids.Except(expectedIds).Any()));
        Assert.IsTrue(result.Success);
    }

    [TestMethod]
    public async Task Run_WithSuperuserIds_RemovesAllUsersExceptTheSuperusers()
    {
        // Act
        var message = new ReceivedMessage { };
        _users.GetList(Arg.Any<Func<User, bool>>()).Returns(call =>
        {
            var predicate = call.Arg<Func<User, bool>>();

            return new[] {
                new User { Id = 1, Role = UserRole.Superuser },
                new User { Id = 2, Role = UserRole.TranslationUser },
                new User { Id = 3, Role = UserRole.Superuser },
                new User { Id = 4, Role = UserRole.TranslationUser },
            }.Where(predicate);
        });
        var result = await _command.Run(message, new[] { "1", "2", "3", "4" }, new CancellationToken());

        // Assert
        var expectedIds = new[] { 2L, 4L };
        _users.Received(1).DeleteTranslationUsers(Arg.Is<IEnumerable<long>>(ids => !ids.Except(expectedIds).Any()));
        Assert.IsTrue(result.Success);
    }
}
