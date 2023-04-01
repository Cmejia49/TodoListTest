using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;

namespace Todo_App.Application.TodoItems.Commands.UpdateTodoItemDetail;
public class UpdateTodoItemDetailCommandValidation : AbstractValidator<UpdateTodoItemDetailCommand>
{

    public UpdateTodoItemDetailCommandValidation()
    {

        RuleFor(v => v.ItemColour)
       .Must(BeValidHexColor).WithMessage("Color Value Invalid.");
    }

    private bool BeValidHexColor(string? color)
    {
        if (string.IsNullOrEmpty(color))
        {
            return true;
        }
        else
        {
            return Regex.Match(color, "^#[0-9a-fA-F]{6}$").Success;
        }
    }


}
