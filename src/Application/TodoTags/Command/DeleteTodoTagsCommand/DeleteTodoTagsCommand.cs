using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Events;

namespace Todo_App.Application.TodoTags.Command.DeleteTodoTagsCommand;

public record DeleteTodoTagsCommand(int Id) : IRequest;

public class DeleteTodoTagsCommandHandler : IRequestHandler<DeleteTodoTagsCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteTodoTagsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(DeleteTodoTagsCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoTags
              .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }

        _context.TodoTags.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
