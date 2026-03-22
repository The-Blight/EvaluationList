using System;
using System.Collections.Generic;
using System.Linq;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Domain.Interfaces;

namespace EvaluationList.Domain;

/// <summary>
/// Корень агрегации: Оценочный лист выставки.
/// Инкапсулирует бизнес-правила добавления оценок и управляет связями (через ID).
/// </summary>
public class EvaluationList : IEntity
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.CreateVersion7();
    
    /// <summary>Название мероприятия или выставки.</summary>
    public required string ExhibitionTitle { get; init; }
    
    /// <summary>Возрастная категория участников.</summary>
    public required string AgeGroup { get; init; }

    // Храним внешние зависимости только по ID, чтобы избежать дублирования данных
    private readonly List<Guid> _expertIds = [];
    private readonly List<Guid> _projectIds = [];
    
    private readonly List<Criterion> _criteria = [];
    private readonly List<Assessment> _assessments = [];

    /// <summary>Идентификаторы прикрепленных экспертов (Только для чтения).</summary>
    public IReadOnlyCollection<Guid> ExpertIds => _expertIds;
    
    /// <summary>Идентификаторы оцениваемых проектов (Только для чтения).</summary>
    public IReadOnlyCollection<Guid> ProjectIds => _projectIds;
    
    /// <summary>Список утвержденных критериев (Только для чтения).</summary>
    public IReadOnlyCollection<Criterion> Criteria => _criteria;
    
    /// <summary>Список выставленных оценок (Только для чтения).</summary>
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

        if (!_expertIds.Contains(assessment.ExpertId))
            throw new InvalidOperationException("Эксперт не прикреплен к этому листу.");

        if (!_projectIds.Contains(assessment.ProjectId))
            throw new InvalidOperationException("Проект не прикреплен к этому листу.");

        if (assessment.Value > criterion.MaxScore)
        {
            throw new ArgumentException(
                $"Оценка {assessment.Value} превышает максимум ({criterion.MaxScore}) для '{criterion.CriterionName}'.");
        }

        if (_assessments.Any(a => a.ProjectId == assessment.ProjectId && 
                                  a.ExpertId == assessment.ExpertId && 
                                  a.CriterionId == assessment.CriterionId))
        {
            throw new InvalidOperationException("Оценка по данному критерию от этого эксперта для этого проекта уже выставлена.");
        }

        _assessments.Add(assessment);
    }


    /// <summary>Прикрепляет эксперта к листу по ID.</summary>
    public void AddExpert(Guid expertId) => _expertIds.Add(expertId);
    
    /// <summary>Прикрепляет проект к листу по ID.</summary>
    public void AddProject(Guid projectId) => _projectIds.Add(projectId);
    
    /// <summary>Добавляет новый критерий оценивания.</summary>
    public void AddCriterion(Criterion criterion) => _criteria.Add(criterion); 
    
    
    /// <summary>
    /// Фабричный метод для восстановления объекта из хранилища о всеми приватными коллекциями
    /// Используется инфраструктурным слоем (Маппером <see cref="IMapper{TEntity,TDto}"/>) для десериализации.
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