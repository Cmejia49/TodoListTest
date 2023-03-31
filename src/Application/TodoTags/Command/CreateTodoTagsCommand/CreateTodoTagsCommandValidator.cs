using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.TodoLists.Commands.CreateTodoList;

namespace Todo_App.Application.TodoTags.Command.CreateTodoTagsCommand;
public class CreateTodoTagsCommandValidator : AbstractValidator<CreateTodoTagsCommand>
{
    private readonly IApplicationDbContext _context;

    public CreateTodoTagsCommandValidator(IApplicationDbContext context)
    {
        _context = context;

        RuleFor(v => v.Name)
     .NotEmpty().WithMessage("Name is required.")
     .MaximumLength(25).WithMessage("Name must not exceed 25 characters.");
    }

}
