using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using BabelBot.Shared.Messenger;
using BabelBot.Shared.Storage;
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
        _users = Substitute.For<IUsers>();

        _command = new(_users);
    }

    [TestMethod]
    public async Task Run_WithoutIds_ReturnsError()
    {
        // Act
        var message = new ReceivedMessage { Text = "" };
        var result = await _command.Run(new CancellationToken(), message);

        // Assert
        Assert.IsTrue(result.HasError);
    }

    [TestMethod]
    public async Task Run_WithIds_SavesIdsAndReturnsSuccessMessage()
    {
        // Act
        var message = new ReceivedMessage { Text = "1 2 3" };
        var result = await _command.Run(new CancellationToken(), message);

        // Assert
        var expectedIds = new[] { 1L, 2L, 3L };
        _users.Received(1).AddUsers(Arg.Is<long[]>(ids => !ids.Except(expectedIds).Any()));
        Assert.IsTrue(result.Success);
    }
}
