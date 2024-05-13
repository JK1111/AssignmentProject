using Assignment.Domain.Entities;
using FluentValidation.Validators;

namespace Assignment.Application.TodoLists.Queries.GetTodos;

public class TodoListDto
{
    public TodoListDto()
    {
        Items = Array.Empty<TodoItemDto>();
    }

    public int Id { get; init; }

    public string? Title { get; init; }

    public string? Colour { get; init; }

    public IList<TodoItemDto> Items { get; init; }

    public DateTimeOffset LastModified { get; set; }

    // Cannot be incorporate in EntityFramework due to SQLimitation https://learn.microsoft.com/en-us/ef/core/providers/sqlite/limitations
    public IList<DateTimeOffset> LastModificationOfItems { get; set; } = [];

    public DateTimeOffset LastModification
    {
        get
        {
            return LastModificationOfItems.Union([LastModified]).Max();
        }
    }

    private class Mapping : Profile
    {
        public Mapping()
        {
            CreateMap<TodoList, TodoListDto>().
                ForMember(dest => dest.LastModificationOfItems, 
                            opts => opts.MapFrom(src => src.Items
                                .Select(c => c.LastModified )
                                .ToArray()));
        }
    }
}
