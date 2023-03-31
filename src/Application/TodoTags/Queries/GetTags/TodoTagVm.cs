using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.TodoLists.Queries.GetTodos;

namespace Todo_App.Application.TodoTags.Queries.GetTags;
public class TodoTagVm
{
    public IList<TodoTagsDto> TodoTags { get; set; } = new List<TodoTagsDto>();
}
