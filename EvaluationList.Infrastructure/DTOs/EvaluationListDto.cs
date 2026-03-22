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
    public Guid Id { get; init; }
    public required string ExhibitionTitle { get; init; }
    public required string AgeGroup { get; init; }

    public List<Guid> ExpertIds { get; init; } = []; 
    public List<Guid> ProjectIds { get; init; } = [];

    public List<Criterion> Criteria { get; init; } = [];
    public List<Assessment> Assessments { get; init; } = []; 
}