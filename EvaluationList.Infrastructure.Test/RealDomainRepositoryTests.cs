using System;
using System.IO;
using System.Linq;
using EvaluationList.Domain.Entities.Scoring;
using EvaluationList.Infrastructure.DTOs;
using EvaluationList.Infrastructure.Mappers;
using EvaluationList.Infrastructure.Repositories;
using Xunit;

namespace EvaluationList.Infrastructure.Test;

public class RealDomainRepositoryTests : IDisposable
{
    private readonly string _testFileName;
    private readonly JsonRepository<Domain.EvaluationList, EvaluationListDto> _repository;

    public RealDomainRepositoryTests()
    {
        _testFileName = Path.Combine(Path.GetTempPath(), $"real_domain_test_{Guid.CreateVersion7()}.json");
        var mapper = new EvaluationListMapper();
        _repository = new JsonRepository<Domain.EvaluationList, EvaluationListDto>(_testFileName, mapper);
    }

    public void Dispose()
    {
        if (File.Exists(_testFileName))
            File.Delete(_testFileName);
    }

    [Fact]
    public void CanSaveAndLoadRealDomainObject()
    {
        var evaluationList = new Domain.EvaluationList
        {
            Id = Guid.CreateVersion7(),
            ExhibitionTitle = "Real Exhibition",
            AgeGroup = "Adults"
        };

        var criterion = new Criterion
        {
            Id = Guid.CreateVersion7(),
            CriterionName = "Design",
            MaxScore = 10
        };
        evaluationList.AddCriterion(criterion);

        var expertId = Guid.CreateVersion7();
        var projectId = Guid.CreateVersion7();
        evaluationList.AddExpert(expertId);
        evaluationList.AddProject(projectId);

        var assessment = new Assessment
        {
            Id = Guid.CreateVersion7(),
            CriterionId = criterion.Id,
            ExpertId = expertId,
            ProjectId = projectId,
            Value = 8.5
        };
        evaluationList.AddAssessment(assessment);


        _repository.Create(evaluationList);
        var loaded = _repository.GetById(evaluationList.Id);

        Assert.Multiple(
            () => Assert.NotNull(loaded),
            () => Assert.Equal(evaluationList.Id, loaded!.Id),
            () => Assert.Equal(evaluationList.ExhibitionTitle, loaded!.ExhibitionTitle),
            () => Assert.Single(loaded!.Criteria),
            () => Assert.Equal(criterion.CriterionName, loaded!.Criteria.First().CriterionName),
            () => Assert.Single(loaded!.Assessments),
            () => Assert.Equal(assessment.Value, loaded!.Assessments.First().Value)
        );
    }
}