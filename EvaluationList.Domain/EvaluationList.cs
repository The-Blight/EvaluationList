using System;
using System.Collections.Generic;
using System.Linq;
using EvaluationList.Domain.Entities.Participants;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Domain.Entities.Staff;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain;

/// <summary>
/// Главный агрегат системы.
/// Представляет собой оценочный лист конкретной выставки.
/// Управляет правилами добавления оценок и гарантирует целостность данных.
/// </summary>
public class EvaluationList : IEntity
{
    public required Guid Id { get; init; } = Guid.CreateVersion7();
    public required string ExhibitionTitle { get; init; }
    public required string AgeGroup { get; init; }

    private readonly List<Guid> _expertIds = [];
    private readonly List<Guid> _projectIds = [];
    
    private readonly List<Criterion> _criteria = [];
    private readonly List<Assessment> _assessments = [];

    public IReadOnlyCollection<Guid> ExpertIds => _expertIds;
    public IReadOnlyCollection<Guid> ProjectIds => _projectIds;
    public IReadOnlyCollection<Criterion> Criteria => _criteria;
    public IReadOnlyCollection<Assessment> Assessments => _assessments;

    
    /// <summary>
    /// Добавляет новую оценку в лист с предварительной проверкой бизнес-правил.
    /// </summary>
    /// <param name="assessment">Объект оценки.</param>
    /// <exception cref="InvalidOperationException">Выбрасывается, если критерий не найден в листе.</exception>
    /// <exception cref="ArgumentException">Выбрасывается, если балл превышает допустимый максимум.</exception>
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


    public void AddExpert(Guid expertId) => _expertIds.Add(expertId);
    public void AddProject(Guid projectId) => _projectIds.Add(projectId);
    public void AddCriterion(Criterion criterion) => _criteria.Add(criterion); 
    
    
    /// <summary>
    /// Фабричный метод для восстановления объекта из хранилища.
    /// Используется инфраструктурным слоем (Маппером) для десериализации.
    /// </summary>
    public static EvaluationList Restore(Guid id, string title, string ageGroup,
                                         IEnumerable<Guid> expertIds, IEnumerable<Guid> projectIds,
                                         IEnumerable<Criterion> criteria, IEnumerable<Assessment> assessments)
    {
        var sheet = new EvaluationList
        {
            Id = id, 
            ExhibitionTitle = title,
            AgeGroup = ageGroup

        }; 
        
        sheet._expertIds.AddRange(expertIds);
        sheet._projectIds.AddRange(projectIds);
        sheet._criteria.AddRange(criteria);
        sheet._assessments.AddRange(assessments);

        return sheet; 
    }
}