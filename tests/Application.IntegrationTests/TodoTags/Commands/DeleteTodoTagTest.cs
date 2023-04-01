using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
using Todo_App.Application.TodoTags.Command.DeleteTodoTagsCommand;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.IntegrationTests.TodoTags.Commands;
using static Testing;

public class DeleteTodoTagTest : BaseTestFixture
{
    [Test]
    public async Task ShouldRequireValidTodoTagId()
    {
        var command = new DeleteTodoTagsCommand(99);
        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ShouldDeleteTodoTag()
    {
        var tagId = await SendAsync(new CreateTodoTagsCommand
        {
            Name = "New Tag"
        });

        await SendAsync(new DeleteTodoTagsCommand(tagId));

        var tag = await FindAsync<TodoTag>(tagId);

        tag.Should().BeNull();
    }
}