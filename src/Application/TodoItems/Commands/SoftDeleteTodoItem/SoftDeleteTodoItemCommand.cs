using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Todo_App.Application.Common.Exceptions;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoItems.Commands.DeleteTodoItem;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Events;

namespace Todo_App.Application.TodoItems.Commands.SoftDeleteTodoItem;
public record SoftDeleteTodoItemCommand(int Id) : IRequest;

public class SoftDeleteTodoItemCommandHandler : IRequestHandler<SoftDeleteTodoItemCommand>
{
    private readonly IApplicationDbContext _context;

    public SoftDeleteTodoItemCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SoftDeleteTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoItem), request.Id);
        }


        entity.Status = 1;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
