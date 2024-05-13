using Assignment.Application.Common.Interfaces;
using Assignment.Domain.Entities;
using Assignment.Domain.Enums;
using Assignment.Domain.Events;
using Microsoft.Extensions.Caching.Memory;

namespace Assignment.Application.TodoItems.Commands.CreateTodoItem;

public record CreateTodoItemCommand : IRequest<int>
{
    public int ListId { get; init; }

    public string? Title { get; init; }
    public string? Note { get; init; }
    public PriorityLevel Priority { get; set; }
}

public class CreateTodoItemCommandHandler : IRequestHandler<CreateTodoItemCommand, int>
{
    private readonly IApplicationDbContext _context;
    private readonly IMemoryCache _memoryCache;

    public CreateTodoItemCommandHandler(IApplicationDbContext context, IMemoryCache memoryCache)
    {
        _context = context;
        _memoryCache = memoryCache;
    }

    public async Task<int> Handle(CreateTodoItemCommand request, CancellationToken cancellationToken)
    {
        var entity = new TodoItem
        {
            ListId = request.ListId,
            Title = request.Title,
            Priority = request.Priority,
            Note = request.Note,
            Done = false
        };

        entity.AddDomainEvent(new TodoItemCreatedEvent(entity));

        _context.TodoItems.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        _memoryCache.Remove("TodoLists");

        return entity.Id;
    }
}
