using Assignment.Application.Common.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Application.TodoItems.Commands.DoneTodoItem;

public record DoneTodoItemCommand(int Id) : IRequest;

public class DoneTodoItemCommandHandler : IRequestHandler<DoneTodoItemCommand>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public DoneTodoItemCommandHandler(IApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task Handle(DoneTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.TodoItems
            .FindAsync(new object[] { request.Id }, cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        entity.Done = true;

        await _context.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove("TodoLists");
    }
}
