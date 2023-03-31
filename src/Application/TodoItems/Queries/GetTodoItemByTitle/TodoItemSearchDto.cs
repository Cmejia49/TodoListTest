using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.Common.Mappings;
using Todo_App.Domain.Entities;
using Todo_App.Domain.Enums;
using Todo_App.Domain.Events;

namespace Todo_App.Application.TodoItems.Queries.GetTodoItemByName;
public class TodoItemSearchDto : IMapFrom<TodoItem>
{
    public int ListId { get; set; }

    public int? TagsId { get; set; }

    public string? Title { get; set; }

    public string? Note { get; set; }

    public string? ItemColour { get; set; }

    public PriorityLevel Priority { get; set; }

    public bool Done { get; set; }

}
