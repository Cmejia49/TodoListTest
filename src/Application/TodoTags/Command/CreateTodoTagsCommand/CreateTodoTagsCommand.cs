using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Events;

namespace Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
public class CreateTodoTagsCommand : IRequest<int>
{

    public string Name { get; set; }

}

public class CreateTodoTagsCommandHandler : IRequestHandler<CreateTodoTagsCommand, int>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoTagsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> Handle(CreateTodoTagsCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoTag
        {
            Name = request.Name,
        };


        _context.TodoTags.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return entity.Id;
    }
}
