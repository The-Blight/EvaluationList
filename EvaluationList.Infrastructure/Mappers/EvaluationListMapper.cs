using System;
using System.Linq;
using EvaluationList.Domain.Interfaces;
using EvaluationList.Infrastructure.DTOs;

namespace EvaluationList.Infrastructure.Mappers;


/// <summary>
/// Реализация маппера для Оценочного листа.
/// Инкапсулирует логику переноса данных между агрегатом и JSON-моделью.
/// </summary>
public class EvaluationListMapper : IMapper<Domain.EvaluationList, EvaluationListDto>
{
    
    /// <inheritdoc/>
    public EvaluationListDto ToDto(Domain.EvaluationList domain)
    {
        return new EvaluationListDto
        {
            Id = domain.Id,
            ExhibitionTitle = domain.ExhibitionTitle,
            AgeGroup = domain.AgeGroup,

            ExpertIds = domain.ExpertIds.ToList(),
            ProjectIds = domain.ProjectIds.ToList(),
            Criteria = domain.Criteria.ToList(),
            Assessments = domain.Assessments.ToList()
        };
    }

    /// <inheritdoc/>
    public Domain.EvaluationList ToDomain(EvaluationListDto dto)
    {
        return Domain.EvaluationList.Restore(
            dto.Id,
            dto.ExhibitionTitle,
            dto.AgeGroup,
            dto.ExpertIds,
            dto.ProjectIds,
            dto.Criteria,
            dto.Assessments
        );
    }
}