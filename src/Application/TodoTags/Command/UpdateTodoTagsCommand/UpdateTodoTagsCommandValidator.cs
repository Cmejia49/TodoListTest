using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoTags.Command.UpdateTodoTagsCommand;
public class UpdateTodoTagsCommandValidator : AbstractValidator<UpdateTodoTagsCommand>
{

    private readonly IApplicationDbContext _context;

    public UpdateTodoTagsCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
        .NotEmpty().WithMessage("Name is required.")
        .MaximumLength(25).WithMessage("Name must not exceed 25 characters.")
        .MustAsync(BeUniqueName).WithMessage("The specified name already exists."); 

    }

    public async Task<bool> BeUniqueName(UpdateTodoTagsCommand model,string name, CancellationToken cancellationToken)
    {
        return await _context.TodoTags
            .Where(l => l.Id != model.Id)
            .AllAsync(l => l.Name != name, cancellationToken);
    }
}
