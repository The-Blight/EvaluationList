using System;
using System.Collections.Generic;
using System.Linq;
using EvaluationList.Domain.Entities.Participants;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Domain.Entities.Staff;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain;

/// <summary>
/// Основной оценочный документ.
/// </summary>
public class EvaluationList : IEntity
{
    public required Guid Id { get; init; } = Guid.CreateVersion7();
    public required string ExhibitionTitle { get; init; }
    public required string AgeGroup { get; init; }

    private readonly List<Expert> _experts = [];
    private readonly List<Project> _projects = [];
    private readonly List<Criterion> _criteria = [];
    private readonly List<Assessment> _assessments = [];

    public IReadOnlyCollection<Expert> Experts => _experts;
    public IReadOnlyCollection<Project> Projects => _projects;
    public IReadOnlyCollection<Criterion> Criteria => _criteria;
    public IReadOnlyCollection<Assessment> Assessments => _assessments;

    public void AddAssessment(Assessment assessment)
    {
        var criterion = _criteria.FirstOrDefault(c => c.Id == assessment.CriterionId)
                        ?? throw new InvalidOperationException("Критерий не найден в этом листе.");

        if (assessment.Value > criterion.MaxScore)
        {
            throw new ArgumentException(
                $"Оценка {assessment.Value} превышает максимум ({criterion.MaxScore}) для '{criterion.CriterionName}'.");
        }

        _assessments.Add(assessment);
    }


    public void AddExpert(Expert expert) => _experts.Add(expert);
    public void AddProject(Project project) => _projects.Add(project);
    public void AddCriterion(Criterion criterion) => _criteria.Add(criterion); 
}