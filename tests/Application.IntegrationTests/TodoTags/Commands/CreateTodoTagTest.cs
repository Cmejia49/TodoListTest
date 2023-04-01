using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;
using Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.TodoTags.Commands;
using static Testing;

public class CreateTodoTagTest : BaseTestFixture
{

    [Test]
    public async Task ShouldRequireMinimumFields()
    {
        var command = new CreateTodoTagsCommand();
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }


    [Test]
    public async Task ShouldRequireUniqueName()
    {
        await SendAsync(new CreateTodoTagsCommand
        {
            Name = "Coocking"
        });

        var command = new CreateTodoTagsCommand
        {
            Name = "Coocking"
        };

        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }

    [Test]
    public async Task ShouldCreateTodoTag()
    {
        var userId = await RunAsDefaultUserAsync();

        var command = new CreateTodoTagsCommand
        {
            Name = "Coocking"
        };

        var id = await SendAsync(command);

        var tags = await FindAsync<TodoTag>(id);

        tags.Should().NotBeNull();
        tags!.Name.Should().Be(command.Name);
        tags.CreatedBy.Should().Be(userId);
        tags.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
