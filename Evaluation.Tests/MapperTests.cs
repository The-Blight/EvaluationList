using System;
using System.Linq;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Infrastructure.Mappers;
using Xunit;

namespace Evaluation.Tests;

public class MapperTests
{
    [Fact]
    public void EvaluationListMapper_ToDtoAndBack_ShouldRetainAllData()
    {
        var mapper = new EvaluationListMapper();
        var originalSheet = new EvaluationList.Domain.EvaluationList
        {
            ExhibitionTitle = "Тестовая выставка",
            AgeGroup = "Студенты",
            Id = default
        };

        var expertId = Guid.NewGuid();
        var projectId = Guid.NewGuid();
        var criterion = new Criterion
        {
            CriterionName = "Дизайн",
            MaxScore = 10,
            Id = default
        };
        var assessment = new Assessment
        {
            ProjectId = projectId,
            CriterionId = criterion.Id,
            ExpertId = expertId,
            Value = 9,
            Id = default
        };

        originalSheet.AddExpert(expertId);
        originalSheet.AddProject(projectId);
        originalSheet.AddCriterion(criterion);
        originalSheet.AddAssessment(assessment);

        var dto = mapper.ToDto(originalSheet);
        var restoredSheet = mapper.ToDomain(dto);

        Assert.Multiple(
            () => Assert.Equal(originalSheet.Id, restoredSheet.Id),
            () => Assert.Equal(originalSheet.ExhibitionTitle, restoredSheet.ExhibitionTitle),
            () => Assert.Equal(originalSheet.AgeGroup, restoredSheet.AgeGroup),
            () => Assert.Single(restoredSheet.ExpertIds),
            () => Assert.Equal(expertId, restoredSheet.ExpertIds.First()),
            () => Assert.Single(restoredSheet.Criteria),
            () => Assert.Equal(criterion.CriterionName, restoredSheet.Criteria.First().CriterionName),
            () => Assert.Single(restoredSheet.Assessments),
            () => Assert.Equal(9, restoredSheet.Assessments.First().Value)
        );
    }
}