using System;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain.Entities.Scoring;

/// <summary>
/// Объект-значения: Конкретная оценка, выставленная экспертом за проект.
/// </summary>
public record Assessment : IEntity
{
    /// <inheritdoc/>
    public required Guid Id { get; init; } = Guid.CreateVersion7();
    
    /// <summary>Идентификатор оцениваемого проекта.</summary>
    public required Guid ProjectId { get; init; }
    
    /// <summary>Идентификатор критерия, по которому выставлена оценка.</summary>
    public required Guid CriterionId { get; init; }
    
    /// <summary>Идентификатор эксперта, выставившего оценку.</summary>
    public required Guid ExpertId { get; init; }

    
    /// <summary>
    /// Значение оценки (количество баллов). 
    /// Не может быть отрицательным.
    /// </summary>
    public required double Value
    {
        get => field;
        init
        {
            if (value < 0)
                throw new ArgumentException("Оценка должна быть положительным числом.");
            
            field = value;
        }
    }
}