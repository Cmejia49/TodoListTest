using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoTags.Command.UpdateTodoTagsCommand;
public class UpdateTodoTagsCommand : IRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
}

public class UpdateTodoTagsCommandHandler : IRequestHandler<UpdateTodoTagsCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateTodoTagsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(UpdateTodoTagsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoTags
      .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoTag), request.Id);
        }

        entity.Name = request.Name;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
