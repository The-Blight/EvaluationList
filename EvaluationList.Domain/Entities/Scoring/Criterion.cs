using System;
using EvaluationList.Domain.Interfaces;
using Microsoft.VisualBasic;

namespace EvaluationList.Domain.Entities.Scoring;

/// <summary>
/// Класс представляет критерий оценки.
/// </summary>
public record Criterion : IEntity
{
    public required Guid Id { get; init; } = Guid.CreateVersion7();

    public required string CriterionName
    {
        get => field;
        init => field = value?.Trim() ?? string.Empty;
    }

    public required double MaxScore
    {
        get => field;
        init
        {
            if (value <= 0)
                throw new ArgumentException("Максимальный балл должен быть положительным числом.");

            field = value; 
        }
    }
}