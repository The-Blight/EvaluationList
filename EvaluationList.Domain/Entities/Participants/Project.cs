using System;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain.Entities.Participants;


/// <summary>
/// Класс представляет проект участника на выставке.
/// </summary>
public class Project : IEntity
{

    public required Guid Id { get; init; } = Guid.CreateVersion7();

    public required string Title 
    {
        get => field;
        init => field = value?.Trim() ?? string.Empty; 
    }

    public required string Author
    {
        get => field;
        init => field = value?.Trim() ?? string.Empty;
    }

    public required string GroupName { get; init; }
    
}