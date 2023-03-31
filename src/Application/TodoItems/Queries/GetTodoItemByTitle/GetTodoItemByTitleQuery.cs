using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Todo_App.Application.Common.Interfaces;
using Todo_App.Application.Common.Mappings;
using Todo_App.Application.Common.Models;
using Todo_App.Application.TodoItems.Queries.GetTodoItemByName;
using Todo_App.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Todo_App.Application.TodoLists.Queries.GetTodos;

namespace Todo_App.Application.TodoItems.Queries.GetTodoItemByName;
public class GetTodoItemByTitleQuery  : IRequest<PaginatedList<TodoItemSearchDto>>
{
    public int ListId { get; init; }
    public string title { get; init; } = "";
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}


public class GetTodoItemByTitleQueryHandler : IRequestHandler<GetTodoItemByTitleQuery, PaginatedList<TodoItemSearchDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodoItemByTitleQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }



    public async Task<PaginatedList<TodoItemSearchDto>> Handle(GetTodoItemByTitleQuery request, CancellationToken cancellationToken)
    {
        return await _context.TodoItems
            .Where(x => x.Title.Contains(request.title) && x.ListId == request.ListId)
             .OrderBy(x => x.Title)
             .ProjectTo<TodoItemSearchDto>(_mapper.ConfigurationProvider)
          .PaginatedListAsync(request.PageNumber, request.PageSize);
    }
}
