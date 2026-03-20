using System;
using EvaluationList.Domain.Interfaces;
using Microsoft.VisualBasic;

namespace EvaluationList.Domain.Entities.Scoring;

/// <summary>
/// Объект-значение: Критерий оценки.
/// Жестко привязан к Оценочному листу.
/// </summary>
public record Criterion : IEntity
{
    
    /// <inheritdoc/>
    public required Guid Id { get; init; } = Guid.CreateVersion7();

    
    /// <summary>
    /// Название или описание критерия (например, "Дизайн", "Техническая сложность").
    /// </summary>
    public required string CriterionName
    {
        get => field;
        init => field = value?.Trim() ?? string.Empty;
    }

    
    /// <summary>
    /// Максимально возможный балл по данному критерию.
    /// </summary>
    /// <exception cref="ArgumentException">Выбрасывается, если балл меньше или равен 0.</exception>
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