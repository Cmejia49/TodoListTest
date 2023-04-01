﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.Common.Mappings;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Entities;

namespace Todo_App.Application.TodoTags.Queries.GetTags;
public class TodoTagsDto : IMapFrom<TodoTag>
{
    public int Id { get; set; }
    public string? name { get; set; }

}