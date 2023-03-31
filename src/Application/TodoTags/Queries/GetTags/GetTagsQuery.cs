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
using Todo_App.Application.TodoItems.Queries.GetTodoItemsWithPagination;
using Todo_App.Application.TodoLists.Queries.GetTodos;
using Todo_App.Domain.Enums;

namespace Todo_App.Application.TodoTags.Queries.GetTags;


public record GetTagsQuery : IRequest<TodoTagVm>;
public class GetTodosQueryHandler : IRequestHandler<GetTagsQuery, TodoTagVm>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }


    public async Task<TodoTagVm> Handle(GetTagsQuery request, CancellationToken cancellationToken)
    {

        return new TodoTagVm
        {
            TodoTags = await _context.TodoTags
              .AsNoTracking()
               .ProjectTo<TodoTagsDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken)

        };

        
    }
}