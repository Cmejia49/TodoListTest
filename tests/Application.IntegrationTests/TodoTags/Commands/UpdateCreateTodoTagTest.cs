

using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
using Todo_App.Application.TodoTags.Command.UpdateTodoTagsCommand;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.TodoTags.Commands;
using static Testing;

public class UpdateCreateTodoTagTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoTagtId()
    {
        var command = new UpdateTodoTagsCommand { Id = 99, Name = "New Tag" };
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldRequireUniqueName()
    {
        var tagId = await SendAsync(new CreateTodoTagsCommand
        {
            Name = "New List"
        });

        await SendAsync(new CreateTodoTagsCommand
        {
            Name = "Other List"
        });

        var command = new UpdateTodoTagsCommand
        {
            Id = tagId,
            Name = "Other List"
        };

        (await FluentActions.Invoking(() =>
            SendAsync(command))
                .Should().ThrowAsync<ValidationException>().Where(ex => ex.Errors.ContainsKey("Name")))
                .And.Errors["Name"].Should().Contain("The specified name already exists.");
    }

    [Test]
    public async Task ShouldUpdateTodoTag()
    {
        var userId = await RunAsDefaultUserAsync();

        var tagId = await SendAsync(new CreateTodoTagsCommand
        {
            Name = "New List"
        });

        var command = new UpdateTodoTagsCommand
        {
            Id = tagId,
            Name = "Updated List Title"
        };

        await SendAsync(command);

        var tag = await FindAsync<TodoTag>(tagId);

        tag.Should().NotBeNull();
        tag!.Name.Should().Be(command.Name);
        tag.LastModifiedBy.Should().NotBeNull();
        tag.LastModifiedBy.Should().Be(userId);
        tag.LastModified.Should().NotBeNull();
        tag.LastModified.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
    }
}
