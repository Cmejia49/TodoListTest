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

namespace Todo_App.Application.TodoLists.Commands.SoftDeleteTodoList;
public record SoftDeleteTodoListCommand(int Id) : IRequest;

public class SoftDeleteTodoListCommandHandler : IRequestHandler<SoftDeleteTodoListCommand>
{
    private readonly IApplicationDbContext _context;

    public SoftDeleteTodoListCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Unit> Handle(SoftDeleteTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoLists
            .FindAsync(new object[] { request.Id }, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(TodoLists), request.Id);
        }

       var todoItem = await _context.TodoItems.Where(x => x.ListId == request.Id).ToListAsync();
        foreach (var item in todoItem)
        {
           item.Status = 1;
        }
        entity.Status = 1;
        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}