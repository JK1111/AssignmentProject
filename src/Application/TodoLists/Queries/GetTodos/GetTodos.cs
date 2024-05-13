using System.Threading;
using Assignment.Application.Common.Interfaces;
using Assignment.Application.Common.Security;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

[Authorize]
public record GetTodosQuery : IRequest<IList<TodoListDto>>;

public class GetTodosQueryHandler : IRequestHandler<GetTodosQuery, IList<TodoListDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public GetTodosQueryHandler(IApplicationDbContext context, IMapper mapper, IMemoryCache memoryCache)
    {
        _context = context;
        _mapper = mapper;
        _memoryCache = memoryCache;
    }

    public async Task<IList<TodoListDto>> Handle(GetTodosQuery request, CancellationToken cancellationToken)
    {
        IList<TodoListDto>? output = new List<TodoListDto>();

        output = _memoryCache.Get<List<TodoListDto>>("TodoLists") as List<TodoListDto>;

        if (output is null)
        {
            output = await GetTodoListFromDB(cancellationToken);

            _memoryCache.Set("TodoLists", output, TimeSpan.FromMinutes(10));
        }
        return output;
    }

    private async Task<IList<TodoListDto>> GetTodoListFromDB(CancellationToken cancellationToken)
    {
        return await _context.TodoLists
            .AsNoTracking()
            .ProjectTo<TodoListDto>(_mapper.ConfigurationProvider)
            .OrderBy(t => t.Title)
            .ToListAsync(cancellationToken);
    }
}
