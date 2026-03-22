using System;
using System.Collections.Generic;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Infrastructure.DTOs;

/// <summary>
/// Объект передачи данных (DTO) для сериализации оценочного листа.
/// Служит структурой данных без поведения для JSON сериализации.
/// </summary>
public record EvaluationListDto : IDto
{
    /// <inheritdoc/>>
    public Guid Id { get; init; }
    
    /// <summary>Название выставки.</summary>
    public required string ExhibitionTitle { get; init; }
    
    /// <summary>Возрастная группа.</summary>
    public required string AgeGroup { get; init; }

    /// <summary>Список ID экспертов для сохранения.</summary>
    public List<Guid> ExpertIds { get; init; } = []; 
    
    /// <summary>Список ID проектов для сохранения.</summary>
    public List<Guid> ProjectIds { get; init; } = [];

    /// <summary>Список критериев для сохранения.</summary>
    public List<Criterion> Criteria { get; init; } = [];
    
    /// <summary>Список выставленных оценок для сохранения.</summary>
    public List<Assessment> Assessments { get; init; } = []; 
}