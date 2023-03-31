using MediatR;
using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;
using Todo_App.Application.TodoLists.Queries.ExportTodos;
using Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
using Todo_App.Application.TodoTags.Command.DeleteTodoTagsCommand;
using Todo_App.Application.TodoTags.Command.UpdateTodoTagsCommand;
using Todo_App.Application.TodoTags.Queries.GetTags;
using Todo_App.Application.TodoTags.Queries.SortTags;

namespace Todo_App.WebUI.Controllers;
public class TodoTagsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<TodoTagVm>> Get()
    {
        return await Mediator.Send(new GetTagsQuery());
    }

    [HttpGet("{sortType}")]
    public async Task<ActionResult<SortTagVm>> Get(int sortType)
    {
        return await Mediator.Send(new SortTagQuery { sortType = sortType });
    }


    [HttpPost]
    public async Task<ActionResult<int>> Create(CreateTodoTagsCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut("[action]")]
    public async Task<ActionResult> Update(int id, UpdateTodoTagsCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest();
        }

        await Mediator.Send(command);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        await Mediator.Send(new DeleteTodoTagsCommand(id));

        return NoContent();
    }

}
