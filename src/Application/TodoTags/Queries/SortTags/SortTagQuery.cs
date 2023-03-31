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
using Todo_App.Application.TodoTags.Queries.GetTags;

namespace Todo_App.Application.TodoTags.Queries.SortTags;
public record SortTagQuery : IRequest<SortTagVm>
{
    public int sortType { get; init; }
}

    public class GetSortTagQueryHandler : IRequestHandler<SortTagQuery, SortTagVm>
    {
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetSortTagQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<SortTagVm> Handle(SortTagQuery request, CancellationToken cancellationToken)
    {
        IList<SortTagsDto> TodoTags = new List<SortTagsDto>();
        if (request.sortType == 1)
        {
            TodoTags = await _context.TodoTags
                   .OrderByDescending(x => x.Items.Count())
                  .AsNoTracking()
                   .ProjectTo<SortTagsDto>(_mapper.ConfigurationProvider)
                    .ToListAsync(cancellationToken);
        }
        else if (request.sortType == 2)
        {

            TodoTags = await _context.TodoTags
                                .OrderBy(x => x.Items.Count())
                               .AsNoTracking()
                                .ProjectTo<SortTagsDto>(_mapper.ConfigurationProvider)
                                 .ToListAsync(cancellationToken);
        }

        return new SortTagVm
        {
            TodoTags = TodoTags,
        };
    }
}
