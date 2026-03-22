using System;
using System.Linq;
using Xunit;
using EvaluationList.Domain;
using EvaluationList.Domain.Entities.Scoring;

namespace Evaluation.Tests;

public class EvaluationLIstTests
{
    private readonly EvaluationList.Domain.EvaluationList _sheet;
    private readonly Criterion _testCriterion;
    
    public EvaluationLIstTests()
    {
        _sheet = new EvaluationList.Domain.EvaluationList
        {
            ExhibitionTitle = "Выставка 2026",
            AgeGroup = "Юниоры",
            Id = default
        };

        _testCriterion = new Criterion
        {
            CriterionName = "Техническая сложность",
            MaxScore = 5.0,
            Id = default
        };

        _sheet.AddCriterion(_testCriterion);
    }
    
    
   [ Fact]
    public void AddAssessment_ValidScore_ShouldAddSuccessfully()
    {
       
        var assessment = new Assessment
        {
            ProjectId = Guid.NewGuid(),
            CriterionId = _testCriterion.Id,
            ExpertId = Guid.NewGuid(),
            Value = 4.5,
            Id = default 
        };
        
        _sheet.AddAssessment(assessment);

        Assert.Single(_sheet.Assessments);
        Assert.Equal(4.5, _sheet.Assessments.First().Value);
    }
    
    [Fact]
    public void AddAssessment_ScoreExceedsMax_ShouldThrowArgumentException()
    {
        
        var assessment = new Assessment
        {
            ProjectId = Guid.NewGuid(),
            CriterionId = _testCriterion.Id,
            ExpertId = Guid.NewGuid(),
            Value = 6.0,
            Id = default 
        };

        var exception = Assert.Throws<ArgumentException>(() => 
            _sheet.AddAssessment(assessment));

        Assert.Contains("превышает максимум", exception.Message);
    }
    
    [Fact]
    public void AddAssessment_UnknownCriterion_ShouldThrowInvalidOperationException()
    {
        var assessment = new Assessment
        {
            ProjectId = Guid.NewGuid(),
            CriterionId = Guid.NewGuid(),
            ExpertId = Guid.NewGuid(),
            Value = 3.0,
            Id = default
        };

        Assert.Throws<InvalidOperationException>(() => 
            _sheet.AddAssessment(assessment));
    }
    
    
    
}
