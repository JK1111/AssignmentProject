using Assignment.Application.Common.Interfaces;
using Assignment.Domain.Entities;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Application.TodoLists.Commands.CreateTodoList;

public record CreateTodoListCommand(string Title) : IRequest<int>;

public class CreateTodoListCommandHandler : IRequestHandler<CreateTodoListCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CreateTodoListCommandHandler(IApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<int> Handle(CreateTodoListCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoList();

        entity.Title = request.Title;

        _context.TodoLists.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);
        _memoryCache.Remove("TodoLists");

        return entity.Id;
    }
}
