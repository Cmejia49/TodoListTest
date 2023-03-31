using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Todo_App.Application.TodoTags.Queries.GetTags;

namespace Todo_App.Application.TodoTags.Queries.SortTags;
public class SortTagVm
{
    public IList<SortTagsDto> TodoTags { get; set; } = new List<SortTagsDto>();
}
