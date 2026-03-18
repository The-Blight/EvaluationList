using System;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain.Entities.Scoring;

/// <summary>
/// Класс представляет конкретный балл, выставленный за проект по определенному критерию.
/// </summary>
public class Assessment : IEntity
{
    public required Guid Id { get; init; } = Guid.CreateVersion7();
    public required Guid ProjectId { get; init; }
    public required Guid CriterionId { get; init; }
    public required Guid ExpertId { get; init; }

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